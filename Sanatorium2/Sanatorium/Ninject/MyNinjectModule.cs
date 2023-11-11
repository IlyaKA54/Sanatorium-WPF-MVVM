using Ninject.Modules;
using Sanatorium.Model.Repositories;
using System;

namespace Sanatorium.Ninject
{
    public class MyNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserRepository>().To<UserRepository>();
        }
    }
}
