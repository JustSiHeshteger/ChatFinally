using ServerCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ServerCore
{
    public class ServerObject
    {
        static TcpListener _tcpListener; // сервер для прослушивания
        readonly List<ClientObject> _clients = new List<ClientObject>(); // все подключения

        protected internal void AddConnection(ClientObject clientObject)
        {
            _clients.Add(clientObject);
            BroadcastUsers();
        }
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject client = _clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                _clients.Remove(client);
                BroadcastUsers();
            }
        }
        // прослушивание входящих подключений
        protected internal void Listen()
        {
            try
            {
                _tcpListener = new TcpListener(IPAddress.Any, 9002);
                _tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        /// <summary>
        /// Отправка сообщений
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        protected internal void BroadcastMessage(BaseMessage message, string id)
        {
            foreach (var client in _clients)
            {
                if (id == client.Id)
                {
                    message.ThisUser = true;
                }
                else
                {
                    message.ThisUser = false;
                }
                var jsonMessage = new JsonMessage() { Method = "GETMESSAGES", Message = message };

                client.SendJsonMessage(jsonMessage);
            }
        }

        /// <summary>
        /// Отправка всех подключенных пользователей
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        protected internal void BroadcastUsers()
        {
            var users = new List<BaseUser>();
            foreach (var client in _clients)
            {
                var user = new BaseUser
                {
                    Id = client.Id,
                    UserName = client.UserName,
                };
                users.Add(user);
            }

            var jsonMessage = new JsonMessage() { Method = "GETUSERS", Users = users };

            foreach (var client in _clients)
                client.SendJsonMessage(jsonMessage);
        }

        // отключение всех клиентов
        protected internal void Disconnect()
        {
            _tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < _clients.Count; i++)
            {
                _clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }
}
