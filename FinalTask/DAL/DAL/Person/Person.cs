namespace DAL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Text;
    using DAL;
    using log4net;
    using static DALResources;

    /// <summary>
    /// Класс, описывающий пользователя системы
    /// </summary>
    public class Person 
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        public Person()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
          }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public Person(int id, string firstname, string lastname, string username, string password, string salt, List<QuizResult> quizResults, RoleEnum role, DateTime registrationDate, DateTime? lastLogonDate)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = id;
            FirstName = firstname;
            LastName = lastname;
            UserName = username;
            HashedPassword = password;
            Salt = salt;
            QuizResults = quizResults;
            Role = role;
            RegistrationDate = registrationDate;
            LastLogonDate = lastLogonDate;
        }

        /// <summary>
        /// ID пользователя. В норме равен ID в БД. У ново-добавленного пользователя ожидаю ID = -1
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Логин в системе. Буду отслеживать его уникальность при добавлении/сохранении пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия (?) пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// все назначенные (и возможно уже выполненные) тесты
        /// </summary>
        public List<QuizResult> QuizResults { get; set; }

        /// <summary>
        /// назначенные роли пользователя
        /// </summary>
        public RoleEnum Role { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? LastLogonDate { get; set; }

        /// <summary>
        /// пароль пользователя
        /// </summary>
        public string HashedPassword { get; set; }

        /// <summary>
        /// Соль для пароля пользователя
        /// </summary>
        public string Salt { get; set; }

       /// <summary>
        /// Проверка наличия роли у пользователя - Student, Admin и т.д.
        /// </summary>
        public bool IsAssignedRole(RoleEnum testrole)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            return (Role & testrole) != 0;
        }

        /// <summary>
        /// Сохранение изменений текущего/добавление нового пользователя в БД
        /// </summary>
        /// <returns></returns>
        public bool Save(IdbConnector db)
        {
            bool saveResult = false;
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            try
            {
                using (var dbconn = db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    var logonDate = string.Empty;
                    if (LastLogonDate.HasValue)
                        logonDate = string.Format("{0:s}", LastLogonDate.Value);
                    /*   эту команду угнал в файл ресурсов под именем P_SaveUser
                                        EXECUTE[dbo].[P_SAVEUSER] @USERID=@Uid, @USERNAME=@Unm, @FIRSTNAME=@fn, @LASTNAME=@ln,@HASHEDPASSWORD=@hp,@SALT=@st,
                                        @ROLESFLAG=@rf, @LastLogonDate=@lld, @ERROR=@er OUT, @ERRORTEXT=@et OUT
                    */
                    command.CommandText = P_SaveUser;
                    command.Parameters.Add(db.CreateParameter("@Uid", DbType.Int32, ID.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@Unm", DbType.String, UserName, 100, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@fn", DbType.String, FirstName, 100, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@ln", DbType.String, LastName, 100, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@hp", DbType.String, HashedPassword, 100, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@st", DbType.String, Salt, 100, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@rf", DbType.Int32, ((int)Role).ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@lld", DbType.String, logonDate, logonDate.Length, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                    command.Parameters.Add(db.CreateParameter("@et", DbType.String, null, 100, ParameterDirection.Output));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();
                    var saveError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                    var saveErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                    if (saveError == 0) saveResult = true;
                    //// проверю, что действительно что-то возвращается
                    Logger.Debug(string.Format("P_SaveUser out : {0} {1}\n", saveError.ToString(), saveErrorText));
                }
            }
            catch (DbException ex)
            {
                Logger.Info(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return saveResult;
        }
    }
}