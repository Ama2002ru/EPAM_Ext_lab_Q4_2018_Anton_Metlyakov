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
            people.Save(new Person(id: -1, firstname: "John", lastname: "Doe", username: "jdoe", password: "123", salt: "salt", quizResults: null, role: RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null));
            people.Save(new Person(id: -1, firstname: "Igor", lastname: "Kalugin", username: "ki", password: "123", salt: "salt", quizResults: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null));
            people.Save(new Person(id: -1, firstname: "Nikolay", lastname: "Piskarev", username: "np", password: "123", salt: "salt", quizResults: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null));
            people.Save(new Person(id: -1, firstname: "Barak", lastname: "Obama", username: "bo", password: "123", salt: "salt", quizResults: null, role: RoleEnum.Admin, registrationDate: DateTime.Now, lastLogonDate: null));

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
            string name = string.Empty;
            foreach (var person in people.GetAll().OrderBy(x => x.ID))
            {
                switch (person.ID)
                {
                    case 1:
                        {
                            name = "Student";
                            break;
                        }

                    case 2:
                        {
                            name = "Instructor";
                            break;
                        }

                    case 3:
                        {
                            name = "Admin";
                            break;
                        }

                    case 6:
                        {
                            name = "gw";
                            break;
                        }

                    case 7:
                        {
                            name = "ja";
                            break;
                        }

                    case 9:
                        {
                            name = "tj";
                            break;
                        }

                    case 10:
                        {
                            name = "jm";
                            break;
                        }

                    case 11:
                        {
                            name = "jmonroe";
                            break;
                        }
                }

                // проверю наличие 3х встроенных учеток
                Assert.That(person.UserName == name);
            }
        }

        /// <summary>
        ///  тестируем метод Get
        /// </summary>
        [Test]
        public void TestGet()
        {
            // ki. мы должны были бы убедиться, что мы получили именно того пользователя, которого хотели. 
            //           people.GetAll();
            var student_id = people.GetAll().First(x => x.UserName == "Student").ID;
            Assert.That(people.Get(student_id).FirstName == "Student");
            var instructor_id = people.GetAll().First(x => x.UserName == "Instructor").ID;
            Assert.That(people.Get(instructor_id).FirstName == "Instructor");
            var admin_id = people.GetAll().First(x => x.UserName == "Admin").ID;
            Assert.That(people.Get(admin_id).FirstName == "Admin");
        }

        /// <summary>
        ///  тестируем метод Save
        /// </summary>
        [Test]
        public void TestSave()
        {
            int count = people.GetAll().Count;
             Assert.That(people.Save(new Person(id: -1, firstname: "John", lastname: "Doe", username: "jdoe", password: "123", salt: "salt", quizResults: null, role: RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null)));
             Assert.That(people.Save(new Person(id: -1, firstname: "Igor", lastname: "Kalugin", username: "ki", password: "123", salt: "salt", quizResults: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null)));
             Assert.That(people.Save(new Person(id: -1, firstname: "Nikolay", lastname: "Piskarev", username: "np", password: "123", salt: "salt", quizResults: null, role: RoleEnum.Admin | RoleEnum.Instructor | RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null)));
             Assert.That(people.Save(new Person(id: -1, firstname: "Barak", lastname: "Obama", username: "bo", password: "123", salt: "salt", quizResults: null, role: RoleEnum.Admin, registrationDate: DateTime.Now, lastLogonDate: null)));
             Assert.That(!people.Save(new Person(id: -1, firstname: "John", lastname: "Doe", username: "jdoe", password: "123", salt: "salt", quizResults: null, role: RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null)));

            // проверю что в БД стало больше персон
            Assert.That(people.GetAll().Count == count + 4);

            // удалю всё что навставлял...
            people.Delete(people.GetAll().Where(x => x.UserName == "jdoe").First().ID);
            people.Delete(people.GetAll().Where(x => x.UserName == "ki").First().ID);
            people.Delete(people.GetAll().Where(x => x.UserName == "np").First().ID);
            people.Delete(people.GetAll().Where(x => x.UserName == "bo").First().ID);
        }
    }
}
