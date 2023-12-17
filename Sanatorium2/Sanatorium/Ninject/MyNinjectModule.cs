using Ninject.Modules;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using System;

namespace Sanatorium.Ninject
{
    public class MyNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>();
        }
    }
}
