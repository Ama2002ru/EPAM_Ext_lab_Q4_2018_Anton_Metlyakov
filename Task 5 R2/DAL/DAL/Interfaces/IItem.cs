namespace DAL.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using DAL.Interfaces;
    using DAL.Utility;

    public interface IItem
    {
        int ID { get; set; }

        bool Delete();

        bool Save();

// Нарушаем принцип SRP :(
//        void Show();
    }
}