using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace TDDD49.Commands
{
    class OpenDialogCommand : ICommand
    {

        private Type type;

        public OpenDialogCommand(Type type)
        {
            this.type = type;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return type != null;

        }

        public void Execute(object parameter)
        {
            Window w = (Window) Activator.CreateInstance(type);
            w.Owner = Application.Current.MainWindow;

            w.ShowDialog();
        }
    }
}
