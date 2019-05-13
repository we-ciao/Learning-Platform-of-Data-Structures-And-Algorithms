using System;
using System.Collections.Generic;
using System.Text;

namespace DSAA.EntityFrameworkCore.Entity
{
    public class LearnCategory
    {
        public virtual int LearnId { set; get; }
        public virtual Learn Learn { set; get; }


        public virtual int CategoryId { set; get; }
        public virtual Category Category { set; get; }
    }
}
