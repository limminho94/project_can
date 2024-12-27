using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static System.Net.Mime.MediaTypeNames;


using System.Drawing;
using System.IO;
using System.Windows.Threading;

using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Net.Sockets;
using System.Net;

namespace project_can
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private VideoCapture capVideo;
        private VideoCapture capCamera;
        
        Mat matImage = new Mat();

        public MainWindow()
        {
            InitializeComponent();
            InitializeCamera();
            SendToServer();
        }

        private void InitializeCamera()
        {
            capCamera = new VideoCapture(0);
        }

        private void btPlay_Click(object sender, RoutedEventArgs e)
        {
            btPlay.IsEnabled = false;
            new Thread(PlayCamera).Start();
        }

        private void PlayCamera()
        {
            while (!capCamera.IsDisposed)
            {
                capCamera.Read(matImage); 
                if (matImage.Empty()) break;
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    var converted = Convert(BitmapConverter.ToBitmap(matImage));
                    imgViewport.Source = converted;
                }));
            }
        }

        private void btStop_Click(object sender, RoutedEventArgs e)
        {
            if (capCamera.IsOpened())
            {
                capCamera.Dispose();
            }
            btStop.IsEnabled = false;
            
            this.Close();
        }

        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        // 소켓을 생성한다.
        public void SendToServer()
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Connect 함수로 로컬(127.0.0.1)의 포트 번호 9999로 대기 중인 socket에 접속한다.
            client.Connect(new IPEndPoint(IPAddress.Parse("10.10.20.116"), 12345));
            // 보낼 메시지를 UTF8타입의 byte 배열로 변환한다.
            var data = Encoding.UTF8.GetBytes("클라이언트에서 보내는 메세지");

            // big엔디언으로 데이터 길이를 변환하고 서버로 보낼 데이터의 길이를 보낸다. (4byte)
            client.Send(BitConverter.GetBytes(data.Length));
            // 데이터를 전송한다.
            client.Send(data);

            // 데이터의 길이를 수신하기 위한 배열을 생성한다. (4byte)
            data = new byte[4];
            // 데이터의 길이를 수신한다.
            client.Receive(data, data.Length, SocketFlags.None);
            // server에서 big엔디언으로 전송을 했는데도 little 엔디언으로 온다. big엔디언과 little엔디언은 배열의 순서가 반대이므로 reverse한다.
            Array.Reverse(data);
            // 데이터의 길이만큼 byte 배열을 생성한다.
            data = new byte[BitConverter.ToInt32(data, 0)];
            // 데이터를 수신한다.
            client.Receive(data, data.Length, SocketFlags.None);
            // 수신된 데이터를 UTF8인코딩으로 string 타입으로 변환 후에 콘솔에 출력한다.
            Console.WriteLine(Encoding.UTF8.GetString(data));
        }
    }
}