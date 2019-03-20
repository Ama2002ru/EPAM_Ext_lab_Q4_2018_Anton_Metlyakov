namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Reflection;
    using static DALResources;

    /// <summary>
    /// Класс описывает вариант ответа 
    /// </summary>
    public class Variant
    {
        private IQuizRepository quizRepository;

        /// <summary>
        /// Для сериализатора
        /// </summary>
        public Variant()
        {
        }

        public Variant(IQuizRepository repo)
        {
            quizRepository = repo;
        }

        public int Variant_Id { get; set; }

        /// <summary>
        /// Ссылка на ID вопроса в БД
        /// </summary>
        public int Question_Id { get; set; }

        public int Quiz_Id { get; set; }

        /// <summary>
        ///  Массив вариантов ответов в human-readable виде
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// для отображение checkbox"ов во View
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Изменение блока ответов к вопросу теста. пока не понимаю механизма реализации.
        /// скорей всего этод метод будет переопределен в QuestionClass
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
                    /*   эту команду угнал в файл ресурсов под именем P_SaveVariant
                                        EXECUTE [dbo].[P_SAVEVARIANT] 
                                        @QuizID=@qid, 
                                        @QuestionID=@qsid, 
                                        @VariantID = @vid,
                                        @text=@t,
                                        @ERROR=@er OUT, 
                                        @ERRORTEXT=@et OUT

                    */
                    command.CommandText = P_SaveVariant;
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qid", DbType.Int32, Quiz_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qsid", DbType.Int32, Question_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@vid", DbType.Int32, Variant_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@t", DbType.String, Text, Text.Length, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@er", DbType.Int32, null, null, ParameterDirection.Output));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@et", DbType.String, null, 100, ParameterDirection.Output));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();

                    var saveError = (int)((IDbDataParameter)command.Parameters["@er"]).Value;
                    var saveErrorText = (string)((IDbDataParameter)command.Parameters["@et"]).Value;
                    if (saveError == 0) saveResult = true;
                    //// проверю, что действительно что-то возвращается
                    Logger.Debug(string.Format("P_SaveVariant out : {0} {1}\n", saveError.ToString(), saveErrorText));
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
        /// Удалить вариант из БД
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
                    /*   эту команду угнал в файл ресурсов под именем P_DeleteVariant
                EXECUTE[dbo].[P_DELETEVARIANT] 
                @QuizID=@qid, 
                @QuestionID=@qsid, 
                @VairantID=@vid,
                @ERROR=@er OUT, 
                @ERRORTEXT=@et OUT
                    */
                    command.CommandText = P_DeleteVariant;
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qid", DbType.Int32, Quiz_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@qsid", DbType.Int32, Question_Id.ToString(), null, ParameterDirection.Input));
                    command.Parameters.Add(quizRepository.Db.CreateParameter("@vid", DbType.Int32, Variant_Id.ToString(), null, ParameterDirection.Input));
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
