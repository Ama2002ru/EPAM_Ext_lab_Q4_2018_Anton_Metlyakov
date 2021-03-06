﻿namespace Tests.DL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DAL;
    using NUnit.Framework;

    /// <summary>
    /// тестируем методы класса Person
    /// </summary>
    public class PersonMethodTest
    {
        /// <summary>
        /// тестируем метод IsAssignedRole
        /// </summary>
        [Test]
        public void TestIsAssignedRole()
        {
            var person = new Person(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", salt: "salt", quizResults: null, role: RoleEnum.Student, registrationDate: DateTime.Now, lastLogonDate: null);
            Assert.That(!person.IsAssignedRole(RoleEnum.Admin));
            Assert.That(!person.IsAssignedRole(RoleEnum.Instructor));
            Assert.That(person.IsAssignedRole(RoleEnum.Student));
        }
    }
}