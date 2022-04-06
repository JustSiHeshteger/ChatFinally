using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
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
        readonly TcpClient client;
        readonly ServerObject server; // объект сервера
        BinaryWriter _writer;
        BinaryReader _reader;

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
                _reader = new BinaryReader(Stream, Encoding.Unicode, false);

                var message = JsonConvert.DeserializeObject<BaseMessage>(_reader.ReadString());
                message.Message = _inputUser[rnd.Next(0, _inputUser.Count)];

                server.BroadcastMessage(message, this.Id);
                Console.WriteLine($"{message.UserName} {message.Message}");

                while (true)
                {
                    try
                    {
                        message = JsonConvert.DeserializeObject<BaseMessage>(_reader.ReadString());
                        Console.WriteLine($"{message.UserName} {message.Message}");
                        server.BroadcastMessage(message, this.Id);
                    }
                    catch
                    {
                        message = JsonConvert.DeserializeObject<BaseMessage>(_reader.ReadString());
                        message.Message = _outputUser[rnd.Next(0, _outputUser.Count)];
                        server.BroadcastMessage(message, this.Id);
                        Console.WriteLine($"{message.UserName} {message.Message}");
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
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        internal void SendMessage(BaseMessage message)
        {
            string mes = JsonConvert.SerializeObject(message);
            this._writer.Write(mes);
        }

        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
