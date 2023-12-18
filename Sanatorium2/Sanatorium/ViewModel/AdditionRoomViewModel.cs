using Microsoft.Win32;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sanatorium.ViewModel;

public class AdditionRoomViewModel : ViewModelBase
{
    private ObservableCollection<string> _types;
    private string? _name;
    private string _price;
    private string _numberOfPlaces;
    private ImageSource _image;
    private string? _description;
    private string _selectedType;
    private string? _errorMessage;

    private string? _path;
    private IDbRepos _repos;

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

    public string? Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }


    public string Price
    {
        get => _price;
        set
        {
            _price = value;
            OnPropertyChanged();
        }
    }

    public string NumberOfPlaces
    {
        get => _numberOfPlaces;
        set
        {
            _numberOfPlaces = value;
            OnPropertyChanged();
        }
    }

    public ImageSource Image
    {
        get
        {
            return _image;
        }

        set
        {
            _image = value;
            OnPropertyChanged();
        }
    }

    public string? Description
    {
        get
        {
            return _description;
        }

        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    public string SelectedType
    {
        get
        {
            return _selectedType;
        }

        set
        {
            _selectedType = value;
            OnPropertyChanged();
        }
    }

    public string? ErrorMessage
    {
        get
        {
            return _errorMessage;
        }

        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddRoomCommand { get; private set; }
    public ICommand UploadImageCommand { get; private set; }
    public ICommand CloseWindowCommand { get; private set; }

    public Action Close;
    public AdditionRoomViewModel()
    {
        AddRoomCommand = new ViewModelCommand(ExecuteAddRoomCommand, CanExecuteAddRoomCommand);
        UploadImageCommand = new ViewModelCommand(ExecuteUploadImageCommand);
        CloseWindowCommand = new ViewModelCommand(ExecuteCloseWindowCommand);
        _repos = new DbEFRepos();
        LoadTypesOfRoom();
    }

    private void ExecuteCloseWindowCommand(object obj)
    {
        Close?.Invoke();
    }

    private bool CanExecuteAddRoomCommand(object obj)
    {
        bool validDate;
        decimal parsedPrice;
        int parsedNumberOfPlaces;

        if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_description) || Image == null ||
            !decimal.TryParse(Price, out parsedPrice) || parsedPrice <= 0 ||
            !int.TryParse(_numberOfPlaces, out parsedNumberOfPlaces) || parsedNumberOfPlaces < 1)
            validDate = false;
        else
            validDate = true;

        return validDate;
    }

    private void ExecuteAddRoomCommand(object obj)
    {
        if(CheckRoom(_repos))
        {
            ErrorMessage = "Такая комната уже сущесвует";
            return;
        }

        var newRoom = GetNewRoom(_repos);
        _repos.Rooms.Create(newRoom);
        _repos.Save();
        Close?.Invoke();
    }

    private bool CheckRoom(IDbRepos repos)
    {
        var room = repos.Rooms.GetCollection().FirstOrDefault(a => a.Name == _name);

        return room != null;
    }

    private void ExecuteUploadImageCommand(object obj)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "Image files|*.bmp;*.jpg;*.png;*.jpeg;";
        fileDialog.FilterIndex = 1;

        if (fileDialog.ShowDialog() == true)
        {
            _path = fileDialog.FileName;
            UploadImage(_path);
        }
    }

    private void UploadImage(string filePath)
    {
        Image = new BitmapImage(new System.Uri(filePath));
    }

    private Room GetNewRoom(IDbRepos repos)
    {
        if (repos == null)
            throw new NullReferenceException("An empty context was passed");

        Room room = new Room()
        {
            Name = this.Name,
            Price = decimal.Parse(this.Price),
            NumberOfPlaces = int.Parse(this.NumberOfPlaces),
            Description = this.Description,
            Image = GetImageBytes(),
            Status = _repos.Statuses.GetCollection().First(t => t.Name == "Готов"),
            Type = GetTypeOfRoomByType(_selectedType, repos)
        };

        return room;
    }

    private TypeOfRoom GetTypeOfRoomByType(string type, IDbRepos repos)
    {
        if (repos == null)
            throw new NullReferenceException("An empty context was passed");

        return repos.Types.GetCollection().First(t => t.Type == type);
    }
    private void LoadTypesOfRoom()
    {
        Types = new ObservableCollection<string>(_repos.Types.GetCollection().Select(t => t.Type));
    }

    private byte[] GetImageBytes()
    {
        using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read))
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
