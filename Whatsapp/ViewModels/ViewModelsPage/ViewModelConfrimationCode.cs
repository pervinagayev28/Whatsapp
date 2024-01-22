using ForecastDesign.Statics.StaticClasses.GetSmtpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Whatsapp.Commands;
using Whatsapp.Views.ViewPages;

namespace Whatsapp.ViewModels.ViewModelsPage
{
    public  class ViewModelConfrimationCode
    {
        public ICommand? ConfirmationCommand { get; set; }
        private string gmail { get; }
        public string VerifyCode { get; set; }

        public ViewModelConfrimationCode(string gmail)
        {
            this.gmail = gmail;
            GetVerifyCode();
            ConfirmationCommand = new Command(ExecuteConfrimationCommand, CanExecuteConfrimationCommandAsync);
        }
        private async void GetVerifyCode()
        {
            VerifyCode = await GetCode.GmailVerify(gmail!);
        }
        private bool CanExecuteConfrimationCommandAsync(object obj) =>
             VerifyCode == ((PasswordBox)((Page)obj).FindName("Code")).Password;

        private void ExecuteConfrimationCommand(object obj)
        {
            var page = new ViewNewPassword();
            page.DataContext = new ViiewModelNewPassword(gmail);
            ((Page)obj).NavigationService.Navigate(page);
        }
    }
}
