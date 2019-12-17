using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49.Models;

namespace TDDD49.Helpers
{
    class ConnectionObservableCollection : ObservableCollection<ConnectionModel>
    {
        public new void Add(ConnectionModel cm)
        {
            base.Add(cm);
            cm.PropertyChanged += Connection_PropertyChanged;
        }

        private void Connection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
    }
}
