using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class JoinSessionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int UserID;
        public string Username;

        public JoinSessionViewModel()
        {
            
        }
    }
}
