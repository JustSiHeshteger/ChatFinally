using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatNaFive.Model
{
    internal class ClientModel
    {
        private string _userName;
        private string _outputMessage;
        private string _inputMessage;
        private const string host = "92.101.223.197";
        private const int port = 9002;
        private TcpClient client;
        public NetworkStream stream;
        private BinaryReader _reader;
        private BinaryWriter _writer;

        public string Exception { get; private set; }
        public string UserName 
        { 
            get => _userName; 
            set => _userName = value; 
        }
        public string OtputMessage
        {
            get => _outputMessage;
            set => _outputMessage = value;
        }
        public string InputMessage
        {
            get => _inputMessage;
            set => _inputMessage = value;
        }

        async public void ConnectAsync()
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

                    client.Connect(host, port); //подключение клиента
                    _writer.Write(UserName + "бонжур");

                    // запускаем новый поток для получения данных
                    Thread receiveThread = new Thread(ReceiveMessage);
                    receiveThread.Start(); //старт потока
                }
                catch (Exception ex)
                {
                    this.Exception = ex.Message;
                }
            });
        }

        // отправка сообщений
        public void SendMessage()
        {
            _writer.Write(OtputMessage);
        }

        // получение сообщений
        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {

                    string message = _reader.ReadString();
                    InputMessage = $"{UserName}  {message}";//вывод сообщения
                }
                catch (Exception ex)
                {
                    this.Exception = ex.Message;
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
