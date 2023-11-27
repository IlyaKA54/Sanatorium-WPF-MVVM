using Sanatorium.Model.Entities;
using System.Windows.Controls;

namespace Sanatorium.Controls
{
    /// <summary>
    /// Логика взаимодействия для RoomPopupContentControl.xaml
    /// </summary>
    public partial class RoomPopupContentControl : UserControl
    {
        public RoomPopupContentControl(Room room)
        {
            InitializeComponent();
            DataContext = room;
        }
    }
}
