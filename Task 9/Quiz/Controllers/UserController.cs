namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;
    using Ninject;

    /// <summary>
    /// User controller
    /// </summary>
    public class UserController : Controller
    {
        private readonly IPersonRepository repository;

        public UserController(IPersonRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// creates PersonStore and fills it with data from DB
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Persons = this.repository.GetAll();
            return this.View();
        }

        /// <summary>
        /// Under construction ...
        /// </summary>
        /// <returns></returns>
        public ActionResult Save()
        {
            return this.View();
        }

        /// <summary>
        /// Under construction ...
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            return this.View();
        }
    }
}