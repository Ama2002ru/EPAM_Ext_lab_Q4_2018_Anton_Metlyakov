namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    
    // класс для работы с паролями
    // Алгоритм работы :
    // 1. Проверка пароля с логин-формы
    //     - запросить для юзернейма из БД хэш и соль
    //     - запросить из конфига глобальную соль
    //     - посчитать хеш введенного пароля с солями
    //     - сравнить хеши
    // 2. Создание данных пользователя    
    //     - сгенерировать соль для пользователя
    //     - сгенерировать хеш для введенного пароля
    //     - сохранить пользователя
    public static class PasswordManager
    {
        /// <summary>
        /// зашифровать пароль пользователя с персональной и глобальной солью
        /// смотри  WebConfigurationManager.AppSettings["globalSalt"];
        /// </summary>
        public static string HashPassword(string password, string saltBase64, string globalSaltBase64)
        {
            byte[] hashedPassword;
            var byte_password = System.Text.Encoding.Unicode.GetBytes(password);
        
            // создаю единый массив для расчета хеша
            int data_size = byte_password.Length +
                            Convert.FromBase64String(saltBase64).Length +
                            Convert.FromBase64String(globalSaltBase64).Length;
            byte[] data = new byte[data_size];
            byte_password.CopyTo(data, 0);
            Convert.FromBase64String(saltBase64).CopyTo(data, byte_password.Length);
            Convert.FromBase64String(globalSaltBase64).CopyTo(data, byte_password.Length + Convert.FromBase64String(saltBase64).Length);

            // считаем хеш
            SHA512 shaM = new SHA512Managed();
            hashedPassword = shaM.ComputeHash(data);
            return Convert.ToBase64String(hashedPassword);
        }

        /// <summary>
        ///  Создать соль 
        /// </summary>
        /// <param name="saltSize"></param>
        /// <returns></returns>
        public static string GenerateSalt(int saltSize = 8)
        {
            byte[] salt = new byte[saltSize];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())

                // Fill the array with a random value.
                rngCsp.GetBytes(salt);
            return Convert.ToBase64String(salt, Base64FormattingOptions.None);
        }

        /// <summary>
        /// проверить пароль на соответствие политикам
        /// </summary>
        public static bool ValidatePassword(string userPassword)
        {
            return false;
        }
    }
}
