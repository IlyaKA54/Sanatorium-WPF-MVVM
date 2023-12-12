using Sanatorium.ViewModel;
using System.Security.Policy;
using System.Windows;
using System.Windows.Input;

namespace Sanatorium.View;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void OnButtonMinimizeClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void OnButtonCloseClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    public void SetDataContext(LoginViewModel viewModel)
    {
        viewModel.Close += this.Close;
        DataContext = viewModel;
    }
}
