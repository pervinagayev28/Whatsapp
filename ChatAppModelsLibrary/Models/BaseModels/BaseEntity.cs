using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppModelsLibrary.Models.BaseModels
{
    public  class BaseEntity<TPrimaryKey>
    {
        public int Id { get; set; }
    }
}
