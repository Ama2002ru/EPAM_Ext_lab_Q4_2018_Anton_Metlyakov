namespace Task6
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// производный класс параметров для передачи в EventHandler
    /// </summary>
    public class OfficeEventArgs : EventArgs
    {
        /// <summary>
        /// Код события
        /// </summary>
        public OfficeEvent Event { get; set; }

        /// <summary>
        /// Объект события
        /// </summary>
        public IPerson Person { get; set; }

        /// <summary>
        /// время события
        /// </summary>
        public DateTime EventTime { get; set; }
    }
}