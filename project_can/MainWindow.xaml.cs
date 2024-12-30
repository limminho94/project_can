using System;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading;
using System.Windows.Media.Imaging;

namespace project_can
{
    public partial class MainWindow : System.Windows.Window
    {
        //private VideoCapture capCamera;
        //private Mat matImage = new Mat();
        //private ClientHandler clientHandler;

        public MainWindow()
        {
            InitializeComponent();
            //InitializeCamera();
            //clientHandler = new ClientHandler();
            //clientHandler.ConnectToServer();
        }

    //    private void InitializeCamera()
    //    {
    //        capCamera = new VideoCapture(0);
    //    }

    //    private void btPlay_Click(object sender, RoutedEventArgs e)
    //    {
    //        new Thread(PlayCamera).Start();
    //    }

    //    private void PlayCamera()
    //    {
    //        while (!capCamera.IsDisposed)
    //        {
    //            capCamera.Read(matImage);
    //            if (matImage.Empty())
    //            {
    //                break;
    //            }
    //            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
    //            {
    //                var converted = Convert(BitmapConverter.ToBitmap(matImage));
    //                imgViewport.Source = converted;

    //                // 프레임 전송
    //                byte[] frameData = matImage.ToBytes();
    //                clientHandler.SendFrameToServer(frameData);
    //            }));
    //        }
    //    }

    //    private void btStop_Click(object sender, RoutedEventArgs e)
    //    {
    //        if (capCamera.IsOpened())
    //        {
    //            capCamera.Dispose();
    //        }

    //        this.Close();
    //    }

    //    public BitmapImage Convert(Bitmap src)
    //    {
    //        MemoryStream ms = new MemoryStream();
    //        ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
    //        BitmapImage image = new BitmapImage();
    //        image.BeginInit();
    //        ms.Seek(0, SeekOrigin.Begin);
    //        image.StreamSource = ms;
    //        image.EndInit();
    //        return image;
    //    }
    //}

    //public class ClientHandler
    //{
    //    private Socket client;

    //    public void ConnectToServer()
    //    {
    //        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        client.Connect(new IPEndPoint(IPAddress.Parse("10.10.20.116"), 12345));
    //    }

        //public void SendMessageToServer(string message)
        //{
        //    var data = Encoding.UTF8.GetBytes(message);
        //    byte[] header = Encoding.UTF8.GetBytes("m");
        //    byte[] lengthBytes = BitConverter.GetBytes((ushort)data.Length);
        //    if (BitConverter.IsLittleEndian)
        //        Array.Reverse(lengthBytes);
        //    client.Send(header);
        //    client.Send(lengthBytes);
        //    client.Send(data);
        //}

        //public void SendFrameToServer(byte[] frameData)
        //{
        //    byte[] buffer = new byte[2048];

        //    try
        //    {
        //        if (client.Connected)
        //        {
        //            byte[] header = Encoding.UTF8.GetBytes("img");
        //            byte[] lengthBytes = BitConverter.GetBytes((ushort)frameData.Length);
        //            if (BitConverter.IsLittleEndian)
        //                Array.Reverse(lengthBytes);
        //            string buff = header + "!" + lengthBytes + "!" + frameData;
        //            buffer = Encoding.UTF8.GetBytes(buff);
        //            client.Send(buffer);
        //            Console.WriteLine(buffer);
        //        }
        //        else
        //        {
        //            Console.WriteLine("연결이 종료되었습니다. 재연결 시도 중...");
        //            //ConnectToServer(); // 재연결 시도
        //        }
        //    }
        //    catch (SocketException ex)
        //    {
        //        Console.WriteLine($"소켓 오류: {ex.Message}");
        //        //ConnectToServer(); // 연결 재시도
        //    }
        //}

        //public string ReceiveMessageFromServer()
        //{
        //    byte[] header = new byte[3];
        //    client.Receive(header, 3, SocketFlags.None);
        //    string dataType = Encoding.UTF8.GetString(header[0..1]);
        //    ushort length = BitConverter.ToUInt16(header[1..3], 0);

        //    if (BitConverter.IsLittleEndian)
        //    {
        //        byte[] lengthBytes = header[1..3];
        //        Array.Reverse(lengthBytes);
        //        length = BitConverter.ToUInt16(lengthBytes, 0);
        //    }

        //    byte[] data = new byte[length];
        //    client.Receive(data, length, SocketFlags.None);

        //    if (dataType == "msg")
        //    {
        //        MessageBox.Show("메세지 받기 성공");
        //        return Encoding.UTF8.GetString(data);
        //    }

        //    return string.Empty;
        //}
    }
}
