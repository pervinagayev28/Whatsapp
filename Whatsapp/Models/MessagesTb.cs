using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Whatsapp.Services;

namespace Whatsapp.Models;

public partial class MessagesTb:ServiceINotifyPropertyChanged
{
    private DateTime date;
    private string message = null!;
    private string messageForVisual;

    [NotMapped]
    public string MessageForVisual { get => messageForVisual; set { messageForVisual = value; OnPropertyChanged(); } }
    public int Id { get; set; }


    public string Message { get => message; set { message = value; OnPropertyChanged(); } }

    public DateTime Date { get => date; set { date = value; OnPropertyChanged(); } }

    public int? ToId { get; set; }
    public  UsersTb? To { get; set; }

    public int FromId { get; set; }
    public  UsersTb From { get; set; } = null!;

    public int UserId { get; set; }
    public UsersTb User { get; set; }

    [NotMapped]

    public int RightOrLeft { get;  set; }
}
