using Microsoft.EntityFrameworkCore;
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
        private EditingARoomView _activeEditingARoomView;

        private ObservableCollection<string> _types;
        private string _selectedType;

        public string SelectedType
        {
            get
            {
                return _selectedType;
            }
            set
            {
                _selectedType = value;
                LoadRooms(_selectedType);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Types
        {
            get
            {
                return _types;
            }
            set
            {
                _types = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand ShowEditWindowCommand { get; private set; }

        public ICommand ChangeStatusCommand {  get; private set; }
        public RoomsViewModel()
        {
            _rooms = new ObservableCollection<Room>();
            _types = new ObservableCollection<string>();
            _types.Add("Все номера");

            ShowAddRoomCommand = new ViewModelCommand(ExecuteShowAddRoomWindowCommand);
            ShowEditWindowCommand = new ViewModelCommand(ExecuteShowEditWindowCommand);
            ChangeStatusCommand = new ViewModelCommand(ExecuteChangeStatusCommand);


            LoadRooms();
            _types = new ObservableCollection<string>(_types.Concat(LoadTypesOfRoom()));
        }

        private void ExecuteChangeStatusCommand(object obj)
        {
            if(obj is Room room)
            {
                using (var context = new SanatoriumContext()) 
                {
                    var roomFromDatabase = context.Rooms.Include(r => r.Status).FirstOrDefault(r => r.Id == room.Id);

                    if (roomFromDatabase != null)
                    {
                        if (roomFromDatabase.Status.Name == "Готов")
                        {
                            roomFromDatabase.Status = context.RoomStatuses.Single(s => s.Name == "Уборка");
                        }
                        else if (roomFromDatabase.Status.Name == "Уборка")
                        {
                            roomFromDatabase.Status = context.RoomStatuses.Single(s => s.Name == "Готов");
                        }

                        context.SaveChanges();
                    }
                }
            }

            LoadRooms();
        }

        private void ExecuteShowEditWindowCommand(object obj)
        {

            if (obj is Room room)
            {
                if (_activeEditingARoomView == null || _activeEditingARoomView.DataContext == null)
                {
                    var editViewModel = new EditingARoomViewModel(room);

                    editViewModel.Close += CloseEditingATreatmentProgramView;

                    _activeEditingARoomView = new EditingARoomView
                    {
                        DataContext = editViewModel
                    };

                    _activeEditingARoomView.Show();
                }
            }
        }

        private void CloseEditingATreatmentProgramView()
        {
            if (_activeEditingARoomView != null)
            {
                _activeEditingARoomView.Close();
                _activeEditingARoomView = null;
                LoadRooms();
            }
        }

        private ObservableCollection<string> LoadTypesOfRoom()
        {
            using (var context = new SanatoriumContext())
            {
                return new ObservableCollection<string>(context.TypeOfRooms.Select(t => t.Type).ToList());
            }

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
                _currentActiveAdditionWindow?.Close();
                _currentActiveAdditionWindow = null;
                LoadRooms();
            }
        }

        public void LoadRooms(string? str = null)
        {
            using (var context = new SanatoriumContext())
            {
                if(str == null || str == _types[0])
                    Rooms = new ObservableCollection<Room>(context.Rooms.Include(a => a.Type).Include(a => a.Status).ToList());
                else
                    Rooms = new ObservableCollection<Room>(context.Rooms.Where(r => r.Type.Type == str));
            }
        }
    }
}
