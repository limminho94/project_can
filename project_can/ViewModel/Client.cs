using OpenCvSharp;
using OpenCvSharp.Extensions;
using project_can.ViewModel.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace project_can.ViewModel
{
    public class Client : INotifyPropertyChanged
    {
        // 카메라 관련 변수
        private VideoCapture? _camera;
        private Mat? _frame;
        private bool _isCameraRunning;

        private BitmapImage? _viewCam; // 이미지 바인딩 데이터

        // TCP 관련 변수
        private TcpClient? _tcpClient;
        private NetworkStream? _stream;
        private bool _isConnected = false; // 서버 연결 상태

        // ListView 바인딩 데이터
        private ObservableCollection<string> _canNames = new ObservableCollection<string>();
        public ObservableCollection<string> CanNames
        {
            get => _canNames;
            set
            {
                _canNames = value;
                OnPropertyChanged(nameof(CanNames));
            }
        }

        // ViewCam 바인딩 속성
        public BitmapImage? ViewCam
        {
            get => _viewCam;
            set
            {
                _viewCam = value;
                OnPropertyChanged(nameof(ViewCam));
            }
        }

        // 명령어 정의
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public Client()
        {
            // 명령어 바인딩
            StartCommand = new RelayCommand(async _ => await StartCameraAsync());
            StopCommand = new RelayCommand(async _ => await StopCameraAsync());
        }

        // 카메라 시작
        private async Task StartCameraAsync()
        {
            if (_isCameraRunning)
            {
                MessageBox.Show("카메라가 이미 실행 중입니다.");
                return;
            }

            // TCP 서버 연결
            await ConnectServerAsync();

            _camera = new VideoCapture(0);
            _frame = new Mat();

            if (!_camera.IsOpened())
            {
                MessageBox.Show("카메라를 열 수 없습니다.");
                return;
            }

            _isCameraRunning = true;

            // 카메라 스트림 시작
            await Task.Run(() => CaptureFramesAsync());
        }

        // 서버 연결
        private async Task ConnectServerAsync()
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync("10.10.20.116", 12345); // 서버 IP와 포트 설정
                _stream = _tcpClient.GetStream();
                _isConnected = true;

                // 서버 메시지 수신 시작
                _ = ReceiveMessageAsync(); // 수신 함수 비동기 실행
            }
            catch (Exception ex)
            {
                MessageBox.Show($"서버 연결 실패: {ex.Message}");
                _isConnected = false;
            }
        }

        // 프레임 캡처 및 전송
        private async Task CaptureFramesAsync()
        {
            while (_isCameraRunning && _camera != null && _camera.IsOpened())
            {
                _camera.Read(_frame); 
                if (_frame.Empty())
                    continue;

                // UI 업데이트
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ViewCam = ConvertBitmapToImageSource(BitmapConverter.ToBitmap(_frame));
                });

                if (_isConnected && _stream != null)
                {
                    try
                    {
                        // 이미지를 바이트 배열로 변환하여 서버에 전송
                        byte[] buffer = ConvertBitmapToBytes(BitmapConverter.ToBitmap(_frame));

                        // 먼저 이미지 크기 전송
                        byte[] sizeInfo = BitConverter.GetBytes(buffer.Length);
                        await _stream.WriteAsync(sizeInfo, 0, sizeInfo.Length);

                        // 그 다음 실제 이미지 데이터 전송
                        await _stream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"프레임 전송 실패: {ex.Message}");
                    }
                }

                await Task.Delay(100);
            }
        }

        // 이미지 변환 (Bitmap -> BitmapImage)
        private BitmapImage ConvertBitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        // Bitmap을 바이트 배열로 변환
        private byte[] ConvertBitmapToBytes(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg); // JPG로 저장
                return memoryStream.ToArray();
            }
        }

        // 데이터 수신
        private async Task ReceiveMessageAsync()
        {
            if (!_isConnected || _stream == null)
                return;

            byte[] buffer = new byte[1024]; // 메시지 버퍼
            try
            {
                while (_isConnected)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        // 수신된 데이터를 문자열로 변환
                        string message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            // 리스트뷰에 추가
                            CanNames.Add(message);
                        });

                        
                    }
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"메시지 수신 오류: {ex.Message}");
                });
                _isConnected = false;
            }
        }

        // 카메라 중지
        private async Task StopCameraAsync()
        {
            if (!_isCameraRunning)
            {
                MessageBox.Show("카메라가 실행 중이 아닙니다.");
                return;
            }

            _isCameraRunning = false;

            // 서버 연결 해제
            if (_stream != null)
            {
                await _stream.FlushAsync();
                _stream.Close();
            }

            _tcpClient?.Close();
            _isConnected = false;

            // 카메라 종료
            _camera?.Release();
            //MessageBox.Show("카메라가 종료되었습니다.");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
