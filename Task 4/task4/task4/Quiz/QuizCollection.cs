namespace Task4
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Класс содержит список со всеми тестами в системе
    /// </summary>
    public class QuizCollection
    {
        /// <summary>
        /// хранит все объекты-Quiz'ы
        /// по идее это список/массив, но Диаграмма классов теряет ассоциацию
        /// когда я ставлю тип  LIST<QuizClass>
        /// </summary>
        public QuizClass QuizList
        {
            get => default(QuizClass);
            set
            {
            }
        }

        /// <summary>
        /// Добавить тест в список
        /// </summary>
        public void Add()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Удалить тест из списка
        /// </summary>
        public void Delete()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Изменить тест в списке
        /// </summary>
        public void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}
