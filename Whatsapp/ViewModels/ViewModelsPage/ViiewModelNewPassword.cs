﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Whatsapp.Commands;
using Whatsapp.Views.ViewPages;
using ChatAppDatabaseLibrary.Contexts;
using ChatAppModelsLibrary.Models;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows;

namespace Whatsapp.ViewModels.ViewModelsPage
{
    public class ViiewModelNewPassword
    {
        public ICommand? ConfirmCommand { get; set; }
        public string Gmail { get; }
        public ICommand? GoBackCommand { get; set; }
        public ICommand? CloseCommand { get; set; }
        public ViiewModelNewPassword(string gmail)
        {
            Gmail = gmail;
            ConfirmCommand = new Command(ExecuteConfirmCommand, CanExecuteConfirmCommand);
            GoBackCommand = new Command(ExecuteGoBackCommand);
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

        private void ExecuteGoBackCommand(object obj) =>
            ((Page)obj).NavigationService.GoBack();

        private bool CanExecuteConfirmCommand(object obj) =>
            ((PasswordBox)((Page)obj).FindName("CodeOne")).Password ==
            ((PasswordBox)((Page)obj).FindName("CodeOne")).Password &&
            Regex.IsMatch(((PasswordBox)((Page)obj)
            .FindName("CodeOne")).Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()-_+=])[A-Za-z\d!@#$%^&*()-_+=]{8,}$");


        private async void ExecuteConfirmCommand(object obj)
        {
            var user = new User()
            {
                Gmail = Gmail,
                Password = ((PasswordBox)((Page)obj).FindName("CodeOne")).Password,
                ImagePath = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"
            };
            var context = new ChatAppDb();
            await context.UsersTbs.AddAsync(user);
            await context.SaveChangesAsync();
            var page = new SuccessfulLogin();
            page.DataContext = new ViewModelSuccsessEntryed(Gmail, context);
            ((Page)obj).NavigationService.Navigate(page);



        }
    }
}
