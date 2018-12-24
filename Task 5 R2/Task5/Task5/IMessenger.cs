namespace Task5
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Просто интерфейс с 2 методами
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Вывод строки
        /// </summary>
        /// <param name="s"></param>
        void Write(string s);

        /// <summary>
        /// Вывод строки + перевод строки
        /// </summary>
        /// <param name="s"></param>
        void WriteLine(string s);
    }
}
