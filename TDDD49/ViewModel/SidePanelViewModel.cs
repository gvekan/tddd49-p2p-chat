using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Views;
using System.Windows;
using System.Windows.Controls;

namespace TDDD49.ViewModel
{
    class SidePanelViewModel : ViewModelBase
    {
        #region Private Fields
        // List of connections
        #endregion

        #region Public Properties
        #endregion

        #region Commands

        public ICommand OpenConnectDialogCommand
        {
            get
            {
                return new RelayCommand(OpenConnectDialog);
            }
        }

        #endregion

        #region Methods

        internal void OpenConnectDialog()
        {
            ConnectDialog cd = new ConnectDialog();
            cd.Owner = Application.Current.MainWindow;

            cd.ShowDialog();
        }

        #endregion
    }
}
