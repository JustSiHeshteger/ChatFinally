using System;
using System.Collections.Generic;
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
        private NetworkStream stream;

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

        public void ConnectAsync()
        {
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток
                
                byte[] data = Encoding.Unicode.GetBytes(UserName);
                stream.Write(data, 0, data.Length);

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
                OtputMessage = $"Ура ура {UserName} в чате";
                Task.Run(() => 
                {
                    SendMessage();
                }); 
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }
        // отправка сообщений
        public void SendMessage()
        {
            while (true)
            {
                byte[] data = Encoding.Unicode.GetBytes(OtputMessage);
                stream.Write(data, 0, data.Length);
            }
        }
        // получение сообщений
        void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    InputMessage = $"{UserName}  {message}";//вывод сообщения
                }
                catch
                {
                    //Console.WriteLine("Подключение прервано!"); //соединение было прервано
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
