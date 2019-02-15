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
    public class Person : IItem
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        public Person()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = 0;
            FirstName = "John";
            LastName = "Doe";
            Username = "JDoe";
            HashedPassword = "123";
            WorkBook = null;
            Role = RoleEnum.None;
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public Person(int id, string firstname, string lastname, string username, string password, WorkBookClass workbook, RoleEnum role)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            ID = id;
            FirstName = firstname;
            LastName = lastname;
            Username = username;
            HashedPassword = password;
            WorkBook = workbook;
            Role = role;
        }

        /// <summary>
        /// ID пользователя. В норме равен ID в БД. У ново-добавленного пользователя ожидаю ID = -1
        /// </summary>
        public int ID { get; set;  }

        /// <summary>
        /// Логин в системе. Буду отслеживать его уникальность при добавлении/сохранении пользователя
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// имя пользователя
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Фамилия (?) пользователя
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// все назначенные (и возможно уже выполненные) тесты
        /// </summary>
        public WorkBookClass WorkBook { get; }

        /// <summary>
        /// назначенные роли пользователя
        /// </summary>
        public RoleEnum Role { get; }

        /// <summary>
        /// пароль пользователя
        /// </summary>
        private string HashedPassword { get; set; }

        /// <summary>
        /// Удаление текущего пользователя в БД
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));

            // there will be ADO.NET implementation;
            return true;
        }

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
            string currentUserName = "anonymous user";
            Logger.Debug(string.Format("{0}.{1} start, committed by {2} ", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, currentUserName));
            try
            {
                using (var dbconn = db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    /*   эту команду угнал в файл ресурсов под именем P_SaveUser
                                        EXECUTE[dbo].[P_SAVEUSER] @USERID=@Uid, @USERNAME=@Unm, @FIRSTNAME=@fn, @LASTNAME=@ln,@HASHEDPASSWORD=@hp,
                                        @ROLESFLAG=@rf,  @ERROR=@er OUT, @ERRORTEXT=@et OUT

                    */
                    command.CommandText = P_SaveUser;
                    command.Parameters.Add(db.CreateParameter("@Uid", DbType.Int32, ID.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@Unm", DbType.String, Username, 100, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@fn", DbType.String, FirstName, 100, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@ln", DbType.String, LastName, 100, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@hp", DbType.String, HashedPassword, 100, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@rf", DbType.Int32, ((int)Role).ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                    var saveTextError = command.Parameters.Add(db.CreateParameter("@et", DbType.String, null, 100, ParameterDirection.Output));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();

                    /* это действительно всё так сложно надо преобразовывать ? */

                    var saveError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                    var saveErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                    //// проверю, что действительно что-то возвращается
                    Logger.Info(string.Format("P_SaveUser out : {0} {1}\n", saveError.ToString(), saveErrorText));
                }
            }
            catch (DbException ex)
            {
                Logger.Info(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return true;
        }
        
        /// <summary>
        /// Вывести инфо о  пользователе
        /// </summary>
        /// <returns></returns>
        public void Show()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
 ///           Console.WriteLine("{0}, {1}, {2}, {3}, {4}", ID, FirstName, LastName, Username, Role.ToString());
        }
    }
}