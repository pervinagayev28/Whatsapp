using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Whatsapp.Services;

namespace Whatsapp.Models;

public partial class UsersTb:ServiceINotifyPropertyChanged
{
    private string? lastMessage;

    public int Id { get; set; }

    public string Gmail { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ImagePath { get; set; }
    [NotMapped]
    public string? LastMessage { get => lastMessage; set { lastMessage = value; OnPropertyChanged(); } }


}
