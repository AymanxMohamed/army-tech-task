using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArmyTechTask.DataAccess.IRepository
{
    public interface IRepository<T> where T : class 
    {
        Task<T> GetALl(
            Expression<Func<T, bool>> expression = null,
            List<string> includes = null
        );
    }
}
