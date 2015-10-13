using System.Collections.Generic;

namespace DataAccess.Interface
{
    public interface IRepository<T>
    {
        IList<T> GetAll();
        void Save(T item);
    }
}
