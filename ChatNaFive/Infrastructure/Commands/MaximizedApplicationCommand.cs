using ChatNaFive.Infrastructure.Commands.Base;
using System.Windows;

namespace ChatNaFive.Infrastructure.Commands
{
    internal class MaximizedApplicationCommand : BaseCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            foreach (Window window in App.Current.Windows)
            {
                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }
    }
}
