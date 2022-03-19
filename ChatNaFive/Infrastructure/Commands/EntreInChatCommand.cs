using ChatNaFive.Infrastructure.Commands.Base;
using System.Windows;

namespace ChatNaFive.Infrastructure.Commands
{
    internal class EntreInChatCommand : BaseCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            
            foreach(Window currentWindow in App.Current.Windows)
            {
                if (currentWindow is UserLoginForm)
                {
                    currentWindow.Close();
                }
            }
            
        }
    }
}
