using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppModelsLibrary.Models.BaseModels
{
    public  interface IPrimaryKey <PrimaryKey>
    {
        public PrimaryKey  Id { get; set; }
    }
}
