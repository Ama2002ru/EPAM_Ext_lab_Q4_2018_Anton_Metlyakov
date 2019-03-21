namespace DAL
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///  Интерфейс, данный при постановке задания
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class, new()
    {
        T Get(int id);

        List<T> GetAll();

        bool Save(T entity);

        bool Delete(int id);
    }
}