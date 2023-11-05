using Sanatorium.ViewModel;
using System.Windows.Controls;

namespace Sanatorium.View
{

    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
            DataContext = new CustomersViewModel();
        }

        private void OnAddButton(object sender, System.Windows.RoutedEventArgs e)
        {
            //var addWindow = new AdditionCustomerView();
            //addWindow.Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
