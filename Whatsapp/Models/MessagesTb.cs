using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Whatsapp.Services;

namespace Whatsapp.Models.TestModels;

public partial class MessagesTb: ServiceINotifyPropertyChanged
{
    private DateTime date;
    private string message = null!;
    private string messageForVisual;

    [NotMapped]
    public string MessageForVisual { get => messageForVisual; set { messageForVisual = value; OnPropertyChanged(); } }
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Message { get => message; set { message = value; OnPropertyChanged(); } }

    public DateTime Date { get => date; set { date = value; OnPropertyChanged(); } }


    public int? ToId { get; set; }

    public virtual UsersTb? To { get; set; }

    public virtual UsersTb User { get; set; } = null!;

    [NotMapped]

    public int RightOrLeft { get; set; }
}
