using Sanatorium.View;
using System.Windows;

namespace Sanatorium
{
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {

            var loginView = new LoginView();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (!loginView.IsVisible && loginView.IsLoaded)
                {
                    var mainView = new MainWindow();
                    mainView.Show();
                }
            };
        }
    }
}
