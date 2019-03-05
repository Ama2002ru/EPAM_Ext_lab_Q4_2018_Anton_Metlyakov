namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using DAL;
    using Quiz.Resources;

    /// <summary>
    /// Class UserModel for user management
    /// </summary>
    public class UserModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [Required(ErrorMessage = "Имя пользователя обязательно!")]
        public string UserName { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string LastName { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string UserSalt { get; set; }

        public RoleEnum Roles { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? LastLogonDate { get; set; }

        public static implicit operator UserModel(Person v)
        {
            /* неужели так копировать все поля ??? */

            if (v == null) return null;
            var user = new UserModel();
            user.Id = v.ID;
            user.UserName = v.UserName;
            user.LastName = v.LastName;
            user.FirstName = v.FirstName;
            user.RegistrationDate = v.RegistrationDate;
            user.LastLogonDate = v.LastLogonDate;
            user.Password = null;
            user.UserSalt = v.Salt;
            user.Roles = v.Role;
            return user;
        }
    }
}