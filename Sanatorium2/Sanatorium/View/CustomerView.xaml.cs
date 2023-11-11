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

    }
}
