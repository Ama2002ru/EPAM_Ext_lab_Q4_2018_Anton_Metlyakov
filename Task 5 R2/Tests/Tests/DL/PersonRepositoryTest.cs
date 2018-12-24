namespace Tests.DL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DAL.Person;
    using NUnit.Framework;

    /// <summary>
    /// тестируем методы класса PersonRepository Add Delete Get GetAll Save 
    /// </summary>
    public class PersonRepositoryTest
    {
        /// <summary>
        ///  тестируем метод Delete
        /// </summary>
        [Test]
        public void TestDelete()
        {
            var people = new PersonRepository();
            people.Init();
            Assert.That(people.GetAll().Count == 4);
            Assert.That(people.Delete(0));
            Assert.That(people.Delete(1));
            Assert.That(people.Delete(2));
            Assert.That(people.Delete(3));
            Assert.That(people.GetAll().Count == 0);
        }

        /// <summary>
        ///  тестируем метод GetAll
        /// </summary>
        [Test]
        public void TestGetAll()
        {
            var people = new PersonRepository();
            foreach (var person in people.GetAll())
                Assert.That(person != null);
        }

        /// <summary>
        ///  тестируем метод Get
        /// </summary>
        [Test]
        public void TestGet()
        {
            var people = new PersonRepository();
            people.Init();
            var person = people.Get(1);
            Assert.That(person != null);
        }

        /// <summary>
        ///  тестируем метод Save
        /// </summary>
        [Test]
        public void TestSave()
        {
            var people = new PersonRepository();
            var person = new PersonClass(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Student);
            Assert.That(people.Save(person));
        }
    }
}
