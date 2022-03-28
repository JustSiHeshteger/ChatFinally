using ChatNaFive.Infrastructure.Commands.Base;
using System;
using System.Windows;

namespace ChatNaFive.Infrastructure.Commands
{
    internal class CloseApplicationCommand : BaseCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Environment.Exit(0);
    }
}
