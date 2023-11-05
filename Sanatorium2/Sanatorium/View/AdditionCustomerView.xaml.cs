using Microsoft.Win32;
using Sanatorium.ViewModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Sanatorium.View
{

    public partial class AdditionCustomerView : Window
    {
        public AdditionCustomerView()
        {
            InitializeComponent();
            DataContext = new AdditionCustomerViewModel();
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

        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
