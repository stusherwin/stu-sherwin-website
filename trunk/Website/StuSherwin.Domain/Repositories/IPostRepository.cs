using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StuSherwin.Domain.Entities;

namespace StuSherwin.Domain.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> FindAllPublishedByCategoryCode(string categoryCode);
        Post FindByOldUrl(string oldUrl);
        Post FindByCode(string code);
    }
}
