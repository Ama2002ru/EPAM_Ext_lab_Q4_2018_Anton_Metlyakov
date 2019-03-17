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
            var person = new Person(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Student);
            Assert.That(!person.IsAssignedRole(RoleEnum.Admin));
            Assert.That(!person.IsAssignedRole(RoleEnum.Instructor));
            Assert.That(person.IsAssignedRole(RoleEnum.Student));
        }

        /// <summary>
        ///  тестируем метод Save
        /// </summary>
        // ki. все же тест должен сначала сохранить значение, а потом попробовать его считать и сверить то, что модель сохраняемая ровна модели считанной. 
        [Test]
        public void TestSave()
        {
            var person = new Person(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Student);
            Assert.That(person.Save(new SQLConnectorClass("QuizDBTestConection")));
        }

        /// <summary>
        ///  тестируем метод Delete
        /// </summary>
        // ki. тут надо проверить, что удаленных данных в БД нет. 
        [Test]
        public void TestDelete()
        {
            var person = new Person(id: 1, firstname: "Anton", lastname: "Metlyakov", username: "am", password: "am", workbook: null, role: RoleEnum.Student);
            Assert.That(person.Delete());
        }
    }
}