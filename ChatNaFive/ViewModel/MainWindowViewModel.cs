using ChatNaFive.ViewModel.Base;
using System.Collections.ObjectModel;
using ChatNaFive.Model;
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

        public ObservableCollection<BaseMessage> Messages { get; }
        public ObservableCollection<BaseMessage> OurMessages { get; }

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
                _context.Invoke(() => OurMessages.Add(message));
                _context.Invoke(() => Messages.Add(null));
            }
            else
            {
                _context.Invoke(() => Messages.Add(message));
                _context.Invoke(() => OurMessages.Add(null));
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
            
            Task.Run(() =>
            {
                _clientModel.UserName = UserName;
                _clientModel.ConnectAsync();
            });
        }

        #endregion

        #region Команда для кнопки отправки сoобщения

        public ICommand SendMessageCommand { get; }

        private void OnSendMessageCommandExecuted(object p)
        {
            Task.Run(() =>
            {
                var message = new BaseMessage { UserName = this.UserName, Message = this.Message, Date = System.DateTime.Now };
                _clientModel.SendMessage(message);
            });
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
            OurMessages = new ObservableCollection<BaseMessage>();

            SetReceiveMessage(new BaseMessage { UserName = "asd", Date = System.DateTime.Now, Message = "привет", ThisUser = true });
            SetReceiveMessage(new BaseMessage { UserName = ";aosiv", Date = System.DateTime.Now, Message = "привет", ThisUser = false });
        }
    }
}
