namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using DAL;

    /// <summary>
    /// Интерфейс для обобщения поведения. Нужен ли ?
    /// </summary>
    public interface IItem
    {
        int ID { get; set; }

        bool Delete();

        bool Save(IdbConnector db);

        void Show();
    }
}