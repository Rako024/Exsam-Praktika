using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RespositoryAbstracts
{
    public interface IGenericRepository<T> where T : class,new()
    {
        void Add(T entity);
        void Remove(T entity);
        void Commit();
        T Get(Func<T, bool>? func = null);
        List<T> GetAll(Func<T,bool>? func = null);

    }
}
