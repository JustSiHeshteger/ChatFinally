using ChatNaFive.Infrastructure.Commands;
using ChatNaFive.Model;
using ChatNaFive.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatNaFive.ViewModel
{
    internal class LoginUserViewModel : ViewModelBase
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

        public ICommand EnterInChatCommand { get; }

        private void OnEnterInChatCommandExecuted(object p)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            foreach (Window currentWindow in App.Current.Windows)
            {
                if (currentWindow is UserLoginForm)
                {
                    //currentWindow.Close();
                }
            }

        }
        private bool CanEnterInChatCommandExecute(object p) => true;

        public LoginUserViewModel()
        {
            EnterInChatCommand = new ActionCommand(OnEnterInChatCommandExecuted, CanEnterInChatCommandExecute);
        }
    }
}
