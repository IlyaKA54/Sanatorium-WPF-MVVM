using Sanatorium.ViewModel;
using System.Windows;
using System.Windows.Input;

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

    }
}
