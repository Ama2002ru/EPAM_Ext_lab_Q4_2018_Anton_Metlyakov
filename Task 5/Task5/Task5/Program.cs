namespace Task5
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DAL.Interfaces;
    using DAL.Person;
    using DAL.Utility;

    /// <summary>
    /// Start test program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">usual array</param>
        public static void Main(string[] args)
        {
            Logger.Log = new Log4NetLogger();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Logger.Log.Info("Logging start");
            Console.WriteLine("Welcome to Task5 \"Generics and Collections\" lab.\n");
            var allpeople = new PersonRepository();
            Console.WriteLine("Test person list is :");
            allpeople.ShowAll();
            Console.WriteLine("\nPerson with ID = 2 :");
            var person = allpeople.Get(id: 2);
            person.Show();
            Console.WriteLine("\nAdding new person...");
            allpeople.Add(new PersonClass(4, "Barak", "Obama", "bo", "Bo", null, RoleEnum.Student));
            allpeople.ShowAll();
            Console.WriteLine(string.Empty);
            Console.WriteLine("Deleting person with ID = 2 ...");
            allpeople.Delete(id: 2);
            Console.WriteLine("\nFinal person list :");
            foreach (var p in allpeople.GetAll())
                p.Show();
            Console.ReadKey();
            Logger.Log.Info("Logging end");
        }
    }
}
