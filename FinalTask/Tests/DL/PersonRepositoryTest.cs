namespace Tests.DL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DAL;
    using NUnit.Framework;

    /// <summary>
    /// тестируем методы класса PersonRepository Delete Get GetAll Save 
    /// </summary>
    [TestFixture]
    public class PersonRepositoryTest
    {
        private PersonRepository people = new PersonRepository(new SQLConnector("QuizDBTestConection"));

        /// <summary>
        ///  тестируем метод Delete
        /// </summary>
        [Test]
        public void TestDelete()
        {
            people.Save(new Person(id: -1, firstname: "John", lastname: "Doe", username: "jdoe", password: "123", salt: "salt", workbook: null, role: RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null));
            people.Save(new Person(id: -1, firstname: "Igor", lastname: "Kalugin", username: "ki", password: "123", salt: "salt", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null));
            people.Save(new Person(id: -1, firstname: "Nikolay", lastname: "Piskarev", username: "np", password: "123", salt: "salt", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null));
            people.Save(new Person(id: -1, firstname: "Barak", lastname: "Obama", username: "bo", password: "123", salt: "salt", workbook: null, role: RoleEnum.Admin, registrationDate: DateTime.Now, lastLogonDate: null));

            // удалю всё что навставлял...
            Assert.That(people.Delete(people.GetAll().Where(x => x.UserName == "jdoe").First().ID));
            Assert.That(people.Delete(people.GetAll().Where(x => x.UserName == "ki").First().ID));
            Assert.That(people.Delete(people.GetAll().Where(x => x.UserName == "np").First().ID));
            Assert.That(people.Delete(people.GetAll().Where(x => x.UserName == "bo").First().ID));
            Assert.That(people.GetAll().Where(x => x.UserName == "bo").Count() == 0);
        }

        /// <summary>
        ///  тестируем метод GetAll
        /// </summary>
        [Test]
        public void TestGetAll()
        {
            // ki. скорее суть теста должна быть не в этом, а в том, что мы сначала подгототавливаем внутри теста список пользователей
            // а потом проверяем, что мы получили именно ожидаемых пользователей
            int id = 1;
            foreach (var person in people.GetAll().OrderBy(x => x.ID))

        // проверю наличие 3х встроенных учеток
                Assert.That(person.ID == id++);
            Assert.That(id == 4);
        }

        /// <summary>
        ///  тестируем метод Get
        /// </summary>
        [Test]
        public void TestGet()
        {
            // ki. мы должны были бы убедиться, что мы получили именно того пользователя, которого хотели. 
            people.GetAll();
            Assert.That(people.Get(1).FirstName == "Student");
            Assert.That(people.Get(2).FirstName == "Instructor");
            Assert.That(people.Get(3).FirstName == "Admin");
        }

        /// <summary>
        ///  тестируем метод Save
        /// </summary>
        [Test]
        public void TestSave()
        {
             Assert.That(people.Save(new Person(id: -1, firstname: "John", lastname: "Doe", username: "jdoe", password: "123", salt: "salt", workbook: null, role: RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null)));
             Assert.That(people.Save(new Person(id: -1, firstname: "Igor", lastname: "Kalugin", username: "ki", password: "123", salt: "salt", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null)));
             Assert.That(people.Save(new Person(id: -1, firstname: "Nikolay", lastname: "Piskarev", username: "np", password: "123", salt: "salt", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null)));
             Assert.That(people.Save(new Person(id: -1, firstname: "Barak", lastname: "Obama", username: "bo", password: "123", salt: "salt", workbook: null, role: RoleEnum.Admin, registrationDate: DateTime.Now, lastLogonDate: null)));
             Assert.That(!people.Save(new Person(id: -1, firstname: "John", lastname: "Doe", username: "jdoe", password: "123", salt: "salt", workbook: null, role: RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null)));

            // удалю всё что навставлял...
//            people.GetAll();
            people.Delete(people.GetAll().Where(x => x.UserName == "jdoe").First().ID);
            people.Delete(people.GetAll().Where(x => x.UserName == "ki").First().ID);
            people.Delete(people.GetAll().Where(x => x.UserName == "np").First().ID);
            people.Delete(people.GetAll().Where(x => x.UserName == "bo").First().ID);
        }
    }
}
