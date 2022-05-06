using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatNaFive.ViewModel;
using System.Text.Json;

namespace ChatNaFive.Model
{
    internal class ConnectionService
    {
        private const string host = "3.73.109.65";
        private const int port = 9002;

        private string _userName;
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        private readonly MainWindowViewModel MVVM;

        public ConnectionService(MainWindowViewModel mvvm)
        {
            this.MVVM = mvvm;
        }

        public string UserName 
        { 
            get => _userName; 
            set => _userName = value; 
        }

        public async void ConnectAsync()
        {
            await Task.Run(() => 
            {
                client = new TcpClient();
                try
                {
                    client.Connect(host, port);
                    stream = client.GetStream(); // получаем поток
                    _reader = new BinaryReader(stream, Encoding.Unicode, true);
                    _writer = new BinaryWriter(stream, Encoding.Unicode, true);

                    string message = JsonSerializer.Serialize(new BaseMessage { UserName = UserName, Message = "", Date = DateTime.Now.ToShortTimeString() });

                    _writer.Write(message);

                    Thread receiveThread = new(ReceiveMessage);
                    receiveThread.Start(); //старт потока
                }
                catch (Exception ex)
                {
                    MVVM.SetException(ex.Message);
                }
            });
        }

        // отправка сообщений
        public void SendMessage(BaseMessage OtputMessage)
        {
            try
            {
                if (_writer != null)
                {
                    string message = JsonSerializer.Serialize(OtputMessage);
                    _writer.Write(message);
                }
            }
            catch (Exception ex)
            {
                MVVM.SetException(ex.Message);
            }
        }

        // получение сообщений
        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    var message = JsonSerializer.Deserialize<BaseMessage>(_reader.ReadString());
                    MVVM.SetReceiveMessage(message);
                }
                catch (Exception ex)
                {
                    MVVM.SetException(ex.Message);
                    Disconnect();
                }
            }
        }

        void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
        }
    }
}
