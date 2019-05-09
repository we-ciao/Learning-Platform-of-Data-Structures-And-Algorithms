namespace DSAA.EntityFrameworkCore.Entity
{
    public class ProblemCategory
    {
        public virtual int ProblemId { set; get; }
        public virtual Problem Problem { set; get; }


        public virtual int CategoryId { set; get; }
        public virtual Category Category { set; get; }
    }
}
