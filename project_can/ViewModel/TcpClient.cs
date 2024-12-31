using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace project_can.ViewModel
{
    public class TCPClient
    {
        private TcpClient? _client;
        private NetworkStream? _stream;
        private bool _isConnected = false;

    }
}
