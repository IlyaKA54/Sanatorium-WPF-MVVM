using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sanatorium.ViewModel
{
    public class RoomsViewModel : ViewModelBase
    {
        private ObservableCollection<Room> _rooms;

        private AdditionRoomView _currentActiveAdditionWindow;

        public ObservableCollection<Room> Rooms
        {
            get
            {
                return _rooms;
            }
            set
            {
                _rooms = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowAddRoomCommand { get; private set; }

        public RoomsViewModel()
        {
            _rooms = new ObservableCollection<Room>();

            ShowAddRoomCommand = new ViewModelCommand(ExecuteShowAddRoomWindowCommand);

            LoadRooms();
        }

        private void ExecuteShowAddRoomWindowCommand(object obj)
        {
            if (_currentActiveAdditionWindow == null)
            {
                _currentActiveAdditionWindow = new AdditionRoomView();
                (_currentActiveAdditionWindow.DataContext as AdditionRoomViewModel).Close += CloseAdditionalCustomerWindow;
                _currentActiveAdditionWindow.Show();
            }
        }

        private void CloseAdditionalCustomerWindow()
        {
            if (_currentActiveAdditionWindow != null)
            {
                _currentActiveAdditionWindow.Close();
                _currentActiveAdditionWindow = null;
            }
        }

        public void LoadRooms(string? str = null)
        {
            using (var context = new SanatoriumContext())
            {

                Rooms = new ObservableCollection<Room>(context.Rooms.ToList());

            }
        }
    }
}
