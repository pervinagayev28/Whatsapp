using ChatAppService.Services;
using ForecastDesign.Statics.StaticClasses.GetSmtpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public  class ViewModelConfrimationCode:ServiceINotifyPropertyChanged
    {
        private Visibility loadingVisibility;
        public Visibility LoadingVisibility { get => loadingVisibility; set { loadingVisibility = value; OnPropertyChanged(); } }
        public ICommand? ConfirmationCommand { get; set; }
        public ICommand? GoBackCommand { get; set; }
        public ICommand? CloseCommand { get; set; }
        private string gmail { get; }
        public string VerifyCode { get; set; }

        public ViewModelConfrimationCode(string gmail)
        {
            this.gmail = gmail;
            Thread loading = new (GetVerifyCode);
            loading.Start();
            ConfirmationCommand = new Command(ExecuteConfrimationCommand, CanExecuteConfrimationCommandAsync);
            GoBackCommand = new Command(ExecuteGoBackCommand);
            CloseCommand = new Command(ExecuteCloseCommand);
        }
        private async void GetVerifyCode()
        {
            VerifyCode = await GetCode.GmailVerify(gmail!);
            LoadingVisibility = Visibility.Hidden;
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

        private void ExecuteGoBackCommand(object obj) =>
            ((Page)obj).NavigationService.GoBack();
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
