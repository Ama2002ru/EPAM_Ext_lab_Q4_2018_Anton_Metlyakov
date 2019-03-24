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
    /// Класс содержит список со всеми тестами в системе
    /// </summary>
    public class QuizRepository : IQuizRepository // BaseRepository<QuizClass>
    {
        /// <summary>
        /// default Constructor
        /// </summary>
        public QuizRepository()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            this.ItemList = new List<Quiz>(0);
        }

        /// <summary>
        /// конструктор с инверсией зависимостей
        /// </summary>
        /// <param name="db"></param>
        public QuizRepository(IdbConnector db)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            Db = db;
            this.ItemList = new List<Quiz>(0);
        }

        /// <summary>
        /// Количество записей в списке
        /// </summary>
        public int Count
        {
            get
            {
                return ItemList.Count;
            }
        }

        public IdbConnector Db { get; }

        private List<Quiz> ItemList { get; set; }

        /// <summary>
        /// удаление квиза
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool deleteResult = false;
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    /*   эту команду угнал в файл ресурсов под именем P_SaveQuiz
                    EXECUTE [dbo].[P_DELETEQUIZ] 
                    @QuizID=@qid, 
                    @ERROR=@er OUT, 
                    @ERRORTEXT=@et OUT
                    */
                    command.CommandText = P_DeleteQuiz;
                    command.Parameters.Add(Db.CreateParameter("@qid", DbType.Int32, id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(Db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                    command.Parameters.Add(Db.CreateParameter("@et", DbType.String, null, 1000, ParameterDirection.Output));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();

                    var deleteError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                    var deleteErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                    if (deleteError == 0) deleteResult = true;
                    //// проверю, что действительно что-то возвращается
                    Logger.Debug(string.Format("P_DeleteQuiz out : {0} {1}\n", deleteError.ToString(), deleteErrorText));
                }
            }
            catch (DbException ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return deleteResult;
        }

        /// <summary>
        /// запросить из БД один квиз
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Quiz Get(int id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            Quiz quiz = null;
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = V_Get_Quiz; // see DALResources.resx,
                    command.Parameters.Add(Db.CreateParameter("@quiz_id", DbType.Int32, id.ToString(), null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var quizes = command.ExecuteReader())
                    {
                        while (quizes.Read())
                        {
                            XmlSerializer quizFormat = new XmlSerializer(typeof(Quiz));
                            using (MemoryStream ms = new MemoryStream(
                                System.Text.Encoding.Unicode.GetBytes(quizes[0].ToString())))
                            {
                                quiz = (Quiz)quizFormat.Deserialize(ms);
                            }
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                quiz = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "There is an error in XML document (0, 0).")
                {
                    quiz = null;
                    Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                }

                throw new Exception(string.Empty, ex);
            }

            return quiz;
        }

        /// <summary>
        /// запросить из БД список квизов
        /// </summary>
        /// <returns></returns>
        public List<Quiz> GetAll()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            List<Quiz> newQuizesList = null;
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    command.CommandText = P_GetAllQuizes;  // see DALResources.resx,
                    command.Parameters.Add(Db.CreateParameter("@rc", DbType.Int32, "-1", null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var quizes = command.ExecuteReader())
                    {
                        while (quizes.Read())
                        {
                            XmlSerializer quizFormat = new XmlSerializer(typeof(List<Quiz>));
                            using (MemoryStream ms = new MemoryStream(
                                System.Text.Encoding.Unicode.GetBytes(quizes[0].ToString())))
                            {
                                newQuizesList = (List<Quiz>)quizFormat.Deserialize(ms);
                            }
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                newQuizesList = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "There is an error in XML document (0, 0).")
                {
                    // это значит список квизов в БД пуст   
                    // пляшем дальше
                    newQuizesList = new List<Quiz>(0);
                }
            }

            ItemList = newQuizesList;
            return ItemList;
        }

        /// <summary>
        /// взять из БД следующий неотвеченный вопрос для этого квиза и пользователя
        /// </summary>
        /// <returns></returns>
        public List<Question> GetNextQuestion(int quizresult_id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            List<Question> newQuestionList = null;
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    /*   эту команду угнал в файл ресурсов под именем P_GetNextQuestion
                 EXECUTE [dbo].[P_GETNEXTQUESTION] 
                    @RecordCount=@rc,
                    @QuizResultId=@qrid 
                 */
                    command.CommandText = P_GetNextQuestion;  // see DALResources.resx,
                    command.Parameters.Add(Db.CreateParameter("@rc", DbType.Int32, "1", null, ParameterDirection.Input));
                    command.Parameters.Add(Db.CreateParameter("@qrid", DbType.Int32, quizresult_id.ToString(), null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var questions = command.ExecuteReader())
                    {
                        while (questions.Read())
                        {
                            if (!string.IsNullOrEmpty(questions[0].ToString()))
                            {
                                try
                                {
                                    XmlSerializer questionFormat = new XmlSerializer(typeof(List<Question>));
                                    using (MemoryStream ms = new MemoryStream(
                                        System.Text.Encoding.Unicode.GetBytes(questions[0].ToString())))
                                    {
                                        newQuestionList = (List<Question>)questionFormat.Deserialize(ms);
                                    }
                                }
                                catch (InvalidOperationException ex)
                                {
                                    newQuestionList = null;
                                    Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                                    throw new Exception(string.Empty, ex);
                                }
                            }
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                newQuestionList = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return newQuestionList;
        }

        /// <summary>
        /// сохранение данных квиза
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public bool Save(Quiz q)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool saveResult = false;
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    /*   эту команду угнал в файл ресурсов под именем P_SaveQuiz
                    EXECUTE [dbo].[P_SAVEQUIZ] 
                    @QuizID=@qid, 
                    @QuizName = @qn,
                    @AuthorID = @a,
                    @CreatedDate=@cdt,
                    @SuccessRate=@sr,
                    @ERROR=@er OUT, 
                    @ERRORTEXT=@et OUT
                    */
                    command.CommandText = P_SaveQuiz;
                    command.Parameters.Add(Db.CreateParameter("@qid", DbType.Int32, q.ID.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(Db.CreateParameter("@qn", DbType.String, q.Quiz_Name, q.Quiz_Name.Length, ParameterDirection.Input));
                    command.Parameters.Add(Db.CreateParameter("@a", DbType.Int32, q.Author_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(Db.CreateParameter("@cdt", DbType.DateTime, q.Created_Date.ToString(), q.Created_Date.ToString().Length, ParameterDirection.Input));
                    command.Parameters.Add(Db.CreateParameter("@sr", DbType.Single, q.Success_Rate.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(Db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                    command.Parameters.Add(Db.CreateParameter("@et", DbType.String, null, 1000, ParameterDirection.Output));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();

                    var saveError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                    var saveErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                    if (saveError == 0) saveResult = true;

                    // проверю, что действительно что-то возвращается
                    Logger.Debug(string.Format("P_SaveQuiz out : {0} {1}\n", saveError.ToString(), saveErrorText));
                }
            }
            catch (DbException ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return saveResult;
        }

        /// <summary>
        /// Возьму из БД список квиз-пользователь для редактирования назначения квиза
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public AssignQuiz GetQuizAssignment(int user_id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            AssignQuiz quizAssignment = null;
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    /*   эту команду угнал в файл ресурсов под именем P_GETQUIZASSIGNMENT
                 EXECUTE [dbo].[P_GETQUIZASSIGNMENT] 
                    @UserId=@uid 
                 */
                    command.CommandText = P_GetQuizAssignment;  // see DALResources.resx,
                    command.Parameters.Add(Db.CreateParameter("@uid", DbType.Int32, user_id.ToString(), null, ParameterDirection.Input));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    using (var assquizes = command.ExecuteReader())
                    {
                        while (assquizes.Read())
                        {
                            try
                            {
                                XmlSerializer quizFormat = new XmlSerializer(typeof(AssignQuiz));
                                using (MemoryStream ms = new MemoryStream(
                                    System.Text.Encoding.Unicode.GetBytes(assquizes[0].ToString())))
                                {
                                    quizAssignment = (AssignQuiz)quizFormat.Deserialize(ms);
                                }
                            }
                            catch (InvalidOperationException ex)
                            {
                                quizAssignment = null;
                                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                                throw new Exception(string.Empty, ex);
                            }
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                quizAssignment = null;
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }

            return quizAssignment;
        }

        /// <summary>
        /// Сохраним в БД отредактированный список
        /// </summary>
        /// <param name="quizes"></param>
        /// <returns></returns>
        public bool SaveQuizAssignment(AssignQuiz quizes)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            var saveResult = false;
            byte[] byteArray = null;
            string xmlQuizes = null;
            try
            {
                using (var dbconn = Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    /*   эту команду угнал в файл ресурсов под именем P_SaveQuizAssignment
                 EXECUTE [dbo].[P_SAVEQUIZASSIGNMENT] 
                    @XmlQuizes=@xmlq,
                    @ERROR=@er OUT, 
                    @ERRORTEXT=@et OUT
                 */
                    XmlSerializer quizFormat = new XmlSerializer(typeof(AssignQuiz));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        quizFormat.Serialize(ms, quizes);
                        ms.Seek(0, SeekOrigin.Begin);
                        byteArray = new byte[ms.Length + 1];
                        ms.Read(byteArray, 0, (int)ms.Length);
                        xmlQuizes = Encoding.Default.GetString(byteArray);
                    }

                    command.CommandText = P_SaveQuizAssignment;  // see DALResources.resx,
                    command.Parameters.Add(Db.CreateParameter("@xmlq", DbType.String, xmlQuizes, xmlQuizes.Length + 1, ParameterDirection.Input));
                    command.Parameters.Add(Db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                    command.Parameters.Add(Db.CreateParameter("@et", DbType.String, null, 1000, ParameterDirection.Output));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();

                    var saveError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                    var saveErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                    if (saveError == 0) saveResult = true;

                    // проверю, что действительно что-то возвращается
                    Logger.Debug(string.Format("P_SAVEQUIZASSIGNMENT out : {0} {1}\n", saveError.ToString(), saveErrorText));
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