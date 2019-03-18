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
        private RolesRepository roles = new RolesRepository(new SQLConnector("QuizDBTestConection"));

        /// <summary>
        /// тестируем экземпляр класса
        /// </summary>
 
        /// <summary>
        ///  тестируем метод GetAll
        /// </summary>
        [Test]
        public void TestGetAll()
        {
            roles.GetAll();
            Assert.That(roles.Count == 3);
            Assert.That(roles.Get(id: 1).Name == "Student" && roles.Get(id: 1).RoleFlag == RoleEnum.Student);
            Assert.That(roles.Get(id: 2).Name == "Instructor" && roles.Get(id: 2).RoleFlag == RoleEnum.Instructor);
            Assert.That(roles.Get(id: 3).Name == "Admin" && roles.Get(id: 3).RoleFlag == RoleEnum.Admin);
        }

        [Test]
        public void TestCheckIsAllowedMethod()
        {
            roles.GetAll();
            var person = new Person(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", salt: "salt", quizResults: null, role: RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null);
            Assert.That(roles.CheckIsAllowed(person, "QuizClass.Show"));
            Assert.That(!roles.CheckIsAllowed(person, "PersonRepository.Add"));
        }
    }
}
