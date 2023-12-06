using Sanatorium.ViewModel;
using System.Windows;

namespace Sanatorium.View
{
    public partial class CustomerSelectionView : Window
    {
        public CustomerSelectionView()
        {
            InitializeComponent();
            DataContext = new CustomerSelectionViewModel();
        }
    }
}
