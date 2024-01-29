using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.XPath;
using Whatsapp.Commands;
using Whatsapp.Models;
using Whatsapp.Models.TestModels;
using Whatsapp.Services;
using Whatsapp.Views.ViewPages;
using Whatsapp.Views.ViewWindows;

namespace Whatsapp.ViewModels.ViewModelsPage
{


    class ViewModelSuccsessEntryed : ServiceINotifyPropertyChanged
    {
        private static DateTime? temp;
        private ManualResetEvent pauseEvent = new ManualResetEvent(true);

        private DispatcherTimer? timer;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        Grid grid;

        private List<ConnectionsTb>? connections;

        public ICommand? SelectedChatUser { get; set; }
        public ICommand? SendMessageCommand { get; set; }
        public ICommand? LogOutCommand { get; set; }
        public ICommand? AllUsersCommand { get; set; }
        public ICommand? OnlyChatUsersCommand { get; set; }
        public ICommand? ProfileCommand { get; set; }
        public ICommand? CloseOpenedImageCommand { get; set; }
        public ICommand? GetImageCommand { get; set; }
        public ICommand? DeleteCommand { get; set; }
        public UsersTb? User { get => user; set { user = value; OnPropertyChanged(); } }
        private static int tempId;
        public ObservableCollection<UsersTb> Users { get => users; set { users = value; OnPropertyChanged(); } }
        private ObservableCollection<UsersTb> users;
        public ObservableCollection<MessagesTb> Messages { get => messages; set { messages = value; OnPropertyChanged(); } }

        private ObservableCollection<MessagesTb> messages = new();
        public MyChatingAppContext? context { get; set; }
        public string SelectedUserImagePath { get => selectedUserImagePath; set { selectedUserImagePath = value; OnPropertyChanged(); } }


        private int currentSelectedUserId;
        private static bool check = false;
        private UsersTb? user;
        private string selectedUserImagePath;

        public ViewModelSuccsessEntryed(string Gmail, MyChatingAppContext? myChatingAppContext)
        {

            context = new();
            SelectedChatUser = new CommandAsync(ExecuteSelectedChatUser);
            SendMessageCommand = new CommandAsync(ExecuteSendMessageCommand, CanExecuteSendMessageCommand);
            LogOutCommand = new Command(ExecuteLogOutCommand);
            AllUsersCommand = new CommandAsync(ExecuteAllUsersCommandAsync, CanExecuteAllUsersCommandAsync);
            OnlyChatUsersCommand = new CommandAsync(ExecuteOnlyChatUsersCommand, CanExecuteOnlyChatUsersCommand);
            ProfileCommand = new CommandAsync(ExecuteProfileCommand);
            CloseOpenedImageCommand = new Command(ExecuteCloseOpenedImageCommand);
            GetImageCommand = new Command(ExecuteGetImageCommand);
            DeleteCommand = new CommandAsync(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            start(Gmail);
        }

        private bool CanExecuteAllUsersCommandAsync(object obj) =>
            !check;

        private bool CanExecuteOnlyChatUsersCommand(object obj) =>
            check;

        private bool CanExecuteDeleteCommand(object obj) =>
            ((ListView)obj)?.SelectedItems?.Count > 0;

        private async Task ExecuteDeleteCommand(object arg)
        {
            timer?.Stop();
            foreach (var item in ((ListView)arg)?.SelectedItems!)
            {
                foreach (var connect in connections!)
                {
                    if (connect.FromId == User.Id && connect.ToId == (item as UsersTb)?.Id)
                        connect.SofDeleteFrom = true;
                    else if (connect.ToId == User.Id && connect.FromId == (item as UsersTb)?.Id)
                        connect.SoftDeleteTo = true;
                }
            }
             ((ListView)arg)?.SelectedItems?.Clear();
            currentSelectedUserId = default;
            await context?.SaveChangesAsync()!;
            await GetUsers();
            timer?.Start();
        }



        private void ExecuteGetImageCommand(object arg)
        {
            ((Grid)((Grid)arg).FindName("OpenedImageGrid")).Visibility = Visibility.Visible;
            SelectedUserImagePath = (((ListView)((Grid)arg).FindName("list")).SelectedItem as UsersTb)!.ImagePath!;
        }

        private void ExecuteCloseOpenedImageCommand(object arg)
        {
            ((Grid)arg).Visibility = Visibility.Hidden;
        }

        private bool CanExecuteSendMessageCommand(object obj) =>
            currentSelectedUserId != 0;

        private async Task ExecuteProfileCommand(object obj)
        {
            var page = new WindowProfile();
            timer.Stop();
            page.DataContext = new ViewModelProfile(User, context);
            page.ShowDialog();
            timer.Start();
            await Task.CompletedTask;
        }

        private async Task ExecuteOnlyChatUsersCommand(object obj)
        {
            check = false;
            await GetUsers();
        }


        private async Task ExecuteAllUsersCommandAsync(object obj)
        {
            check = true;
            await GetAllUsers();
        }

        private async void start(string Gmail)
        {
            User = await context?.UsersTbs.FirstOrDefaultAsync(u => u.Gmail == Gmail)!;
            await GetUsers();
            await GetLastMessages();
            connections = await context.ConnectionsTb.ToListAsync();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += async (sender, e) => await TrickerDataBase();
        }
        private async Task GetLastMessages()
        {
            timer?.Stop();
            foreach (var item in users)
            {

                var messages = await context.MessagesTbs
                  .Where(message => message.User.Id == User.Id
                                 && message.To.Id == item.Id
                                 || message.To.Id == User.Id
                                 && message.User.Id == item.Id)
                  .Include(x => x.User)
                  .Include(y => y.To)
                  .OrderByDescending(m => m.Date)
                  .FirstOrDefaultAsync();

                item.LastMessage = messages?.Message;
                item.LastMessageDate = messages?.Date;
            }

            if (temp < users?.FirstOrDefault(u => u.Id == currentSelectedUserId)?.LastMessageDate || timer is null)
                Users = new(Users.OrderByDescending(u => u.LastMessageDate).ToList());
            temp = users?.FirstOrDefault(u => u.Id == currentSelectedUserId)?.LastMessageDate;
            connections = await context.ConnectionsTb.ToListAsync();
            timer?.Start();
        }


        private async Task GetAllUsers()
        {
            timer?.Stop();
            Users = new(await context!.UsersTbs.ToListAsync());
            timer?.Start();
        }

        private async Task GetUsers()
        {
            pauseEvent.Reset();

            timer?.Stop();
            Users = new(await context.UsersTbs
                             .Where(u => (u.Id != User!.Id) && (u.MessagesTbUsers
                                        .Any(m => m.ToId == User.Id || m.UserId == User.Id) || u.MessagesTbTos
                                        .Any(m => m.ToId == User.Id || m.UserId == User.Id))
                                         && u.ConnectionsTbFroms
                                        .Any(c => c.FromId == User.Id && !c.SofDeleteFrom || c.ToId == User.Id && !c.SoftDeleteTo) || u.ConnectionsTbTos
                                        .Any(c => c.FromId == User.Id && !c.SofDeleteFrom || c.ToId == User.Id && !c.SoftDeleteTo) && u.Id != User.Id)
                            .Include(u => u.MessagesTbUsers)
                            .Include(u => u.MessagesTbTos)
                            .ToListAsync());
            await GetLastMessages();
            pauseEvent.Set();
            timer?.Start();
        }

        private void ExecuteLogOutCommand(object obj)
        {
            var page = new ViewEntry();
            page.DataContext = new ViewModelEntry();
            ((Page)obj).NavigationService.Navigate(page);

        }


        public async Task TrickerDataBase()
        {

            var messages = await context.MessagesTbs
                  .Where(message => message.User.Id == User.Id
                                 && message.To.Id == currentSelectedUserId
                                 || message.To.Id == User.Id
                                 && message.User.Id == currentSelectedUserId)
                  .Include(x => x.User)
                  .Include(y => y.To)
                  .Select(x => new MessagesTb
                  {
                      RightOrLeft = x.UserId == currentSelectedUserId ? 0 : 1,
                      Message = x.Message,
                      Date = x.Date,
                      MessageForVisual = x.Message + " " + x.Date.ToString("HH:mm")
                  })
                  .OrderBy(m => m.Date)
                  .ToListAsync();


            if (tempId == currentSelectedUserId)
            {
                if (Messages.Count() != 0 && messages.Count() != 0)
                {

                    if (Messages.Last().Date != messages.Last().Date)
                    {

                        Messages.Add(new MessagesTb()
                        {
                            Message = messages.Last().Message,
                            MessageForVisual = messages.Last().MessageForVisual,
                            RightOrLeft = messages.Last().RightOrLeft,
                            Date = messages.Last().Date,
                        });

                        ((ListView)grid.FindName("list2")).ScrollIntoView(Messages.Last());
                        Users = new(Users.OrderByDescending(u => u.LastMessageDate).ToList());
                    }
                }
            }
            else
            {

                if (grid is not null)
                    ((ListView)grid.FindName("list2")).ScrollIntoView(Messages);
                Messages = new(messages);

                if (messages.Count != 0 && grid is not null)
                {
                    ((ListView)grid.FindName("list2")).
                        ScrollIntoView(((ListView)grid.FindName("list2")).
                        Items[((ListView)grid.FindName("list2")).Items.Count - 1]);
                }
            }

            await GetLastMessages();

            tempId = currentSelectedUserId;
        }


        private async Task ExecuteSendMessageCommand(object obj)
        {
            timer?.Stop();
            await semaphore.WaitAsync();
            await context!.MessagesTbs.AddAsync(new MessagesTb() { UserId = User.Id, Message = ((TextBox)obj).Text, Date = DateTime.Now, ToId = currentSelectedUserId });
            bool check = true;
            foreach (var item in connections!)
            {
                if ((item.FromId == User.Id && item.ToId == currentSelectedUserId) || (item.ToId == User.Id && item.FromId == currentSelectedUserId))
                {
                    check = false;
                    if (item.FromId == User.Id)
                    {
                        item.SoftDeleteTo = false;
                        item.SofDeleteFrom= false;
                    }
                    else if (item.ToId == User.Id)
                    {
                        item.SoftDeleteTo = false;
                        item.SofDeleteFrom = false;
                    }
                }


            }
            if (check)
                await context!.ConnectionsTb.AddAsync(new ConnectionsTb()
                {
                    FromId = User.Id,
                    ToId = currentSelectedUserId
                });

            await context.SaveChangesAsync();
            await GetUsers();
            semaphore.Release();
            timer?.Start();
            ((TextBox)obj).Text = "";
        }

        private async Task ExecuteSelectedChatUser(object obj)
        {
            grid = (Grid)obj;
            timer?.Start();
            currentSelectedUserId = Users[(int)((ListView)grid.FindName("list")).SelectedIndex].Id;
            await Task.CompletedTask;
        }


    }
}
