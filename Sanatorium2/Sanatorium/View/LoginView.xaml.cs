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

    private void OnButtonLoginClick(object sender, RoutedEventArgs e)
    {

    }
}
