namespace Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using DAL;
    using Ninject;

    /// <summary>
    /// Класс для авторизации пользователей
    /// </summary>
    public class QuizRoleProvider : RoleProvider
    {
        public QuizRoleProvider()
        {
        }

        [Inject]
        public IPersonRepository PersonRepo { get; set; }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] roles = new string[] { };

               // Получаем пользователя
                var user = PersonRepo.GetAll().First(x => x.UserName.ToUpper() == username.ToUpper());
                if (user != null)

                    // получаем роль
                    roles = user.Role.ToString().Split(new char[] { ',' });
            for (int i = 0; i < roles.Length; i++)
                roles[i] = roles[i].Trim();
            return roles;
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
                // Получаем пользователя
                var user = PersonRepo.GetAll().First(x => x.UserName.ToUpper() == username.ToUpper());

                if (user != null && user.Role.ToString().ToUpper().Contains(roleName.ToUpper()))
                    return true;
                else
                    return false;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}