using System.Windows;
using System.Windows.Controls;

namespace Sanatorium.Controls;


public partial class PasswordBox : UserControl
{

    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(PasswordBox));

    public string Password
    {
        get { return (string)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }

    public PasswordBox()
    {
        InitializeComponent();
        txtPassword.PasswordChanged += OnPasswordChanged;
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = txtPassword.Password;
    }
}
