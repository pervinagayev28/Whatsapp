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
using System.Windows.Threading;
using System.Xml.XPath;
using Whatsapp.Commands;
using Whatsapp.DbContexts;
using Whatsapp.Models;
using Whatsapp.Services;
using Whatsapp.Views.ViewPages;
using Whatsapp.Views.ViewWindows;

namespace Whatsapp.ViewModels.ViewModelsPage
{


    class ViewModelSuccsessEntryed : ServiceINotifyPropertyChanged
    {
        private ManualResetEvent pauseEvent = new ManualResetEvent(true);

        private DispatcherTimer timer;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        //Timer timer;
        Grid grid;
        public ICommand? SelectedChatUser { get; set; }
        public ICommand? SendMessageCommand { get; set; }
        public ICommand? LogOutCommand { get; set; }
        public ICommand? AllUsersCommand { get; set; }
        public ICommand? OnlyChatUsersCommand { get; set; }
        public ICommand? ProfileCommand { get; set; }
        public UsersTb? User { get => user; set { user = value; OnPropertyChanged(); } }
        private static int tempId;
        public ObservableCollection<UsersTb> Users { get => users; set { users = value; OnPropertyChanged(); } }
        private ObservableCollection<UsersTb> users;
        public ObservableCollection<MessagesTb> Messages { get => messages; set { messages = value; OnPropertyChanged(); } }

        private ObservableCollection<MessagesTb> messages = new();
        public MyChatingAppContext? context { get; set; }


        private int currentSelectedUserId;
        private static bool check = false;
        private UsersTb? user;

        public ViewModelSuccsessEntryed(string Gmail, MyChatingAppContext? myChatingAppContext)
        {

            context = new();
            SelectedChatUser = new Command(ExecuteSelectedChatUser);
            SendMessageCommand = new Command(ExecuteSendMessageCommand, CanExecuteSendMessageCommand);
            LogOutCommand = new Command(ExecuteLogOutCommand);
            AllUsersCommand = new Command(ExecuteAllUsersCommandAsync);
            OnlyChatUsersCommand = new Command(ExecuteOnlyChatUsersCommand);
            ProfileCommand = new Command(ExecuteProfileCommand);
            start(Gmail);
        }

        private bool CanExecuteSendMessageCommand(object obj) =>
            currentSelectedUserId != 0;

        private void ExecuteProfileCommand(object obj)
        {
            var page = new WindowProfile();
            timer.Stop();
            page.DataContext = new ViewModelProfile(User, context);
            page.ShowDialog();
            timer.Start();
        }

        private async void ExecuteOnlyChatUsersCommand(object obj)
        {
            if (check)
            {
                check = false;
                await GetUsers();
            }
        }


        private async void ExecuteAllUsersCommandAsync(object obj)
        {
            check = true;
            await GetAllUsers();
            //await GetLastMessages();
        }

        private async void start(string Gmail)
        {
            User = await context.UsersTbs.FirstOrDefaultAsync(u => u.Gmail == Gmail)!;
            await GetUsers();
            await GetLastMessages();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += async (sender, e) => await TrickerDataBase();
            //await GetLastMessages();
            //timer.Start();
            //timer = new Timer(async state => await TrickerDataBase(), null, 0, 1000);


        }
        private async Task GetLastMessages()
        {
            timer?.Stop();


            pauseEvent.Reset();
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
            pauseEvent.Set();
            //Users = new(Users.OrderByDescending(u => u.LastMessageDate).ToList());
            timer?.Start();
        }


        private async Task GetAllUsers()
        {
            timer?.Stop();
            Users = new(await context.UsersTbs.ToListAsync());
            timer?.Start();
        }

        private async Task GetUsers()
        {
            pauseEvent.Reset();
           

            Users = new(await context.UsersTbs.
                            Where(u => u.MessagesFrom.Any(m => m.From.Id == User.Id) || u.MessagesTo.Any(m => m.To.Id == User.Id))
                            .ToListAsync());



            pauseEvent.Set();
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
                      MessageForVisual = x.Message+" "+x.Date.ToString("HH:mm")
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
                            Message = messages.Last().Message ,
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


        private async void ExecuteSendMessageCommand(object obj)
        {
            timer.Stop();
            await semaphore.WaitAsync();
            pauseEvent.Reset();
            await context.MessagesTbs.AddAsync(new MessagesTb() { UserId = User.Id, Message = ((TextBox)obj).Text, Date = DateTime.Now, ToId = currentSelectedUserId });
            await context.SaveChangesAsync();
            pauseEvent.Set();
            semaphore.Release();
            timer.Start();
            ((TextBox)obj).Text = "";
        }

        private void ExecuteSelectedChatUser(object obj)
        {
            grid = (Grid)obj;
            timer?.Start();
            currentSelectedUserId = Users[(int)((ListView)grid.FindName("list")).SelectedIndex].Id;
        }


    }
}
