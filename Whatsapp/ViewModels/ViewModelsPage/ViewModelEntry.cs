using ChatAppDatabaseLibrary.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Whatsapp.Commands;
using Whatsapp.Views.ViewPages;

namespace Whatsapp.ViewModels.ViewModelsPage
{
    public class ViewModelEntry
    {
        private List<string> gmails = new();
        public ICommand? LogInCommand { get; set; }
        public ICommand? RegistrationCommand { get; set; }
        public ICommand? CloseCommand { get; set; }
        public ViewModelEntry()
        {
            GetUserGmails();
            LogInCommand = new Command(ExecuteLogInCommand, CanExecuteLogInCommand);
            RegistrationCommand = new Command(ExecuteRegistrationCommand);
            CloseCommand = new Command(ExecuteCloseCommand);
        }

        private void ExecuteCloseCommand(object obj)
        {
            if (obj is Page child)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(child);

                while (parent != null && !(parent is NavigationWindow))
                    parent = VisualTreeHelper.GetParent(parent);
                if (parent != null)
                    (parent as NavigationWindow)!.Close();
            }
        }

        private async void GetUserGmails()
        {
            gmails = await new ChatAppDb().UsersTbs.Select(x => x.Gmail).ToListAsync();
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
                            , new ChatAppDb());
            ((Page)obj).NavigationService.Navigate(page);
        }
    }
}
