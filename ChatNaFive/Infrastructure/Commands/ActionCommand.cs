using ChatNaFive.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNaFive.Infrastructure.Commands
{
    internal class ActionCommand : BaseCommand
    {

        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public ActionCommand(Action<object> Execute, Func<object, bool> CanExecute)
        {
            _execute = Execute ?? throw new ArgumentException(null, nameof(Execute));
            _canExecute = CanExecute;
        }

        public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => _execute(parameter);
    }
}
