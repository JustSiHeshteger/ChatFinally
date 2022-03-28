using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace Server
{
    class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        ServerObject server; // объект сервера
        private BinaryWriter _writer;
        private BinaryReader _reader;

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();// получаем имя пользователя
                _writer = new BinaryWriter(Stream, Encoding.Unicode, false);
                _reader = new BinaryReader(Stream, Encoding.Unicode, false);

                string message = _reader.ReadString();
                userName = message;

                message = userName + " вошел в чат"; // посылаем сообщение о входе в чат всем подключенным пользователям
                
                server.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);

                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = _reader.ReadString();
                        message = String.Format("{0}: {1}", userName, message);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                    }
                    catch
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }

        internal void SendMessage(string message)
        {
            this._writer.Write(message);
        }
    }
}
