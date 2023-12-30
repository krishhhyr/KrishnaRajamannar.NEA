using KrishnaRajamannar.NEA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for ViewSessionInformation.xaml
    /// </summary>
    public partial class ViewSessionInformation : Window
    {
        private readonly ViewSessionInfoViewModel _viewSessionInfoViewModel;
        public ViewSessionInformation(ViewSessionInfoViewModel viewSessionInfoViewModel)
        {           
            InitializeComponent();
            _viewSessionInfoViewModel = viewSessionInfoViewModel;
            DataContext = _viewSessionInfoViewModel;
        }
    }
}
