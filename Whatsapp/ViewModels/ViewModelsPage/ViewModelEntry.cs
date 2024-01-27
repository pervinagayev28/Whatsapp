using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Whatsapp.Commands;
using Whatsapp.Models.TestModels;
using Whatsapp.Views.ViewPages;

namespace Whatsapp.ViewModels.ViewModelsPage
{
    public class ViewModelEntry
    {
        private List<string> gmails = new();
        public ICommand? LogInCommand { get; set; }
        public ICommand? RegistrationCommand { get; set; }
        public ViewModelEntry()
        {
            GetUserGmails();
            LogInCommand = new Command(ExecuteLogInCommand, CanExecuteLogInCommand);
            RegistrationCommand = new Command(ExecuteRegistrationCommand);
        }

        private async void GetUserGmails()
        {
            gmails = await new MyChatingAppContext().UsersTbs.Select(x => x.Gmail).ToListAsync();
        }
        private void ExecuteRegistrationCommand(object obj)
        {
            var page = new ViewRegstration();
            page.DataContext = new ViewModelRegistration();
            ((Page)obj).NavigationService.Navigate(page);
        }

        private bool CanExecuteLogInCommand(object obj) =>
                  gmails.Any(g => g == ((PasswordBox)((Page)obj).FindName("GmailTextBox")).Password);

        private void ExecuteLogInCommand(object obj)
        {
            var page = new SuccessfulLogin();
            page.DataContext = new ViewModelSuccsessEntryed(((PasswordBox)((Page)obj).FindName("GmailTextBox")).Password
                            , new MyChatingAppContext());
            ((Page)obj).NavigationService.Navigate(page);
        }
    }
}
