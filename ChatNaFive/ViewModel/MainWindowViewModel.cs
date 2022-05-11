using ChatNaFive.ViewModel.Base;
using System.Collections.ObjectModel;
using ChatNaFive.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using ChatNaFive.Infrastructure.Commands;
using System.Linq;
using ChatNaFive.Services;

namespace ChatNaFive.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Поля

        private string _title = "ChatHaHa";
        private string _userName;
        private string _message;
        private string _exception;
        private readonly ConnectionService _clientModel;
        private readonly IContext _context;

        #endregion

        #region Свойства
        public string Title
        {
            get => _title;
            set => Set(ref _title, value); //Это то, что написали
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

        public ObservableCollection<BaseUser> AllUsers { get; }
        public ObservableCollection<BaseMessage> Messages { get; }

        #endregion

        #region Методы

        /// <summary>
        /// Старый прием сообщений и установка
        /// </summary>
        /// <param name="message"></param>
        public void SetReceiveMessage(BaseMessage message)
        {
            if (message.ThisUser)
            {
                message.UserName = "Вы";
                _context.Invoke(() => Messages.Add(message));
            }
            else
            {
                _context.Invoke(() => Messages.Add(message));
            }
        }

        private void SetUsers(List<BaseUser> users)
        {
            foreach (var user in users)
            {
                try
                {
                    switch (user.Action)
                    {
                        case "DELETE":
                            if (AllUsers.Contains(user))
                                _context.Invoke(() => AllUsers.Remove(user));
                            break;

                        case "ADD":
                            if (!AllUsers.Contains(user))
                                _context.Invoke(() => AllUsers.Add(user));
                            break;

                        case null:
                            if (!AllUsers.Contains(user))
                                _context.Invoke(() => AllUsers.Add(user));
                            break;

                        case "":
                            if (!AllUsers.Contains(user))
                                _context.Invoke(() => AllUsers.Add(user));
                            break;

                        default:
                            Exception = "Получена неизвестная команда для операции с пользователем";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Exception = ex.Message;
                }
            }
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
                    var users = (List<BaseUser>)jsonMessage.Message;
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

        public void SetException(string ex)
        {
            Exception = ex;
        }

        #endregion

        #region Команда для кнопки подключения

        private bool CanEnterInChatCommandExecute(object p) => true;
        private void OnEnterInChatCommandExecuted(object p)
        {
            _clientModel.UserName = UserName;
            _clientModel.ConnectAsync();
        }

        public ICommand EnterInChatCommand { get; }

        #endregion

        #region Команда для кнопки отправки сoобщения

        public ICommand SendMessageCommand { get; }

        private void OnSendMessageCommandExecuted(object p)
        {
            var message = new BaseMessage { UserName = this.UserName, Message = this.Message, Date = DateTime.Now.ToShortTimeString() };
            var jsonMessage = new JsonMessage { Method = "GETMESSAGES", Message = message };
            _clientModel.SendJsonMessageAsync(jsonMessage);
            Message = string.Empty;
        }

        private bool CanSendMessageCommandExecute(object p) => true;

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
            _clientModel = new ConnectionService(this);
            Messages = new ObservableCollection<BaseMessage>();
            AllUsers = new ObservableCollection<BaseUser>();
        }
    }
}
