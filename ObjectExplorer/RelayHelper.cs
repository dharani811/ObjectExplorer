using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ObjectExplorer
{
    internal class RelayHelper : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<object> execute;

        public RelayHelper(Action<object> execute)
        {
            this.execute = execute;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
