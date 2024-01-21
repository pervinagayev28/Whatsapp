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
using Whatsapp.DbContexts;
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
            Log.Information("*********************************** enter to login ***********************************");

            //page.DataContext = new ViewModelSuccsessEntryed(((PasswordBox)((Page)obj).FindName("GmailTextBox")).Password
                            //,App._serviceProvider?.GetRequiredService<MyChatingAppContext>());
            page.DataContext = new ViewModelSuccsessEntryed(((PasswordBox)((Page)obj).FindName("GmailTextBox")).Password
                            ,new MyChatingAppContext());
           ((Page)obj).NavigationService.Navigate(page);
        }
    }
}
