namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Reflection;
    using DAL;
    using log4net;
    using static DALResources;

    /// <summary>
    /// Класс описывает сущность - 1 вопрос теста.
    /// </summary>
    public class Question 
    {
        private IQuizRepository quizRepository;

        /// <summary>
        /// Безпараметрический конструктор для сериализатора
        /// </summary>
        public Question()
        {
        }

        public Question(IQuizRepository repo)
        {
            quizRepository = repo;
        }

        /// <summary>
        /// Параметризованный конструктор
        /// </summary>
        /// <param name="quiz_id"></param>
        /// <param name="question_id"></param>
        /// <param name="info"></param>
        /// <param name="text"></param>
        /// <param name="correctOptionFlag"></param>
        public Question(IQuizRepository repo, int quiz_id, int question_id, string info, string text, int correctOptionFlag, Variant[] options) : this(repo)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            this.Quiz_Id = quiz_id;
            this.Question_Id = question_id;
            this.Info = info;
            this.Text = text;
            this.CorrectOptionFlag = correctOptionFlag;
            this.Options = options;
        }

        public Variant[] Options { get; set; }

        /// <summary>
        /// Ссылка на ID вопроса в БД
        /// </summary>
        public int Question_Id { get; set; }

        /// <summary>
        /// Ссылка на ID вопроса в БД
        /// </summary>
        public int Quiz_Id { get; set; }

        /// <summary>
        /// Преамбула вопроса, если потребуется
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Текст самого вопроса
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Битовое поле - правильные ответы. 0х1 -1й, 0х2 - 2й, 0х4 -3й и т.д.
        /// </summary>
        public int CorrectOptionFlag { get; set; }

        /// <summary>
        /// сохранить вопрос в БД
        /// </summary>
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
                    /*   эту команду угнал в файл ресурсов под именем P_SaveQuestion
                                        EXECUTE[dbo].[P_SAVEQUESTION] 
                                        @QuizID=@qid, 
                                        @QuestionID=@qsid, 
                                        @info = @i,
                                        @text=@t,
                                        @CorrectOptionFlag=@cof,
                                        @ERROR=@er OUT, 
                                        @ERRORTEXT=@et OUT
                    */
                    command.CommandText = P_SaveQuestion;
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qid", DbType.Int32, Quiz_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qsid", DbType.Int32, Question_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@i", DbType.String, Info, Info.Length, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@t", DbType.String, Text, Text.Length, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@cof", DbType.Int32, CorrectOptionFlag.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@et", DbType.String, null, 100, ParameterDirection.Output));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();

                    var saveError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                    var saveErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                    if (saveError == 0) saveResult = true;

                    // проверю, что действительно что-то возвращается
                    Logger.Debug(string.Format("P_SaveQuestion out : {0} {1}\n", saveError.ToString(), saveErrorText));
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
        /// сохранить вопрос в БД
        /// </summary>
        public bool Delete()
            {
                Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
                bool deleteResult = false;
                try
                {
                    using (var dbconn = quizRepository.Db.CreateConnection())
                    {
                        dbconn.Open();
                        var command = dbconn.CreateCommand();
                        /*   эту команду угнал в файл ресурсов под именем P_DeleteQuestion
                    EXECUTE[dbo].[P_DELETEQUESTION] 
                    @QuizID=@qid, 
                    @QuestionID=@qsid, 
                    @ERROR=@er OUT, 
                    @ERRORTEXT=@et OUT
                        */
                        command.CommandText = P_DeleteQuestion;
                        command.Parameters.Add(quizRepository.Db.CreateParameter("@qid", DbType.Int32, Quiz_Id.ToString(), null, ParameterDirection.Input));
                        command.Parameters.Add(quizRepository.Db.CreateParameter("@qsid", DbType.Int32, Question_Id.ToString(), null, ParameterDirection.Input));
                        command.Parameters.Add(quizRepository.Db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                        command.Parameters.Add(quizRepository.Db.CreateParameter("@et", DbType.String, null, 100, ParameterDirection.Output));
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 10;
                        command.ExecuteNonQuery();

                        var deleteError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                        var deletErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                        if (deleteError == 0) deleteResult = true;
                        //// проверю, что действительно что-то возвращается
                        Logger.Debug(string.Format("P_DeleteQuestion out : {0} {1}\n", deleteError.ToString(), deletErrorText));
                        ///     сохранять список курсов персоны
                    }
                }
                catch (DbException ex)
                {
                    Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                    throw new Exception(string.Empty, ex);
                }

                return deleteResult;
            }
    }
}
