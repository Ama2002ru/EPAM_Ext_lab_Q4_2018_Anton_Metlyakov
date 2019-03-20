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
    /// Class UserModel для управления пользователями
    /// </summary>
    public class UserModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [Required(ErrorMessage = "Unique username is required!")]
        [Display(Name = "Unique username")]
        public string UserName { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [Required]
        [Display(Name = "User firstname")]
        public string FirstName { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [Required]
        [Display(Name = "User lastname")]
        public string LastName { get; set; }

        // Атрибут Required ставить не буду - своя логика проверки наличия пароля
        [DataType(DataType.Password)]
        [Display(Name = "User password")]
        public string Password { get; set; }

        public string UserSalt { get; set; }

        [Display(Name = "Application roles :")]
        [Required(ErrorMessage = "Please select one")]
        public RoleEnum Roles { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? LastLogonDate { get; set; }

        public static implicit operator UserModel(Person v)
        {
            /* неужели так копировать все поля ??? */
            if (v == null) return null;
            var user = new UserModel
            {
                Id = v.ID,
                UserName = v.UserName,
                LastName = v.LastName,
                FirstName = v.FirstName,
                RegistrationDate = v.RegistrationDate,
                LastLogonDate = v.LastLogonDate,
                Password = null,
                UserSalt = v.Salt,
                Roles = v.Role
            };
            return user;
        }
    }
}