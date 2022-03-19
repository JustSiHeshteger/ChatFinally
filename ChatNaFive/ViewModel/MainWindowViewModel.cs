using ChatNaFive.ViewModel.Base;

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

        public MainWindowViewModel()
        {
            
        }
    }
}
