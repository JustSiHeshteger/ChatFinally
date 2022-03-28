using ChatNaFive.ViewModel.Base;
using System.Collections.ObjectModel;
using ChatNaFive.Model;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using ChatNaFive.Infrastructure.Commands;

namespace ChatNaFive.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _title = "ChatHaHa";
        private string _message;
        private string _userName;
        private string _inputMessage;
        private ClientModel model;

        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

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

        public string InputMessage
        {
            get => _inputMessage;
            set => Set(ref _inputMessage, model.InputMessage);
        }

        #region Кнопка подключения
        public ICommand EnterInChatCommand { get; }

        private void OnEnterInChatCommandExecuted(object p)
        {
            
            
            {
                model.UserName = UserName;
                model.ConnectAsync();
            };
        }

        #endregion

        #region Кнопка отправки ссобщения

        public ICommand SendMessageCommand { get; }

        private void OnSendMessageCommandExecuted(object p)
        {
            
            {
                model.OtputMessage = Message;
                model.SendMessage();
            };
        }

        private bool CanSendMessageCommandExecute(object p) => true;

        #endregion

        private bool CanEnterInChatCommandExecute(object p) => true;

        public MainWindowViewModel()
        {
            EnterInChatCommand = new ActionCommand(OnEnterInChatCommandExecuted, CanEnterInChatCommandExecute);
            SendMessageCommand = new ActionCommand(OnSendMessageCommandExecuted, CanSendMessageCommandExecute);
            model = new ClientModel();
        }
    }
}
