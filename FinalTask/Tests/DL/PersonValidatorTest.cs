namespace Tests.DL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL;
    using NUnit.Framework;

    [TestFixture]
    public class PersonValidatorTest
    {
        private static PersonRepository personRepository = new PersonRepository(new SQLConnector("QuizDBTestConection"));
        private static PersonValidator personValidator = new PersonValidator(personRepository);

        /// <summary>
        /// тестируем методы класса
        /// </summary>
        [Test]
        public void TestIsDeleteOK()
        {
            string message;
            var person = new Person(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", salt: "salt", quizResults: null, role: RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null);
            Assert.That(personValidator.IsValid(person, out message));
        }

        [Test]
        public void TestIsValid()
        {
            string message;
            var person = new Person(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", salt: "salt", quizResults: null, role: RoleEnum.Admin, registrationDate: DateTime.Now, lastLogonDate: null);
            Assert.That(personValidator.IsDeleteOK(person, out message));
        }
    }
}
