using System;
using System.Collections.Generic;
using Whatsapp.Models.TestModels;

namespace Whatsapp.Models;

public partial class ConnectionsTb
{
    public int Id { get; set; }

    public int FromId { get; set; }
    public UsersTb From{ get; set; }

    public int ToId { get; set; }
    public UsersTb To { get; set; }

    public bool SofDeleteFrom { get; set; }
    public bool SoftDeleteTo { get; set; }
}
