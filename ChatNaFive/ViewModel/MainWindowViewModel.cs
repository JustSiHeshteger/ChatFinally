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
        private readonly ClientModel _clientModel;
        private readonly IContext _context;

        public ObservableCollection<string> Messages { get; }

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
        public void SetReceiveMessage(string message)
        {
            _context.Invoke(() => Messages.Add(message));
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
            _clientModel.UserName = this.UserName;
            _clientModel.ConnectAsync();
        }

        #endregion

        #region Команда для кнопки отправки сoобщения

        public ICommand SendMessageCommand { get; }

        private void OnSendMessageCommandExecuted(object p)
        {
            _clientModel.SendMessage(Message);
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
            _clientModel = new ClientModel(this);
            Messages = new ObservableCollection<string>();
        }
    }
}
