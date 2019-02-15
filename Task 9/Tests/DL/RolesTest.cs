namespace Tests.DL
{   
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DAL;
    using NUnit.Framework;

    [TestFixture]
    public class RolesTest
    {
        /// <summary>
        /// тестируем экземпляр класса
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            var db = new SQLConnectorClass("QuizDBTestConection");

            var roles = Roles.Init();
            Assert.That(roles[0].Name.Contains("No access"));
            Assert.That(roles[0].AllowedMethods.Contains("Test.Test"));
            Assert.That(roles.Count == 4);
        }

        [Test]
        public void TestCheckIsAllowedMethod()
        {
            var person = new Person(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Student);
            Assert.That(Roles.CheckIsAllowed(person, "QuizClass.Show"));
            Assert.That(!Roles.CheckIsAllowed(person, "PersonRepository.Add"));
        }
    }
}
