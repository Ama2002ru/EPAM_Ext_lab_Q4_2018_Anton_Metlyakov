namespace Task6
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Общие свойства работников
    /// </summary>
    public interface IPerson
    {
        string Name { get; set; }

// хотел разделить Имя и метод Say человека на 2 интерфейса,
// но столкнулся в дальнейшем с затруднениями при работе с интерфейсной переменной.
// так что придется нанимать только тех работников, которые умеют говорить :)
        string[] Greetings { get; set; }

        string[] Farewell { get; set; }

        void Say(object sender, OfficeEventArgs args);
    }
}
