using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Whatsapp.Commands;
using Whatsapp.DbContexts;
using Whatsapp.Models;
using Whatsapp.Services;

namespace Whatsapp.ViewModels.ViewModelsPage
{
    public class ViewModelProfile:ServiceINotifyPropertyChanged
    {
        public ICommand? ChangePasswordCommand { get; set; }
        public ICommand? ChangeImageUrlCommand { get; set; }
        public ICommand? ChangeImageFromPCCommand { get; set; }
        public ICommand? CloseCommand { get; set; }
        private UsersTb? user;
        public UsersTb? User { get => user; set { user = value; OnPropertyChanged(); } }

        public MyChatingAppContext? Context { get; }

        public ViewModelProfile(UsersTb user, DbContexts.MyChatingAppContext? context)
        {
            User = user;
            Context = context;
            ChangePasswordCommand = new Command(ExecuteChangePasswordCommand, CanExecuteChangePasswordCommand);
            ChangeImageUrlCommand = new Command(ExecuteChangeImageUrlCommand, CanExecuteChangeImageUrlCommand);
            ChangeImageFromPCCommand = new Command(ExecuteChangeImageFromPCCommand);
            CloseCommand = new Command(ExecuteCloseCommand);
        }

        private bool CanExecuteChangeImageUrlCommand(object obj) =>
            User.ImagePath != obj.ToString();
        private async void ExecuteCloseCommand(object obj)
        {
            await Context!.SaveChangesAsync();
            ((Window)obj).Close();
        }

        private void ExecuteChangeImageFromPCCommand(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                if (!File.Exists($"..\\..\\..\\Images\\{Path.GetFileName(fileDialog.FileName)}"))
                    File.Copy(fileDialog.FileName, $"..\\..\\..\\Images\\{Path.GetFileName(fileDialog.FileName)}");

                string filename = Path.GetFileName(fileDialog.FileName);
                User!.ImagePath = $@"\Images\{Path.GetFileName(fileDialog.FileName)}";
            }
        }

        private void ExecuteChangeImageUrlCommand(object obj) =>
            User.ImagePath = obj.ToString();
        private bool CanExecuteChangePasswordCommand(object obj) =>
            Regex.IsMatch(obj.ToString()!, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()-_+=])[A-Za-z\d!@#$%^&*()-_+=]{8,}$")
            && User.Password != obj.ToString()!;
        private void ExecuteChangePasswordCommand(object obj)=>
            User!.Password = obj.ToString()!;
        
    }
}
