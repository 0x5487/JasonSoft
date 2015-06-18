using System;

namespace JasonSoft.Math
{
    public class Pager
    {
        public Pager(Int32 pageNumber, Int32 pageSize)
        {
            if(pageNumber <= 0) throw new ArgumentException("pageNumber can't be less than or equal than 0");
            if(pageSize <= 0) throw new ArgumentException("pageSize can't be less than 0");

            if(pageNumber > 1)
            {
                Skip = (pageNumber - 1)*pageSize;
            }

            Take = pageNumber * pageSize;
        }

        public Int32 Take { get; private set; }
        public Int32 Skip { get; private set; }
    }
}
