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

    public static class SetLogonDate
    {
        /// <summary>
        /// Jnvt
        /// </summary>
        /// <returns></returns>
        public static bool Set_Logon_Date(IPersonRepository personRepository, string username)
        {
            Logger.Debug(string.Format("{0}.{1} start", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name));
            try
            {
                var person = personRepository.GetAll().Find(x => x.UserName.ToUpper() == username.ToUpper());
                person.LastLogonDate = DateTime.Now;
                return personRepository.Save(person);
            }
            catch (DbException ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                throw new Exception(string.Empty, ex);
            }
        }
    }
}
