using ChatNaFive.Infrastructure.Commands.Base;
using System.Windows;

namespace ChatNaFive.Infrastructure.Commands
{
    internal class HideAppkicationCommand : BaseCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.MainWindow.WindowState = WindowState.Minimized; 
    }
}
