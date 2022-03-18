using ChatNaFive.Infrastructure.Commands;
using ChatNaFive.ViewModel.Base;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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

        public MainWindowViewModel()
        {
            
        }
    }
}
