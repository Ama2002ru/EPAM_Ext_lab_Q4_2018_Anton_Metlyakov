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
        private PersonRepository people = new PersonRepository(new SQLConnectorClass("QuizDBTestConection"));

        /// <summary>
        ///  тестируем метод Delete
        /// </summary>
        [Test]
        public void TestDelete()
        {
            people.LoadPersons();
            Assert.That(people.Count == 4);
            Assert.That(people.Delete(1));
            Assert.That(people.Delete(2));
            Assert.That(people.Delete(3));
            Assert.That(people.Delete(4));
            Assert.That(people.Count == 0);
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
            people.LoadPersons();
            foreach (var person in people.GetAll().OrderBy(x => x.ID))
                Assert.That(person.ID == id++);
            Assert.That(id == 5);
        }

        /// <summary>
        ///  тестируем метод Get
        /// </summary>
        [Test]
        public void TestGet()
        {
            // ki. мы должны были бы убедиться, что мы получили именно того пользователя, которого хотели. 
            people.LoadPersons();
            Assert.That(people.Get(1).FirstName == "John");
            Assert.That(people.Get(4).FirstName == "Barak");
        }

        /// <summary>
        ///  тестируем метод Save
        /// </summary>
        [Test]
        public void TestSave()
        {
             Assert.That(people.Save(new Person(id: 1, firstname: "John", lastname: "Doe", username: "jdoe", password: "123", workbook: null, role: RoleEnum.Student)));
             Assert.That(people.Save(new Person(id: 2, firstname: "Igor", lastname: "Kalugin", username: "ki", password: "123", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student)));
             Assert.That(people.Save(new Person(id: 3, firstname: "Nikolay", lastname: "Piskarev", username: "np", password: "123", workbook: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student)));
             Assert.That(people.Save(new Person(id: 4, firstname: "Barak", lastname: "Obama", username: "bo", password: "123", workbook: null, role: RoleEnum.Admin)));
        }
    }
}
