using ChatNaFive.ViewModel.Base;
using System.Collections.ObjectModel;
using ChatNaFive.Model;
using System;
using System.Windows.Input;
using ChatNaFive.Infrastructure.Commands;
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

        public ObservableCollection<BaseMessage> Messages { get; }

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

        #endregion

        #region Методы
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

        public void SetException(string ex)
        {
            Exception = ex;
        }

        #endregion

        #region Команда для кнопки подключения

        private bool CanEnterInChatCommandExecute(object p) => true;
        public ICommand EnterInChatCommand { get; }

        private void OnEnterInChatCommandExecuted(object p)
        {
            _clientModel.UserName = UserName;
            _clientModel.ConnectAsync();
        }

        #endregion

        #region Команда для кнопки отправки сoобщения

        public ICommand SendMessageCommand { get; }

        private void OnSendMessageCommandExecuted(object p)
        {
            var message = new BaseMessage { UserName = this.UserName, Message = this.Message, Date = DateTime.Now.ToShortTimeString() };
            _clientModel.SendMessage(message);
            Message = string.Empty;
            
        }

        private bool CanSendMessageCommandExecute(object p) => true;

        #endregion

        public MainWindowViewModel()
        {
            #region Команды
            EnterInChatCommand = new ActionCommand(OnEnterInChatCommandExecuted, CanEnterInChatCommandExecute);
            SendMessageCommand = new ActionCommand(OnSendMessageCommandExecuted, CanSendMessageCommandExecute);
            #endregion

            _context = new WpfDipatcherContext();
            _clientModel = new ConnectionService(this);
            Messages = new ObservableCollection<BaseMessage>();
        }
    }
}
