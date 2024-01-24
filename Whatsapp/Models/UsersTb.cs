using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Whatsapp.Services;

namespace Whatsapp.Models;

public partial class UsersTb:ServiceINotifyPropertyChanged
{

    private string? lastMessage;
    private DateTime? lastMessageDate;
    private string gmail = null!;
    private string? imagePath;

    public int Id { get; set; }

    public string Gmail { get => gmail; set { gmail = value; OnPropertyChanged(); } }
    public string Password { get; set; } = null!;

    public string? ImagePath { get => imagePath; set { imagePath = value; OnPropertyChanged(); } }
    [NotMapped]
    public string? LastMessage { get => lastMessage; set { lastMessage = value; OnPropertyChanged(); } }
    [NotMapped]
    public DateTime ?LastMessageDate { get => lastMessageDate;  set { lastMessageDate = value; OnPropertyChanged(); } }

    public virtual ICollection<MessagesTb> MessagesFrom { get; set; } = new List<MessagesTb>();
    public virtual ICollection<MessagesTb> MessagesTo { get; set; } = new List<MessagesTb>();

}
