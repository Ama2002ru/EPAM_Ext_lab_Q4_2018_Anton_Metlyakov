namespace Task6
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Возможные события в офисе
    /// </summary>
    public enum OfficeEvent
    {
        /// <summary>
        /// кто-то пришел
        /// </summary>
        SomebodyCame = 1,

        /// <summary>
        /// кто-то ушел
        /// </summary>
        SomebodyGone = 2
    }

    public class OfficeClass
    {
        /// <summary>
        ///  Список всех работников офиса
        /// </summary>
        private List<IPerson> staff = new List<IPerson>
        {
            new Employee("Bill"),
            new Employee("John"),
            new Employee("Margo"),
            new Manager("Helene"),
            new Employee("Alex"),
            new Employee("Bob")
        };

        // добавляю анонимный делегат чтобы не проверять на null (в интернетах такое пишут).
        // Игорь, так можно делать ?
        // ki.можно, но смысла особого в этом не вижу. Написать ?. - это не долго.
        public event EventHandler<OfficeEventArgs> PeopleAtOffce = delegate { };
        
        // ki. Как-то очень заморочено. Я бы сделал проще - у каждого человека есть метода SayHello и
        // SayGoodbay - это будущие обработчики событий прихода, ухода
        //Есть лист работников PeopleInOffice. При приходе нового работника мы добавляем его в лист
        //а перед этим проходим по всему списку сотрудников в peopleInOffice и у каждого из них
        // подписываюсь на событие прихода в офис добавляемого человека/
        // примерно так же с их уходом из офиса
        // а то у тебя тут обработка событий как-то постфактум сделана.
         /// <summary>
        ///  главный метод - ускоренно пробегу по часам/минутам в сутках 
        /// </summary>
        public void SimulateWorkDay() 
        {
// Готовим реквизит
// генерирую 1. список событий 
//           2. время прихода сотрудников
//           3. полагаю что трудоголиков среди них нет, и уход - ровно через 8 часов от прихода
            var todayTime = new DateTime(2018, 12, 24, 9, 0, 0);
            var rnd = new Random();
            var events = new List<OfficeEventArgs>(staff.Count * 2);

            foreach (var employee in staff)
            {
                // жду следующего человека в течении 60 минут
                todayTime = todayTime.Add(new TimeSpan(0, rnd.Next(60), 0));  
                events.Add(new OfficeEventArgs() { Event = OfficeEvent.SomebodyCame, Person = employee, EventTime = todayTime });

                // уход - через 8 часов после прихода
                events.Add(new OfficeEventArgs() { Event = OfficeEvent.SomebodyGone, Person = employee, EventTime = todayTime.AddHours(8) });
            }

            // отсортирую список по времени события 
            events.Sort(delegate(OfficeEventArgs args1, OfficeEventArgs args2)
                {
                    return args1.EventTime.CompareTo(args2.EventTime);
                });

            foreach (var officeEvent in events)
            {
                // Обработка события - немного хардкода :(
                switch (officeEvent.Event)
                {
                    case OfficeEvent.SomebodyCame:
                        {
                            Console.WriteLine("[На работу пришел {0}]", officeEvent.Person.Name);
                            PeopleAtOffce.Invoke(this, officeEvent);
                            PeopleAtOffce += officeEvent.Person.Say;
                            break;
                        }

                    case OfficeEvent.SomebodyGone:
                        {
                            Console.WriteLine("[{0} ушел домой]", officeEvent.Person.Name);
                            PeopleAtOffce -= officeEvent.Person.Say;
                            PeopleAtOffce.Invoke(this, officeEvent);
                            break;
                        }
                }

                Thread.Sleep(400);
            }
        }
    }
}
