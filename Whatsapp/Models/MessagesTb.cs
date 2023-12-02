using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Whatsapp.Models;

public partial class MessagesTb
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime Date { get; set; }

    public int? ToId { get; set; }
    [NotMapped]

    public int RightOrLeft { get;  set; }
}
