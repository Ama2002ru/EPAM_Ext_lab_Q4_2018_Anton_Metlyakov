namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;
    using DAL;
    using log4net;
    using static DALResources;

    /// <summary>
    /// Класс содержит методы, обращающиеся в БД за статистикой квизов
    /// </summary>
    public static class Statistic
    {
        /// <summary>
        /// Получить из БД отчет №1
        /// </summary>
        /// <returns></returns>
        public static List<StatsAllQuizes> AllQuizes(IQuizRepository quizRepository)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            var allQuizesList = new List<StatsAllQuizes>(0);
            try
            {
                using (var dbconn = quizRepository.Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = P_StatsAllQuizes;  // see DALResources.resx,
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var quizes = command.ExecuteReader())
                    {
                        while (quizes.Read())
                        {
                            float percent_Passed;
                            float.TryParse(quizes[6].ToString(), out percent_Passed);
                            float success_Rate;
                            float.TryParse(quizes[1].ToString(), out success_Rate);
                            allQuizesList.Add(new StatsAllQuizes()
                            {
                                Quiz_Name = quizes[0].ToString(),
                                Success_Rate = success_Rate,
                                Total_Assigned = (int)quizes[2],
                                Total_Passed = (int)quizes[3],
                                Total_Failed = (int)quizes[4],
                                Average_Rate = quizes[5].ToString(),
                                Percent_Passed = percent_Passed
                            });
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                allQuizesList = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return allQuizesList;
        }

        /// <summary>
        /// Получить из БД отчет №2
        /// </summary>
        /// <returns></returns>
        public static List<StatsAllUsers> AllUsers(IQuizRepository quizRepository)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            var allUsersList = new List<StatsAllUsers>(0);
            try
            {
                using (var dbconn = quizRepository.Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = P_StatsAllUsers;  // see DALResources.resx,
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var quizes = command.ExecuteReader())
                    {
                        while (quizes.Read())
                        {
                            float percent_Passed;
                            float.TryParse(quizes[5].ToString(), out percent_Passed);
                            allUsersList.Add(new StatsAllUsers()
                            {
                                User_Name = quizes[0].ToString(),
                                Total_Assigned = (int)quizes[1],
                                Total_Passed = (int)quizes[2],
                                Total_Failed = (int)quizes[3],
                                Average_Rate = quizes[4].ToString(),
                                Percent_Passed = percent_Passed
                            });
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                allUsersList = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return allUsersList;
        }

        /// <summary>
        /// Получить из БД отчет №3
        /// </summary>
        /// <returns></returns>
        public static List<StatsByQuiz> ByQuiz(IQuizRepository quizRepository, int quiz_id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            var quizList = new List<StatsByQuiz>(0);
            try
            {
                using (var dbconn = quizRepository.Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = P_StatsByQuiz;  // see DALResources.resx,
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qid", DbType.Int32, quiz_id.ToString(), null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var quizes = command.ExecuteReader())
                    {
                        while (quizes.Read())
                        {
                            quizList.Add(new StatsByQuiz()
                            {
                                User_Id = (int)quizes[0],
                                User_Name = quizes[1].ToString(),
                                Quiz_Status = quizes[2].ToString(),
                                Completed_Rate = quizes[3].ToString(),
                                Completed_Date = quizes[4].ToString(),
                                Started_Date = quizes[5].ToString(),
                                Time_Taken = quizes[6].ToString()
                            });
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                quizList = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return quizList;
        }

        /// <summary>
        /// Получить из БД отчет №4
        /// </summary>
        /// <returns></returns>
        public static List<StatsByUser> ByUser(IQuizRepository quizRepository, int user_id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            var userList = new List<StatsByUser>(0);
            try
            {
                using (var dbconn = quizRepository.Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = P_StatsByUser;  // see DALResources.resx,
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@uid", DbType.Int32, user_id.ToString(), null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var quizes = command.ExecuteReader())
                    {
                        while (quizes.Read())
                        {
                            userList.Add(new StatsByUser()
                            {
                                Quiz_Id = (int)quizes[0],
                                Quiz_Name = quizes[1].ToString(),
                                Quiz_Status = quizes[2].ToString(),
                                Completed_Rate = quizes[3].ToString(),
                                Completed_Date = quizes[4].ToString(),
                                Started_Date = quizes[5].ToString(),
                                Time_Taken = quizes[6].ToString()
                            });
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                userList = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return userList;
        }

        /// <summary>
        /// Получить из БД отчет №5
        /// </summary>
        /// <returns></returns>
        public static List<StatsByUserQuiz> ByUserQuiz(IQuizRepository quizRepository, int user_id, int quiz_id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            var userquizList = new List<StatsByUserQuiz>(0);
            try
            {
                using (var dbconn = quizRepository.Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = P_StatsByUserQuiz;  // see DALResources.resx,
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@uid", DbType.Int32, user_id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qid", DbType.Int32, quiz_id.ToString(), null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var quizes = command.ExecuteReader())
                    {
                        while (quizes.Read())
                        {
                            userquizList.Add(new StatsByUserQuiz()
                            {
                                Info = quizes[0].ToString(),
                                Text = quizes[1].ToString(),
                                Result = quizes[2].ToString()
                            });
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                userquizList = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return userquizList;
        }
    }
}
