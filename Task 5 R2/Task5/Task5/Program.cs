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
        /// <param name="args">usual args array</param>
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Logger.Log = new Log4NetLogger();
            var messenger = new OutputClass();

            Logger.Info("Logging start");
            messenger.WriteLine("Welcome to Task5 \"Generics and Collections\" lab.\n");
            var allpeople = new PersonRepository();

            // добавлю тестовую группу 
            allpeople.Init();

            messenger.WriteLine("Test person list is :");
            foreach (var p in allpeople.GetAll())
                messenger.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}", p.ID, p.FirstName, p.LastName, p.Username, p.Role.ToString()));
            messenger.WriteLine("\nPerson with ID = 2 :");
            var person = allpeople.Get(id: 2);
            if (person != null)
                messenger.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}", person.ID, person.FirstName, person.LastName, person.Username, person.Role.ToString()));

            messenger.Write("\nAdding new person ... ");
            var obama = new PersonClass(4, "Barak", "Obama", "bo", "Bo", null, RoleEnum.Student);
            if (PersonValidator.IsValid(obama, out string errormessage))
            if (allpeople.Save(obama))
                messenger.WriteLine("Ok");
            else
                messenger.WriteLine("Save failed!");

            foreach (var p in allpeople.GetAll())
                messenger.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}", p.ID, p.FirstName, p.LastName, p.Username, p.Role.ToString()));

            messenger.Write("\nDeleting person with ID = 2 ... ");
            var idPersonToDelete = 2;
            if (PersonValidator.IsDeleteOK(allpeople.Get(idPersonToDelete), out errormessage))
                if (allpeople.Delete(idPersonToDelete))
                    messenger.WriteLine("Ok");
                else
                    messenger.WriteLine("Delete failed!");

            messenger.WriteLine("\nFinal person list :");
            foreach (var p in allpeople.GetAll())
                messenger.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}", p.ID, p.FirstName, p.LastName, p.Username, p.Role.ToString()));
            Logger.Info("Logging end");
        }
    }
}
