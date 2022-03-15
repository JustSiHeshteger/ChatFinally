using ChatNaFive.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNaFive.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _title;

        public string Title
        {
            get => _title;

            set => Set(ref _title, value); //Это то, что написали
        }

        private string _name;

        public string Name
        {
            get => _name;

            set => Set(ref _title, value); //Это то, что написали
        }

        private IEnumerable<string> _messages;

        public IEnumerable<string> Messages
        {
            get => _messages;
            set
            {
            }
        }
    }
}
