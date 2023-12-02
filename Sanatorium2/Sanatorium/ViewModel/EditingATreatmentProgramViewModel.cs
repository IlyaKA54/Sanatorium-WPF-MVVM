using Microsoft.Win32;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.ViewModel.Base;
using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sanatorium.ViewModel;

public class EditingATreatmentProgramViewModel : ViewModelBase
{
    private readonly TreatmentProgram _treatmentProgram;

    private string? _name;
    private string? _price;
    private string? _description;
    private ImageSource? _image;
    private string? _errorMessage;

    private string? _path;

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

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
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

    public EditingATreatmentProgramViewModel(TreatmentProgram treatmentProgram)
    {
        _treatmentProgram = treatmentProgram;

        Init(_treatmentProgram);

        SaveChangesCommand = new ViewModelCommand(ExecuteSaveChangesCommand, CanExecuteSaveChangesCommand);
        EditImageCommand = new ViewModelCommand(ExecuteEditImageCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    private bool CanExecuteSaveChangesCommand(object obj)
    {
        bool validDate;

        if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_description) || Image == null || decimal.Parse(Price) <= 0)
            validDate = false;
        else
            validDate = true;

        return validDate;
    }

    private void ExecuteCancelCommand(object obj)
    {
        Close?.Invoke();
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

    private void ExecuteSaveChangesCommand(object obj)
    {
        using (var context = new SanatoriumContext())
        {
            var customerInDb = context.TreatmentPrograms.Find(_treatmentProgram.Id);

            if (customerInDb != null)
            {
                customerInDb.Name = _name;
                customerInDb.Description = _description;
                customerInDb.Price = decimal.Parse(_price);

                if (_path != null)
                    customerInDb.Image = GetImageBytes();

                context.SaveChanges();
            }
        }

        Close?.Invoke();
    }

    private void Init(TreatmentProgram program)
    {
        Name = program.Name;
        Description = program.Description;
        Price = program.Price.ToString();
        Image = ByteArrayToImageSource(program.Image);
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
