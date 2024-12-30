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

        //public bool Connect(string ip, int port)
        //{
        //    try
        //    {
        //        _client = new TcpClient();
        //        _client.Connect(IPAddress.Parse(ip), port);
        //        _stream = _client.GetStream();
        //        _isConnected = true;

        //        MessageBox.Show("서버 연결 성공");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"서버 연결 실패: {ex.Message}");
        //        return false;
        //    }
        //}

        //public void Disconnect()
        //{
        //    if (_isConnected)
        //    {
        //        _stream?.Close();
        //        _client?.Close();
        //        _isConnected = false;
        //        //MessageBox.Show("서버 연결 종료");
        //    }
        //}

        //public async Task SendDataAsync(byte[] data)
        //{
        //    if (_isConnected && _stream != null)
        //    {
        //        try
        //        {
        //            await _stream.WriteAsync(data, 0, data.Length);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"데이터 전송 오류: {ex.Message}");
        //        }
        //    }
        //}

        //public async Task<string?> ReceiveDataAsync()
        //{
        //    if (_isConnected && _stream != null)
        //    {
        //        try
        //        {
        //            byte[] buffer = new byte[1024];
        //            int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        //            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"데이터 수신 오류: {ex.Message}");
        //        }
        //    }
        //    return null;
        //}
    }
}
