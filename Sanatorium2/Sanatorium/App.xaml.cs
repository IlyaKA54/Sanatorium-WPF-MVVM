using Sanatorium.Users;
using Sanatorium.View;
using Sanatorium.ViewModel;
using System.Windows;

namespace Sanatorium
{
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var mainView = new MainWindow { DataContext = new MainViewModel(new Admin()) };
            mainView.Show();

            //var loginVM = new LoginViewModel();
            //var loginView = new LoginView();
            //loginView.SetDataContext(loginVM);

            //loginView.Show();

        }
    }
}
