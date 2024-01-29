using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Whatsapp.Services;

namespace Whatsapp.Models.TestModels;

public partial class UsersTb:ServiceINotifyPropertyChanged
{
    private string? lastMessage;
    private DateTime? lastMessageDate;
    private string gmail = null!;
    private string? imagePath;
    public int Id { get; set; }

    public string Password { get; set; } = null!;
    public string ?Bio{ get; set; } = null!;
    public string Gmail { get => gmail; set { gmail = value; OnPropertyChanged(); } }

    public string? ImagePath { get => imagePath; set { imagePath = value; OnPropertyChanged(); } }

    public virtual ICollection<MessagesTb> MessagesTbTos { get; set; } = new List<MessagesTb>();

    public virtual ICollection<MessagesTb> MessagesTbUsers { get; set; } = new List<MessagesTb>();
    public virtual ICollection<ConnectionsTb> ConnectionsTbTos{ get; set; } = new List<ConnectionsTb>();
    public virtual ICollection<ConnectionsTb> ConnectionsTbFroms{ get; set; } = new List<ConnectionsTb>();

    [NotMapped]
    public string? LastMessage { get => lastMessage; set { lastMessage = value; OnPropertyChanged(); } }
    [NotMapped]
    public DateTime? LastMessageDate { get => lastMessageDate; set { lastMessageDate = value; OnPropertyChanged(); } }
}
