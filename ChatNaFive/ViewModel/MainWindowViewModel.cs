using ChatNaFive.ViewModel.Base;
using System.Collections.ObjectModel;
using ChatNaFive.Model;
using System;
using System.Windows.Input;
using ChatNaFive.Infrastructure.Commands;
using ChatNaFive.Services;
using System.Collections.Generic;

namespace ChatNaFive.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Поля

        private string _title = "ChatHaHa";
        private string _userName;
        private string _message;
        private string _exception;
        private bool _connect;
        private readonly IContext _context;
        public ConnectionService _clientModel;

        #endregion

        #region Свойства
        public string Title
        {
            get => _title;
            set => Set(ref _title, value); 
        }

        public string UserName
        {
            get => _userName;
            set => Set(ref _userName, value);
        }

        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        public string Exception
        {
            get => _exception;
            set => Set(ref _exception, value);
        }

        public bool Connection
        {
            get => _connect;
            set 
            {
                if (value == true)
                    this.Exception = string.Empty;
                Set(ref _connect, value);
            } 
        }

        public ObservableCollection<BaseUser> AllUsers { get; }
        public ObservableCollection<BaseMessage> Messages { get; }

        #endregion

        #region Методы

        /// <summary>
        /// Запись всех пользователей в список
        /// </summary>
        /// <param name="users"></param>
        private void SetUsers(List<BaseUser> users)
        {
            _context.Invoke(() => AllUsers.Clear());

            foreach(var user in users)
                _context.Invoke(() => AllUsers.Add(user));
        }

        /// <summary>
        /// Новый прием сообщений
        /// </summary>
        /// <param name="jsonMessage"></param>
        public void SetReceiveJsonMessage(JsonMessage jsonMessage)
        {
            switch (jsonMessage.Method)
            {
                case "GETUSERS":
                    var users = jsonMessage.Users;
                    SetUsers(users);
                    break;

                case "GETMESSAGES":
                    try
                    {
                        var message = (BaseMessage)jsonMessage.Message;

                        if (message.ThisUser)
                        {
                            message.UserName = "Вы";
                            _context.Invoke(() => Messages.Add(message));
                        }
                        else
                            _context.Invoke(() => Messages.Add(message));
                    }
                    catch(Exception ex)
                    {
                        this.Exception = ex.Message;
                    }
                    break;

                default:
                    this.Exception = "Поступила несуществующая команда";
                    break;
            }
        }

        #endregion

        #region Команда для кнопки подключения

        private bool CanEnterInChatCommandExecute(object p) => true;
        private void OnEnterInChatCommandExecuted(object p)
        {
            _clientModel = new ConnectionService(this)
            {
                UserName = UserName
            };
            _clientModel.ConnectAsync();
        }

        public ICommand EnterInChatCommand { get; }

        #endregion

        #region Команда для кнопки отправки сoобщения

        public ICommand SendMessageCommand { get; }
        private bool CanSendMessageCommandExecute(object p) => true;

        private void OnSendMessageCommandExecuted(object p)
        {
            if (this.Message == null || string.IsNullOrWhiteSpace(this.Message))
                return;

            var message = new BaseMessage { UserName = this.UserName, Message = this.Message, Date = DateTime.Now.ToShortTimeString() };
            var jsonMessage = new JsonMessage { Method = "GETMESSAGES", Message = message };
            _clientModel.SendJsonMessageAsync(jsonMessage);
            Message = string.Empty;
        }


        #endregion

        #region Команда для закрытия приложения

        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object parameter) 
        {
            _clientModel.Disconnect();
            Environment.Exit(0);
        } 

        public ICommand CloseApplicationCommand { get; }

        #endregion

        public MainWindowViewModel()
        {
            #region Команды

            EnterInChatCommand = new ActionCommand(OnEnterInChatCommandExecuted, CanEnterInChatCommandExecute);
            SendMessageCommand = new ActionCommand(OnSendMessageCommandExecuted, CanSendMessageCommandExecute);
            CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            
            #endregion

            _context = new WpfDipatcherContext();
            Messages = new ObservableCollection<BaseMessage>();
            AllUsers = new ObservableCollection<BaseUser>();

            Connection = false;
        }
    }
}
