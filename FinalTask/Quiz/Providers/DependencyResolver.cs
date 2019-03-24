namespace Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using DAL;
    using Ninject;
    using Quiz.Controllers; 

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
            this.kernel.Bind<IPersonRepository>().To<PersonRepository>().WithConstructorArgument("db", new SQLConnector("QuizDBConection"));
            this.kernel.Bind<IRolesRepository>().To<RolesRepository>().WithConstructorArgument("db", new SQLConnector("QuizDBConection"));
            this.kernel.Bind<IQuizRepository>().To<QuizRepository>().WithConstructorArgument("db", new SQLConnector("QuizDBConection"));
            this.kernel.Bind<IAuthProvider>().To<MyAuthProvider>();

            // black !@#$%^ magic :(
            this.kernel.Inject(Roles.Provider);
        }
    }
}