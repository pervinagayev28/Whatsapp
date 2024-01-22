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
            SendMessageCommand = new Command(ExecuteSendMessageCommand);
            LogOutCommand = new Command(ExecuteLogOutCommand);
            AllUsersCommand = new Command(ExecuteAllUsersCommandAsync);
            OnlyChatUsersCommand = new Command(ExecuteOnlyChatUsersCommand);
            ProfileCommand = new Command(ExecuteProfileCommand);
            start(Gmail);
        }

        private void ExecuteProfileCommand(object obj)
        {
            var page = new WindowProfile();
            page.DataContext = new ViewModelProfile(User,context);
            page.ShowDialog();
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
            foreach (var item in Users)
            {
                var result = await (from m in context.MessagesTbs
                                    join fromUser in context.UsersTbs on m.UserId equals fromUser.Id
                                    join toUser in context.UsersTbs on m.ToId equals toUser.Id
                                    where fromUser.Id == User.Id && toUser.Id == item.Id || fromUser.Id == item.Id && toUser.Id == User.Id
                                    orderby m.Date descending
                                    select new
                                    {
                                        m.Date,
                                        m.Message

                                    })
                          .FirstOrDefaultAsync();



                item.LastMessage = result?.Message;
                item.LastMessageDate = result?.Date;
            }
            //Users = new(Users.OrderByDescending(u => u.LastMessageDate).ToList());
            timer?.Start();
        }


        private async Task GetAllUsers()
        {
            //Users = new((await (from fromUser in context?.UsersTbs
            //                    join m in context?.MessagesTbs on fromUser.Id equals m.UserId into fromUserMessages
            //                    from m in fromUserMessages.DefaultIfEmpty()
            //                    join toUser in context?.UsersTbs on m.ToId equals toUser.Id into toUserMessages
            //                    from toUser in toUserMessages.DefaultIfEmpty()
            //                    select new UsersTb
            //                    {
            //                        Id = fromUser.Id == User!.Id ? toUser.Id : fromUser.Id,
            //                        ImagePath = fromUser.Id == User!.Id ? toUser.ImagePath : fromUser.ImagePath,
            //                        Gmail = fromUser.Id == User!.Id ? toUser.Gmail : fromUser.Gmail,
            //                        Password = fromUser.Id == User!.Id ? toUser.Password : fromUser.Password,
            //                        LastMessage = m.Message,
            //                        LastMessageDate = m.Date
            //                    })
            //   .OrderByDescending(u => u.LastMessageDate)
            //   .ToListAsync()).DistinctBy(u => u.Id));

            timer?.Stop();
            Users = new(await context.UsersTbs.ToListAsync());
            timer?.Start();
        }

        private async Task GetUsers()
        {
            timer?.Stop();
            Users?.Clear();
            Users = new((await (from m in context?.MessagesTbs
                                join fromUser in context!.UsersTbs on m.UserId equals fromUser.Id
                                join toUser in context.UsersTbs on m.ToId equals toUser.Id
                                select new UsersTb
                                {
                                    Id = fromUser.Id == User!.Id ? toUser.Id : fromUser.Id,
                                    ImagePath = fromUser.Id == User!.Id ? toUser.ImagePath : fromUser.ImagePath,
                                    Gmail = fromUser.Id == User!.Id ? toUser.Gmail : fromUser.Gmail,
                                    Password = fromUser.Id == User!.Id ? toUser.Password : fromUser.Password,
                                    LastMessage = m.Message,
                                    LastMessageDate = m.Date
                                })
                                  .OrderByDescending(u => u.LastMessageDate)
                                  .ToListAsync()).DistinctBy(u => u.Id));
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
            var query = from m in context.MessagesTbs
                        join fromUser in context.UsersTbs on m.UserId equals fromUser.Id
                        join toUser in context.UsersTbs on m.ToId equals toUser.Id
                        where fromUser.Id == User.Id && toUser.Id == currentSelectedUserId || fromUser.Id == currentSelectedUserId && toUser.Id == User.Id
                        select new
                        {
                            RightOrLeft = fromUser.Id == currentSelectedUserId ? 0 : 1,
                            m.Message,
                            m.Date
                        };

            var result = await query.ToListAsync();

            if (tempId == currentSelectedUserId)
            {
                if (Messages.Count() != result.Count())
                {
                    try
                    {

                        Messages.Add(new MessagesTb() { Message = result.Last().Message + "  " + result.Last().Date.ToString("HH:mm"), RightOrLeft = result.Last().RightOrLeft });
                    }
                    catch (Exception)
                    {

                    }
                    ((ListView)grid.FindName("list2")).ScrollIntoView(Messages.Last());
                    //Users = new(Users.OrderByDescending(u => u.LastMessageDate).ToList());
                }
            }
            else
            {

                Messages.Clear();
                if (grid is not null)
                    ((ListView)grid.FindName("list2")).ScrollIntoView(Messages);
                foreach (var m in result)
                    Messages.Add(new MessagesTb() { Message = m.Message + "  " + m.Date.ToString("HH:mm"), RightOrLeft = m.RightOrLeft });
                if (result.Count != 0 && grid is not null)
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
            try
            {
                await semaphore.WaitAsync();
                await context.MessagesTbs.AddAsync(new MessagesTb() { UserId = User.Id, Message = ((TextBox)obj).Text, Date = DateTime.Now, ToId = currentSelectedUserId });
                await context.SaveChangesAsync();
            }
            finally
            {
                semaphore.Release();
            }
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
