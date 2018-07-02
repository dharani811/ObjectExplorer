using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ObjectExplorer
{
    internal interface IComExpose
    {
        ICommand NewCommand { get; }
    }
}
