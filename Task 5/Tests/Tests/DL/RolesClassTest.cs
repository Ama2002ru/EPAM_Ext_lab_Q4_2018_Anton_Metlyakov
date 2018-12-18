namespace Tests.DL
{   
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL.Person;
    using NUnit.Framework;

    [TestFixture]
    public class RolesClassTest
    {
        /// <summary>
        /// тестируем экземпляр класса
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            var roles = RolesClass.Init();
            Assert.That(roles[0].Name.Contains("No access"));
            Assert.That(roles[0].AllowedMethods.Contains("Test.Test"));
            Assert.That(roles.Count == 4);
        }

        [Test]
        public void TestCheckIsAllowedMethod()
        {
            var person = new PersonClass(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Student);
            Assert.That(RolesClass.CheckIsAllowed(person, "QuizClass.Show"));
            Assert.That(!RolesClass.CheckIsAllowed(person, "PersonRepository.Add"));
        }
    }
}
