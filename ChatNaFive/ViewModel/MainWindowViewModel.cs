using ChatNaFive.ViewModel.Base;
using System.Collections.ObjectModel;
using ChatNaFive.Model;

namespace ChatNaFive.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _title = "ChatHaHa";
        private string _userName;

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

        private ObservableCollection<MessageInfo> Messages { get; set; }
        public ObservableCollection<MessageInfo> Message
        {
            get => Messages;
            set
            {
                Messages.Add(ClientModel.InputMessage);
            }
        }

        public MainWindowViewModel()
        {
            Message = new ObservableCollection<MessageInfo>();
        }
    }
}
