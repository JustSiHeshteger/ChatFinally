using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace Server
{
    public class ClientObject
    {
        private readonly Random rnd = new Random();
        private readonly List<string> _inputUser = new List<string>()
        {
            "вошел в чат",
            "влетел с ноги",
            "прилетел на штанах-парашютах",
            "вылез из канавы",
            "установил соединение",
            "убежал от Никитиной"
        };
        private readonly List<string> _outputUser = new List<string>()
        {
            "убился",
            "умер насмерть",
            "прыгнул в канаву",
            "отключился",
            "стал dead inside",
            "прекратил общение"
        };
        string userName;
        readonly TcpClient client;
        readonly ServerObject server; // объект сервера
        BinaryWriter _writer;

        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
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
                Stream = client.GetStream();

                _writer = new BinaryWriter(Stream, Encoding.Unicode, false);
                var _reader = new BinaryReader(Stream, Encoding.Unicode, false);

                // получаем имя пользователя
                string message = _reader.ReadString();
                userName = message;
                message = $"{userName} {_inputUser[rnd.Next(0, _inputUser.Count)]}";

                // посылаем сообщение о входе в чат всем подключенным пользователям
                server.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = _reader.ReadString();
                        message = String.Format($"{userName}: {message}");
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                    }
                    catch
                    {
                        message = String.Format($"{userName}: {_outputUser[rnd.Next(0, _outputUser.Count)]}");
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
        internal void SendMessage(string message)
        {
            this._writer.Write(message);
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
