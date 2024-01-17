using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
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
using System.Xml.XPath;
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

        public MyChatingAppContext? context { get; }

        private ObservableCollection<MessagesTb> messages = new();

        private int currentSelectedUserId;

        public ViewModelSuccsessEntryed(string Gmail, MyChatingAppContext? myChatingAppContext)
        {
            Log.Information("***************************************** constructor started *****************************************");
            context = myChatingAppContext;
            SelectedChatUser = new Command(ExecuteSelectedChatUser);
            SendMessageCommand = new Command(ExecuteSendMessageCommand);
            LogOutCommand = new Command(ExecuteLogOutCommand);
            start(Gmail);
            Log.Information("***************************************** constructor finished *****************************************");
        }
        private async void start(string Gmail)
        {
            User = await context.UsersTbs.FirstOrDefaultAsync(u => u.Gmail == Gmail)!;
            await GetLastMessages();
            await GetUsers();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TrickerDataBase;
            timer.Start();
        }
        private async Task GetLastMessages()
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
                item.LastMessage = (await result.ToListAsync()).Last().Message;
            }
        }

        private async Task GetUsers()
        {
            var result = (await (from m in context?.MessagesTbs
                                 join fromUser in context!.UsersTbs on m.UserId equals fromUser.Id
                                 join toUser in context.UsersTbs on m.ToId equals toUser.Id
                                 select new
                                 {
                                     user = fromUser.Id == User!.Id ? toUser : fromUser,
                                 }).ToListAsync())
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

        private async void TrickerDataBase(object? sender, EventArgs e)
        {
            messages.Clear();
            var result = await (from m in context.MessagesTbs
                         join fromUser in context.UsersTbs on m.UserId equals fromUser.Id
                         join toUser in context.UsersTbs on m.ToId equals toUser.Id
                         where fromUser.Id == User.Id && toUser.Id == currentSelectedUserId || fromUser.Id == currentSelectedUserId && toUser.Id == User.Id
                         select new
                         {
                             RightOrLeft = fromUser.Id == currentSelectedUserId ? 0 : 1,
                             m.Message,
                             m.Date
                         }).ToListAsync();
            try
            {
                foreach (var m in result)
                    Messages.Add(new MessagesTb() { Message = m.Message + "  " + m.Date.ToString("HH:mm"), RightOrLeft = m.RightOrLeft });
                await GetLastMessages();
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
