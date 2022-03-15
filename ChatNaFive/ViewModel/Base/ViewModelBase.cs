using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatNaFive.ViewModel.Base
{
    internal abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null) //Посмотри в классе MainWindowViewModel
        {
            if (Equals(field, value))
            {
                return false;
            }
            else
            {
                field = value;
                OnPropertyChanged(PropertyName);
                return true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

        private bool _isDisposed;
        protected virtual void Dispose(bool Disposing)
        {
            if (!Disposing || _isDisposed)
            {
                return;
            }
            else
            {
                _isDisposed = true;
            }
        }
    }
}
