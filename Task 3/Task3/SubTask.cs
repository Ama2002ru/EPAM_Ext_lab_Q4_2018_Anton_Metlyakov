namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    // ki прекрасный вариант с наследованием и реализацией общего метода для вызова + массив заранее инициализированных объектов. Я бы сделал так же. 
    public abstract class SubTask
    {
        /// <summary>
        /// Main functionality of object will be placed here
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// returns descriptive string to menu builder
        /// </summary>
        /// <returns> returns descriptive string to menu builder </returns>
        public abstract string TellAboutMyself();
    }
}
