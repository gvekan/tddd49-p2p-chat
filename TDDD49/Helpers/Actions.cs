using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TDDD49.Helpers
{
    class Actions
    {

        public static void HandleBugException(Exception e, String message = "Oops! Something went wrong.")
        {
            Console.WriteLine(e.ToString());
            MessageBox.Show(message);
        }
        public static void OpenDialog(Type t, NotifyPropertyChangedBase vm)
        {
            try
            {
                Window w = (Window)Activator.CreateInstance(t);
                w.Owner = Application.Current.MainWindow;
                w.DataContext = vm;

                w.ShowDialog();
            }
            catch (InvalidCastException e)
            {
                HandleBugException(e);
            }
        }

        public static void CloseDialog(object parameter)
        {
            try
            {
                Window w = (Window) parameter;
                w.Close();
            }
            catch (InvalidCastException e)
            {
                HandleBugException(e);
            }
        }
    }
}
