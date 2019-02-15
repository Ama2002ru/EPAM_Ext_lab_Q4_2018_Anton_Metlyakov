namespace Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;
    using Ninject;

    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            this.kernel = kernelParam;
            this.AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            this.kernel.Bind<IPersonRepository>().To<PersonRepository>().WithConstructorArgument("db", new SQLConnectorClass("QuizDBConection"));
        }
    }
}