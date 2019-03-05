namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using DAL;

    /*
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IItem, new()
    {
        public List<T> ItemList { get; set; }

        /// <summary>
        /// Удалить Item, в т.ч. и из БД, id = Item.ID
        /// <returns>bool Success?</returns>
        /// </summary>
        public bool Delete(int id)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            bool boolresult = false;
            try
            {
                int personListIndex = ItemList.FindIndex(p => p.ID == id);
 ///                if (personListIndex == -1) 
                if (boolresult = ItemList[personListIndex].Delete())

                    // Если удаление успешно, надо перезапросить из БД список item
                    // а пока просто удалю из списка
                    ItemList.RemoveAt(personListIndex);
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
        /// Возвращает список всех item системы
        /// </summary>
        /// <returns>ItemList</returns>
        public List<T> GetAll()
        {
            // Наверно надо перезапросить из БД список пользователей
  //          ItemList.Load();
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
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
                if (boolresult = entity.Save())
                {
                    // надо обновить инф. в списке
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
    */
}
