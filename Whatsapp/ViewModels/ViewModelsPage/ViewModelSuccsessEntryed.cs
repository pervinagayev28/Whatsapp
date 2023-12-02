using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Whatsapp.Commands;
using Whatsapp.DbContexts;
using Whatsapp.Models;
using Whatsapp.Services;
using Whatsapp.Views.ViewPages;

namespace Whatsapp.ViewModels.ViewModelsPage
{

   
    class ViewModelSuccsessEntryed : ServiceINotifyPropertyChanged
    {
        private DispatcherTimer timer;

        public ICommand? SelectedChatUser { get; set; }
        public ICommand? SendMessageCommand { get; set; }
        public ICommand? LogOutCommand { get; set; }
        public UsersTb? User { get; set; }
        public ObservableCollection<UsersTb> Users { get; set; } = new();
        public ObservableCollection<MessagesTb> Messages { get => messages; set { messages = value; OnPropertyChanged(); } }

        private MyChatingAppContext context = new();

        private ObservableCollection<MessagesTb> messages = new();

        private int currentSelectedUserId;

        public ViewModelSuccsessEntryed(string Gmail)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TrickerDataBase;
            timer.Start();
            SelectedChatUser = new Command(ExecuteSelectedChatUser);
            SendMessageCommand = new Command(ExecuteSendMessageCommand);
            LogOutCommand = new Command(ExecuteLogOutCommand);
            User = context.UsersTbs.FirstOrDefault(u => u.Gmail == Gmail);
            GetUsers();
            GetLastMessages();
        }

        private void GetLastMessages()
        {
            foreach (var item in Users)
            {
                var result = from m in context.MessagesTbs
                             join fromUser in context.UsersTbs on m.UserId equals fromUser.Id
                             join toUser in context.UsersTbs on m.ToId equals toUser.Id
                             where fromUser.Id == User.Id && toUser.Id == item.Id || fromUser.Id == item.Id && toUser.Id == User.Id
                             select new
                             {
                                 m.Message,
                             };

                item.LastMessage = result.ToList().Last().Message;               
            }
        }

        private void GetUsers()
        {
            var result = (from m in context.MessagesTbs
                          join fromUser in context.UsersTbs on m.UserId equals fromUser.Id
                          join toUser in context.UsersTbs on m.ToId equals toUser.Id
                          select new
                          {
                              user = fromUser.Id == User.Id ? toUser : fromUser,
                          }).ToList()
                          .DistinctBy(x => x.user.Id);

            foreach (var item in result)
                Users.Add(new UsersTb()
                {
                    Id = item.user.Id,
                    ImagePath = item.user.ImagePath,
                    Gmail = item.user.Gmail,
                    Password = item.user.Password,
                });
        }

        private void ExecuteLogOutCommand(object obj)
        {
            var page = new ViewEntry();
            page.DataContext = new ViewModelEntry();
            ((Page)obj).NavigationService.Navigate(page);
        }

        private void TrickerDataBase(object? sender, EventArgs e)
        {
            messages.Clear();
            var result = from m in context.MessagesTbs
                         join fromUser in context.UsersTbs on m.UserId equals fromUser.Id
                         join toUser in context.UsersTbs on m.ToId equals toUser.Id
                         where fromUser.Id == User.Id && toUser.Id == currentSelectedUserId || fromUser.Id == currentSelectedUserId && toUser.Id == User.Id
                         select new
                         {
                             RightOrLeft = fromUser.Id == currentSelectedUserId ? 0 : 1,
                             m.Message,
                             m.Date
                         };
            try
            {
            foreach (var m in result)
                Messages.Add(new MessagesTb() { Message = m.Message + "  " + m.Date.ToString("HH:mm"), RightOrLeft = m.RightOrLeft });
                GetLastMessages();
            }
            catch (Exception)
            {
            }
        }

        private async void ExecuteSendMessageCommand(object obj)
        {
            await context.MessagesTbs.AddAsync(new MessagesTb() { UserId = User.Id, Message = obj.ToString()!, Date = DateTime.Now, ToId = currentSelectedUserId });
            await context.SaveChangesAsync();

        }

        private void ExecuteSelectedChatUser(object obj) =>
            currentSelectedUserId = Users[(int)obj].Id;

    }
}
