﻿using ChatAppModelsLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Whatsapp.Commands;
using Whatsapp.UnitOfWorks.BaseUnitOfWorks;
using Whatsapp.UnitOfWorks.Concrets;
using Whatsapp.Views.ViewPages;

namespace Whatsapp.ViewModels.ViewModelsPage
{
    public class ViewModelRegistration
    {
        private readonly IUnitOfWork unitOfWork;
        private List<string> gmails;
        public ICommand? SendCodeCommand { get; set; }
        public ViewModelRegistration()
        {
            unitOfWork = new UnitOfWork();
            CheckGmail();
            SendCodeCommand = new Command(ExecuteSendCodeCommand, CanExecuteSendCodeCommand);
        }

        private async void CheckGmail() =>
            gmails = await (await unitOfWork.GetRepository<User, int>().GetAll()).Select(x => x.Gmail).ToListAsync();

        private bool CanExecuteSendCodeCommand(object obj) =>
            Regex.IsMatch(((PasswordBox)((Page)obj)
                .FindName("gmail")).Password, @"^[a-zA-Z0-9._%+-]+@gmail\.com$")
            && !gmails.Contains(((PasswordBox)((Page)obj)
                .FindName("gmail")).Password);

        private void ExecuteSendCodeCommand(object obj)
        {
            var gmail = ((PasswordBox)((Page)obj).FindName("gmail")).Password;
            var page = new ViewConfrimationCode();
            page.DataContext = new ViewModelConfrimationCode(gmail);
            ((Page)obj).NavigationService.Navigate(page);
        }
    }
}
