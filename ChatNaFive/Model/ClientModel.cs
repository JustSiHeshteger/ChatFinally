using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatNaFive.ViewModel;

namespace ChatNaFive.Model
{
    internal class ClientModel
    {
        private const string host = "92.101.223.197";
        private const int port = 9002;

        private string _userName;
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        private MainWindowViewModel _mvvm;

        public ClientModel(MainWindowViewModel mvvm)
        {
            if (mvvm != null)
                this.MVVM = mvvm;
        }
        public string UserName 
        { 
            get => _userName; 
            set => _userName = value; 
        }
        public  MainWindowViewModel MVVM
        {
            get => _mvvm;
            set => _mvvm = value;
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

                    _writer.Write(UserName);

                    // запускаем новый поток для получения данных
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
        public void SendMessage(string OtputMessage)
        {
            try
            {
                if (_writer != null)
                {
                    _writer.Write(OtputMessage);
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
                    string message = _reader.ReadString();
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
