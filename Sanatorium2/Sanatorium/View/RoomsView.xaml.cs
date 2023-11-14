using Sanatorium.ViewModel;
using System.Windows.Controls;

namespace Sanatorium.View
{
    public partial class RoomsView : UserControl
    {
        public RoomsView()
        {
            InitializeComponent();
            DataContext = new RoomsViewModel();
        }
    }
}
