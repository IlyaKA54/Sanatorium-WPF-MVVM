using Microsoft.Win32;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
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

    public ObservableCollection<String> Types
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
        _types = new ObservableCollection<string>();
        AddRoomCommand = new ViewModelCommand(ExecuteAddRoomCommand, CanExecuteAddRoomCommand);
        UploadImageCommand = new ViewModelCommand(ExecuteUploadImageCommand);
        CloseWindowCommand = new ViewModelCommand(ExecuteCloseWindowCommand);
        LoadTypesOfRoom();
    }

    private void ExecuteCloseWindowCommand(object obj)
    {
        Close?.Invoke();
    }

    private bool CanExecuteAddRoomCommand(object obj)
    {
        bool validDate;

        if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_description) || Image == null || decimal.Parse(Price) <= 0 || int.Parse(_numberOfPlaces) < 1)
            validDate = false;
        else
            validDate = true;

        return validDate;
    }

    private void ExecuteAddRoomCommand(object obj)
    {
        using (var context = new SanatoriumContext())
        {
            context.Rooms.Add(GetNewRoom(context));
            context.SaveChanges();
            Close?.Invoke();
        }
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

    private Room GetNewRoom(SanatoriumContext context)
    {
        if (context == null)
            throw new NullReferenceException("An empty context was passed");

        Room room = new Room()
        {
            Name = this.Name,
            Price = decimal.Parse(this.Price),
            NumberOfPlaces = int.Parse(this.NumberOfPlaces),
            Description = this.Description,
            Image = GetImageBytes(),
            Type = GetTypeOfRoomByType(_selectedType, context)
        };

        return room;
    }

    private TypeOfRoom GetTypeOfRoomByType(string type, SanatoriumContext context)
    {
        if (context == null)
            throw new NullReferenceException("An empty context was passed");

        return context.TypeOfRooms.FirstOrDefault(t => t.Type == type);
    }
    private void LoadTypesOfRoom()
    {
        using (var context = new SanatoriumContext())
        {

            Types = new ObservableCollection<string>(context.TypeOfRooms.Select(t => t.Type).ToList());
        }
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
