namespace DAL.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using DAL.Utility;

    public abstract class BaseRepositoryClass<T> : IBaseRepository<T> where T : class, IItem, new()
    {
        /// <summary>
        /// Массив/список хранимых сущностей
        /// </summary>
        public List<T> ItemList { get; set; }

        /// <summary>
        /// Удалить Item, в т.ч. и из БД, id = Item.ID
        /// <returns>bool Success?</returns>
        /// </summary>
        public bool Delete(int id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = false;
            int itemListIndex;
            try
            {
                itemListIndex = ItemList.FindIndex(p => p.ID == id);
                if (itemListIndex == -1) return boolresult;

                // На мой взгляд обобщенный репозиторий не сможет удалить один item из Базы данных
                // так для этого нужны разные SQL команды.
                // поэтому удаление item"а доверяю самому item"у
                if (boolresult = ItemList[itemListIndex].Delete())

                    // Если удаление успешно, надо перезапросить из БД список item
                    // а пока просто удалю из списка
                    ItemList.RemoveAt(itemListIndex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                boolresult = false;
                Logger.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

            return boolresult;
        }

        /// <summary>
        /// Вернуть из списка item, id = item.ID
        /// </summary>
        /// <param name="id"> id = item.ID</param>
        /// <returns>ссылка на Item в списке, Null - что то пошло не так :(</returns>
        public T Get(int id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            try
            {
                // верну ссылку на существующий item
                return ItemList[ItemList.FindIndex(item => item.ID == id)];
            }
            catch (NullReferenceException ex)
            {
                Logger.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return null;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Logger.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Возвращает список всех item класса
        /// </summary>
        /// <returns>ItemList</returns>
        public List<T> GetAll()
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));

            // вернуть существующий список
            return ItemList;
        }

        /// <summary>
        ///  Записать в БД инфо о item.Ожидаю что item.ID будет корректным и не проверяю его
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Save(T entity)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = false;
            int itemListIndex;
            try
            {
                // Аналогично удалению спускаю операцию сохранения в БД на уровень item'a
                if (boolresult = entity.Save())
                {
                    // надо обновить инф. в списке ItemList
                    itemListIndex = ItemList.FindIndex(item => item.ID == entity.ID);
                    if (itemListIndex == -1)
                        ItemList.Add(entity);  // новая запись
                    else
                        ItemList[itemListIndex] = entity; // существующая запись
                }
            }
            catch (NullReferenceException ex)
            {
                Logger.Error(string.Format("{0}.{1} {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

            return boolresult;
        }
    }
}
