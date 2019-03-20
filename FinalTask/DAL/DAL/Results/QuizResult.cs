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
    using static DALResources;

    /// <summary>
    /// Класс хранит результат прохождения теста, 
    /// в том числе и назначенного (ещё не пройденного) теста
    /// думаю что не буду связывать с QuizCollection по ID теста
    /// </summary>
    public class QuizResult
    {
        private IQuizRepository quizRepository;

        public QuizResult()
        {
            Answer_List = new List<Answer>(0);
        }

        public QuizResult(IQuizRepository repo)
        {
            quizRepository = repo;
        }

        /// <summary>
        /// Для работы с БД
        /// </summary>
        public int QuizResult_Id { get; set; }

        /// <summary>
        /// собственно ID студента. Может быть и не надо?
        /// </summary>
        public int User_Id { get; set; }

        /// <summary>
        /// собственно имя студента.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Для работы с БД
        /// </summary>
        public int Quiz_Id { get; set; }

        public string Quiz_Name { get; set; }
        
        /// <summary>
        /// Person, назначивший тест студенту
        /// </summary>
        public string AssignedBy { get; set; }

        public DateTime? Assigned_Date { get; set; }

        public DateTime? Completed_Date { get; set; }

        /// <summary>
        /// сохраню состояние теста - назначен/пройден/провален ...
        /// </summary>
        public QuizStatusEnum QuizResult_Status { get; set; }

        /// <summary>
        /// Рейт, полученный в результате прохождения теста
        /// </summary>
        public float? Completed_Rate { get; set; }

        public DateTime? Started_Date { get; set; }

        public TimeSpan? Time_Taken { get; set; }

        /// <summary>
        /// список/массив ? ответов на каждый вопрос теста
        /// </summary>
        public List<Answer> Answer_List { get; set; }

        /// <summary>
        /// Получить из БД результаты студента
        /// </summary>
        public List<QuizResult> Get(int user_id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            List<QuizResult> quizResult = null;
            try
            {
                using (var dbconn = quizRepository.Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    /*
                     EXECUTE P_GETQUIZRESULTS @UserId = @uid
                    */
                    command.CommandText = P_GetQuizResults; // see DALResources.resx,
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@uid", DbType.Int32, user_id.ToString(), null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var quizes = command.ExecuteReader())
                    {
                        while (quizes.Read())
                        {
                            if (!string.IsNullOrEmpty(quizes[0].ToString()))
                            {
                                XmlSerializer quizFormat = new XmlSerializer(typeof(List<QuizResult>));
                                using (MemoryStream ms = new MemoryStream(
                                    System.Text.Encoding.Unicode.GetBytes(quizes[0].ToString())))
                                {
                                    quizResult = (List<QuizResult>)quizFormat.Deserialize(ms);
                                }
                            }
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                quizResult = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return quizResult;
        }

        /// <summary>
        /// Получить из БД результаты одного квиза студента
        /// </summary>
        public QuizResult GetQuizResult(int quizresult_id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            QuizResult quizResult = null;
            try
            {
                using (var dbconn = quizRepository.Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    /*
                     EXECUTE P_GETQUIZRESULT @QuizResultId = @qrid
                    */
                    command.CommandText = P_GetQuizResult; // see DALResources.resx,
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qrid", DbType.Int32, quizresult_id.ToString(), null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var quizes = command.ExecuteReader())
                    {
                        while (quizes.Read())
                        {
                            XmlSerializer quizFormat = new XmlSerializer(typeof(QuizResult));
                            using (MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(quizes[0].ToString())))
                            {
                                quizResult = (QuizResult)quizFormat.Deserialize(ms);
                            }
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                quizResult = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return quizResult;
        }

        /// <summary>
        /// закончить квиз - оформить результаты
        /// </summary>
        /// <returns></returns>
        public bool Save(int quizresult_id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            var saveResult = false;
            try
            {
                using (var dbconn = quizRepository.Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    /*   эту команду угнал в файл ресурсов под именем P_SAVEQUIZRESULT
                 EXECUTE [dbo].[P_SAVEQUIZRESULT] 
                    @QuizResultId=@qrid, 
                    @ERROR=@er OUT, 
                    @ERRORTEXT=@et OUT
                 */
                    command.CommandText = P_SaveQuizResult;  // see DALResources.resx,
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qrid", DbType.Int32, quizresult_id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@et", DbType.String, null, 1000, ParameterDirection.Output));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();
                    var saveError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                    var saveErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                    if (saveError == 0) saveResult = true;
                    //// проверю, что действительно что-то возвращается
                    Logger.Debug(string.Format("P_SaveQuizResult out : {0} {1}\n", saveError.ToString(), saveErrorText));
                }
            }
            catch (DbException ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return saveResult;
        }
    }
}
