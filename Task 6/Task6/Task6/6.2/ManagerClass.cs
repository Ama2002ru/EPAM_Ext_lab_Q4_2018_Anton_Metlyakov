namespace Task6
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// "Стандартный" Manager
    /// </summary>
    public class Manager : IPerson
    {
         /// <summary>
        /// Конструктор с параметром Имя. Можно ли его красиво унаследовать ?
        /// </summary>
        /// <param name="name"></param>
        public Manager(string name) 
        {
            Name = name;
            Greetings = new string[]
            {
                "Доброе утро",
                "Доброе утро",
                "Доброе утро",
                "Доброе утро",
                "Доброе утро",
                "Доброе утро",
                "Доброе утро",
                "Доброе утро",
                "Доброе утро",
                "Доброе утро",
                "Доброе утро", // 10 часов
                "Ты опоздал",
                "Ты опоздал",
                "Ты опоздал",
                "Ты опоздал",
                "Ты опоздал",
                "Ты опоздал",
                "Ты сильно опоздал", // 17 часов
                "Ты сильно опоздал",
                "Ты сильно опоздал",
                "Ты сильно опоздал",
                "Ты сильно опоздал",
                "Ты сильно опоздал",
                "Ты сильно опоздал",
            };
            Farewell = new string[] // 24 варианта
            {
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания",
                "До свидания"
            };
        }

        /// <summary>
        /// Почасовой массив прощаний
        /// </summary>
        public string[] Farewell { get; set; }

        /// <summary>
        /// Почасовой массив приветствий
        /// </summary>
        public string[] Greetings { get; set; }

        /// <summary>
        /// Имя работника
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Метод реализует голосовую активность персоны в офисе в ответ на офисные события
        /// </summary>
        /// <param name="d"></param>
        /// <param name="EmployeeName"></param>
        public void Say(object sender, OfficeEventArgs args)
        {
            string sentenceSaid = string.Empty;
            switch (args.Event)
            {
                case OfficeEvent.SomebodyCame:
                    {
                        sentenceSaid = string.Format("'{0}, {1}!' - сказал {2}.", this.Greetings[args.EventTime.Hour], args.Person.Name, this.Name);
                        break;
                    }

                case OfficeEvent.SomebodyGone:
                    {
                        sentenceSaid = string.Format("'{0}, {1}!' - сказал {2}.", Farewell[args.EventTime.Hour], args.Person.Name, this.Name);
                        break;
                    }
            }

            Console.WriteLine(sentenceSaid);
        }
    }
}
