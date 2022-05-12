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
        //private const string host = "3.73.109.65";
        private const string host = "127.0.0.1";
        private const int port = 9002;

        private string _userName;
        private TcpClient _client;
        private NetworkStream _stream;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        private readonly MainWindowViewModel MVVM;

        private Thread receiveThread;
        private Thread usersThread;

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
                _client = new TcpClient();
                try
                {
                    _client.Connect(host, port);
                    _stream = _client.GetStream(); // получаем поток
                    _reader = new BinaryReader(_stream, Encoding.Unicode, true);
                    _writer = new BinaryWriter(_stream, Encoding.Unicode, true);

                    var message = new BaseMessage() { UserName = UserName, Message = "", Date = DateTime.Now.ToShortTimeString() };
                    string jsonMessage = JsonSerializer.Serialize(new JsonMessage() { Method = "GETMESSAGES", Message = message });

                    _writer.Write(jsonMessage);

                    // запускаем новый поток для получения данных
                    receiveThread = new(ReceiveJsonMessage);
                    usersThread = new(GetUsers);
                    usersThread.Start();
                    receiveThread.Start();

                    MVVM.Connection = true;
                }
                catch (Exception ex)
                {
                    MVVM.Exception = ex.Message;
                    MVVM.Connection = false;
                }
            });
        }

        // отправка сообщений
        public async void SendMessageAsync(BaseMessage OtputMessage)
        {
            try
            {
                if (_writer != null)
                {
                    await Task.Run(() =>
                    {
                        string message = JsonSerializer.Serialize(OtputMessage);
                        _writer.Write(message);
                    });
                }
            }
            catch (Exception ex)
            {
                MVVM.Exception = ex.Message;
            }
        }

        public async void SendJsonMessageAsync(JsonMessage OtputMessage)
        {
            if (_writer != null)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        string message = JsonSerializer.Serialize(OtputMessage);
                        _writer.Write(message);
                    }
                    catch(Exception ex)
                    {
                        MVVM.Exception = ex.Message;
                        Disconnect();
                    }
                });
            }
        }

        private void ReceiveJsonMessage()
        {
            while (true)
            {
                try
                {
                    var message = JsonSerializer.Deserialize<JsonMessage>(_reader.ReadString());
                    MVVM.SetReceiveJsonMessage(message);
                }
                catch (Exception ex)
                {
                    MVVM.Exception = ex.Message;
                    Disconnect();
                    break;
                }
            }
        }

        private void GetUsers()
        {
            while (true)
            {
                try
                {
                    _writer.Write(JsonSerializer.Serialize(new JsonMessage() { Method = "GETUSERS" }));
                    Thread.Sleep(10000);
                }
                catch(Exception ex)
                {
                    MVVM.Exception = ex.Message;
                    Disconnect();
                    break;
                }
            }
        }

        public void Disconnect()
        {
            if (_stream != null)
                _stream.Close();
            if (_client != null)
                _client.Close();
            if (_reader != null)
                _reader.Close();
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
            }

            MVVM.Connection = false;
        }
    }
}
