using Ninject;
using Sanatorium.Ninject;
using Sanatorium.View;
using Sanatorium.ViewModel;
using System.Windows;

namespace Sanatorium
{
    public partial class App : Application
    {
        private IKernel _kernel;
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            _kernel = new StandardKernel(new MyNinjectModule());

            var loginVM = _kernel.Get<LoginViewModel>();
            var loginView = new LoginView ();
            loginView.SetDataContext(loginVM);

            loginView.Show();

        }
    }
}
