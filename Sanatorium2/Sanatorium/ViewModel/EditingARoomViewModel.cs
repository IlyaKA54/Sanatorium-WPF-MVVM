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

public class EditingARoomViewModel : ViewModelBase
{
    private ObservableCollection<string> _types;
    private readonly Room _room;

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

    public ICommand SaveChangesCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }
    public ICommand EditImageCommand { get; private set; }

    public Action Close;

    public EditingARoomViewModel(Room room)
    {
        _room = room;
        _repos = new DbEFRepos();

        LoadTypesOfRoom();

        Init(room);

        SaveChangesCommand = new ViewModelCommand(ExecuteSaveChangesCommand, CanExecuteSaveChangesCommand);
        EditImageCommand = new ViewModelCommand(ExecuteEditImageCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    private bool CanExecuteSaveChangesCommand(object obj)
    {
        bool validDate;

        if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_description) || Image == null || decimal.Parse(Price) <= 0 || int.Parse(_numberOfPlaces) < 1)
            validDate = false;
        else
            validDate = true;

        return validDate;
    }

    private void ExecuteSaveChangesCommand(object obj)
    {
        if (CheckRoom(_repos))
        {
            ErrorMessage = "Такая комната уже сущесвует";
            return;
        }

        var roomInDb = _repos.Rooms.GetItem(_room.Id);

        UpdateRoom(roomInDb);

        Close?.Invoke();
    }

    private bool CheckRoom(IDbRepos repos)
    {
        var room = repos.Rooms.GetCollection().FirstOrDefault(a => a.Name == _name && a.Id != _room.Id);

        return room != null;
    }

    private void UpdateRoom(Room roomInDb)
    {
        if (roomInDb != null)
        {
            roomInDb.Name = _name;
            roomInDb.Description = _description;
            roomInDb.Price = decimal.Parse(_price);
            roomInDb.NumberOfPlaces = int.Parse(_numberOfPlaces);
            var type = GetTypeOfRoomByType(_selectedType, _repos);
            roomInDb.Type = type;

            if (_path != null)
                roomInDb.Image = GetImageBytes();

            _repos.Save();
        }
    }

    private TypeOfRoom GetTypeOfRoomByType(string type, IDbRepos repos)
    {
        if (repos == null)
            throw new NullReferenceException("An empty context was passed");

        return repos.Types.GetCollection().First(t => t.Type == type);
    }

    private void ExecuteEditImageCommand(object obj)
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

    private void ExecuteCancelCommand(object obj)
    {
        Close?.Invoke();
    }

    private void LoadTypesOfRoom()
    {
        Types = new ObservableCollection<string>(_repos.Types.GetCollection().Select(t => t.Type));
    }

    private void Init(Room room)
    {
        Name = room.Name;
        Description = room.Description;
        Price = room.Price.ToString();
        NumberOfPlaces = room.NumberOfPlaces.ToString();
        SelectedType = room.Type.Type;
        Image = ByteArrayToImageSource(room.Image);
    }

    private ImageSource ByteArrayToImageSource(byte[] byteArray)
    {
        if (byteArray == null || byteArray.Length == 0)
            return null;

        BitmapImage image = new BitmapImage();
        using (MemoryStream stream = new MemoryStream(byteArray))
        {
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
        }

        return image;
    }

    private void UploadImage(string filePath)
    {
        Image = new BitmapImage(new System.Uri(filePath));
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
