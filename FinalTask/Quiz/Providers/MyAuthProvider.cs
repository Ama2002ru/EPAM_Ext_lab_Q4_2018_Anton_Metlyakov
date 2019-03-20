namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Web.Security;
    using DAL;

    /// <summary>
    /// Класс для аутентификации пользователей
    /// </summary>
    public class MyAuthProvider : IAuthProvider
    {
    private readonly IPersonRepository personRepository;

    public MyAuthProvider()
    {
    }

    public MyAuthProvider(IPersonRepository personRepo)
    {
        personRepository = personRepo;
    }

    public bool Authenticate(string username, string password)
        {
        bool result = false;

        // 1 Get Person by Username
        // 2 hash password
        // 3 hashes == ? - redirect
        var user = personRepository.GetAll().Where(x => x.UserName.ToUpper() == username.ToUpper());
        if (user.Count() == 0) return result;
        var hashedPassword = PasswordManager.HashPassword(
                    password,
                    user.First().Salt,
                    WebConfigurationManager.AppSettings["quizGlobalSalt"]);
        if (hashedPassword == user.First().HashedPassword)
            result = true;
        if (result)
            FormsAuthentication.SetAuthCookie(user.First().UserName, false);
        return result;
        }
    }
}
