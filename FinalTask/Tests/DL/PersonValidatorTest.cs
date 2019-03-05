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
        /// <summary>
        /// тестируем методы класса
        /// </summary>
        [Test]
        public void TestIsDeleteOK()
        {
            string message;
            var person = new Person(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", salt: "salt", workbook: null, role: RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null);
            Assert.That(PersonValidator.IsValid(person, out message));
        }

        [Test]
        public void TestIsValid()
        {
            string message;
            var person = new Person(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", salt: "salt", workbook: null, role: RoleEnum.Admin, registrationDate: DateTime.Now, lastLogonDate: null);
            Assert.That(PersonValidator.IsDeleteOK(person, out message));
        }
    }
}
