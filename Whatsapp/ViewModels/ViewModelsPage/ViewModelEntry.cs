using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Whatsapp.Commands;
using Whatsapp.Views.ViewPages;

namespace Whatsapp.ViewModels.ViewModelsPage
{
    public class ViewModelEntry
    {
        public ICommand ?LogInCommand { get; set; }
        public ViewModelEntry()
        {
            LogInCommand = new Command(ExecuteLogInCommand);
        }

        private void ExecuteLogInCommand(object obj)
        {
            
            var page = new SuccessfulLogin();
            page.DataContext = new ViewModelSuccsessEntryed(((PasswordBox)((Page)obj).FindName("GmailTextBox")).Password);
           ((Page)obj).NavigationService.Navigate(page);
        }
    }
}
