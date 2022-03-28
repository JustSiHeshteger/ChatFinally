using System;
using System.Windows.Threading;

namespace ChatNaFive.Services
{
    public sealed class WpfDipatcherContext : IContext
    {
        private readonly Dispatcher _dispatcher;

        public WpfDipatcherContext() : this(Dispatcher.CurrentDispatcher) { }

        public WpfDipatcherContext(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        public void Invoke(Action action)
        {
            this._dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            this._dispatcher.BeginInvoke(action);
        }
    }
}
