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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Whatsapp.ViewModels.ViewModelsPage;

namespace Whatsapp.Views.ViewPages
{
    /// <summary>
    /// Interaction logic for ViewEntry.xaml
    /// </summary>
    public partial class ViewEntry : Page
    {
        public ViewEntry()
        {
            InitializeComponent();
            DataContext = new ViewModelEntry();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
