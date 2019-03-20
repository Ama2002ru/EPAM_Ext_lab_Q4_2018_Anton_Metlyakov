namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using static DALResources;

    /// <summary>
    /// Класс хранит информацию об ответе на 1 вопрос теста
    /// </summary>
    public class Answer
    {
        private IQuizRepository quizRepository;

        public Answer(IQuizRepository repo)
        {
            quizRepository = repo;
        }

        public Answer()
        {
        }

        public int Answer_Id { get; set; }

        /// <summary>
        /// Ссылка на ID вопроса в БД
        /// </summary>
        public int Question_Id { get; set; }

        public int Quiz_Id { get; set; }

        public int QuizResult_Id { get; set; }

        /// <summary>
        /// Битовое поле ответов студента
        /// </summary>
        public int Answer_Flag { get; set; }

        /// <summary>
        ///  Сколько времени потребовалось на ответ - потом попробую рассчитать
        /// </summary>
        public DateTime? TimeStamp { get; set; }

        /// <summary>
        /// Сохранить ответ в БД
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool saveResult = false;
            try
            {
                using (var dbconn = quizRepository.Db.CreateConnection())
                {
                    dbconn.Open();
                    var command = dbconn.CreateCommand();
                    /*   эту команду угнал в файл ресурсов под именем P_SaveAnswer
                                        EXECUTE[dbo].[P_SAVEANSWER] 
                                        @QuizResultId=@qrid,
                                        @QuestionID=@qsid, 
                                        @AnswerID = @aid,
                                        @AnswerFlag=@af,
                                        @ERROR=@er OUT, 
                                        @ERRORTEXT=@et OUT
                    */
                    command.CommandText = P_SaveAnswer;
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qrid", DbType.Int32, QuizResult_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qsid", DbType.Int32, Question_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@aid", DbType.Int32, Answer_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@af", DbType.Int32, Answer_Flag.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@et", DbType.String, null, 1000, ParameterDirection.Output));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();

                    var saveError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                    var saveErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                    if (saveError == 0) saveResult = true;
                    //// проверю, что действительно что-то возвращается
                    Logger.Debug(string.Format("P_SaveAnswer out : {0} {1}\n", saveError.ToString(), saveErrorText));
                    ///     сохранять список курсов персоны
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
