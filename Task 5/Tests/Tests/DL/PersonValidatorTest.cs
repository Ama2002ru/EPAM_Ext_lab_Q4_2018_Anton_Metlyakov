namespace Tests.DL
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL.Person;
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
            var person = new PersonClass(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Student);
            Assert.That(PersonValidator.IsValid(person, out message));
        }

        [Test]
        public void TestIsValid()
        {
            string message;
            var person = new PersonClass(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Admin);
            Assert.That(PersonValidator.IsDeleteOK(person, out message));
        }
    }
}
