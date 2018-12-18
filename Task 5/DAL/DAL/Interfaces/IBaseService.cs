namespace DAL.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IBaseService<T> where T : class, new()
    {
        T Get(int id);

        List<T> GetAll();

        bool Save(T entity);

        bool Delete(int id);
    }
}