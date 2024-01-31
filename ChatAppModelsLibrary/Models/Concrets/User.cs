using ChatAppModelsLibrary.Models.BaseModels;
using ChatAppModelsLibrary.Models.Concrets;
using ChatAppService.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppModelsLibrary.Models
{
    public class User: ServiceINotifyPropertyChanged, IPrimaryKey<int>
    {
        public int Id { get; set; }

        public string Password { get; set; } = null!;
        public string? Bio { get; set; } = null!;
        public string Gmail { get => gmail; set { gmail = value; OnPropertyChanged(); } }

        public string? ImagePath { get => imagePath; set { imagePath = value; OnPropertyChanged(); } }

        public virtual ICollection<Message>? MessagesTo { get; set; }

        public virtual ICollection<Message>? MessagesFroms { get; set; }
        public virtual ICollection<UserConnection>? ConnectionTos { get; set; }
        public virtual ICollection<UserConnection>? ConnectionFroms { get; set; }



        //Not mapped propts.
        [NotMapped]
        public string? LastMessage { get => lastMessage; set { lastMessage = value; OnPropertyChanged(); } }
        private string? lastMessage;
        [NotMapped]
        public DateTime? LastMessageDate { get => lastMessageDate; set { lastMessageDate = value; OnPropertyChanged(); } }
        private DateTime? lastMessageDate;

        //back fields
        private string gmail = null!;
        private string? imagePath;

    }
}
