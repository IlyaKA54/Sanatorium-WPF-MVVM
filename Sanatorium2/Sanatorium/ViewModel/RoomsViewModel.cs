using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using Sanatorium.Users;
using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sanatorium.ViewModel;

public class RoomsViewModel : ViewModelBase
{
    private ObservableCollection<Room> _rooms;

    private Window _currentActiveWindow;

    private ObservableCollection<string> _types;
    private IDbRepos _repos;
    private string _selectedType;
    private UserRoleInfo _user;

    public UserRoleInfo User => _user;

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

    public ICommand ChangeStatusCommand { get; private set; }
    public RoomsViewModel(UserRoleInfo user)
    {
        _repos = new DbEFRepos();
        _types = new ObservableCollection<string>();

        _user = user;

        _types.Add("Все номера");
        _types = new ObservableCollection<string>(_types.Concat(LoadTypesOfRoom()));

        ShowAddRoomCommand = new ViewModelCommand(ExecuteShowAddRoomWindowCommand);
        ShowEditWindowCommand = new ViewModelCommand(ExecuteShowEditWindowCommand);
        ChangeStatusCommand = new ViewModelCommand(ExecuteChangeStatusCommand);

        LoadRooms();
    }

    private void ExecuteChangeStatusCommand(object obj)
    {
        if (obj is Room room)
        {
            _repos = new DbEFRepos();

            var roomRepository = _repos.Rooms;

            var roomFromDatabase = roomRepository.GetCollection()
                .FirstOrDefault(r => r.Id == room.Id);

            if (roomFromDatabase != null)
            {
                if (room.Status.Name == "Готов")

                    roomFromDatabase.Status = _repos.Statuses.GetCollection().Single(s => s.Name == "Уборка");
                else
                    roomFromDatabase.Status = _repos.Statuses.GetCollection().Single(s => s.Name == "Готов");

                _repos.Save();
            }

            LoadRooms();
        }

    }

    private void ExecuteShowEditWindowCommand(object obj)
    {

        if (obj is Room room)
        {
            if (_currentActiveWindow == null || _currentActiveWindow.DataContext == null)
            {
                var editViewModel = new EditingARoomViewModel(room);

                editViewModel.Close += CloseAdditionalCustomerWindow;

                _currentActiveWindow = new EditingARoomView
                {
                    DataContext = editViewModel
                };

                _currentActiveWindow.Show();
            }
        }
    }

    private IEnumerable<string> LoadTypesOfRoom()
    {
        return new ObservableCollection<string>(_repos.Types.GetCollection().Select(t => t.Type).ToList());
    }

    private void ExecuteShowAddRoomWindowCommand(object obj)
    {
        if (_currentActiveWindow == null)
        {
            _currentActiveWindow = new AdditionRoomView();
            (_currentActiveWindow.DataContext as AdditionRoomViewModel).Close += CloseAdditionalCustomerWindow;
            _currentActiveWindow.Show();
        }
    }

    private void CloseAdditionalCustomerWindow()
    {
        if (_currentActiveWindow != null)
        {
            _currentActiveWindow?.Close();
            _currentActiveWindow = null;
            LoadRooms();
        }
    }

    public void LoadRooms(string? str = null)
    {
        _repos = new DbEFRepos();

        if (string.IsNullOrEmpty(str) || str == _types[0])
            Rooms = new ObservableCollection<Room>(_repos.Rooms.GetCollection());
        else
            Rooms = new ObservableCollection<Room>(_repos.Rooms.GetCollection().Where(r => r.Type.Type == str).ToList());

        CalculateTheNumberOfFreeSeatsInTheRoom(Rooms);
    }

    private void CalculateTheNumberOfFreeSeatsInTheRoom(IEnumerable<Room> rooms)
    {
        var currentDate = DateTime.Now;

        foreach (var room in rooms)
        {
            var numberOfOccupiedSeatsInTheRoom = _repos.CustomerOrders.GetCollection()
                .Where(a => currentDate >= a.IdOrder.ArrivalDate && currentDate <= a.IdOrder.DateOfDeparture && a.Room.Id == room.Id)
                .Count();

            room.NumberOfPlaces -= numberOfOccupiedSeatsInTheRoom;
        }
    }
}
