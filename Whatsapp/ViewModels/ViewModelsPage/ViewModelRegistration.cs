using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Whatsapp.Commands;
using Whatsapp.Views.ViewPages;

namespace Whatsapp.ViewModels.ViewModelsPage
{
    public class ViewModelRegistration
    {
        public ICommand? SendCodeCommand { get; set; }
        public ViewModelRegistration()
        {
            SendCodeCommand = new Command(ExecuteSendCodeCommand, CanExecuteSendCodeCommand);
        }

        private bool CanExecuteSendCodeCommand(object obj) =>
            Regex.IsMatch(((PasswordBox)((Page)obj)
                .FindName("gmail")).Password, @"^[a-zA-Z0-9._%+-]+@gmail\.com$");

        private void ExecuteSendCodeCommand(object obj)
        {
            var gmail = ((PasswordBox)((Page)obj).FindName("gmail")).Password;
            var page = new ViewConfrimationCode();
            page.DataContext = new ViewModelConfrimationCode(gmail);
            ((Page)obj).NavigationService.Navigate(page);
        }
    }
}
