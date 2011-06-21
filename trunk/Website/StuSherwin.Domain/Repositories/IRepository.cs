using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StuSherwin.Domain.Repositories
{
    public interface IRepository<T> 
        where T : class
    {
        T FindById(int id);
        void SaveChanges();
    }
}
