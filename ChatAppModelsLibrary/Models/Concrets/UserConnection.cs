using ChatAppModelsLibrary.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppModelsLibrary.Models.Concrets
{
    public class UserConnection:IPrimaryKey<int>
    {
        public int Id { get; set; }

        public int FromId { get; set; }
        public virtual  User From { get; set; }

        public int ToId { get; set; }
        public virtual User To { get; set; }
        public DateTime FromConnectedDate { get; set; }
        public DateTime ToConnectedDate { get; set; }
        public bool SofDeleteFrom { get; set; }
        public bool SoftDeleteTo { get; set; }
    }
}
