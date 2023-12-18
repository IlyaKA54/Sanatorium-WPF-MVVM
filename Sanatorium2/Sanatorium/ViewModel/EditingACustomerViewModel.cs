using Microsoft.Win32;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using Sanatorium.ViewModel.Base;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sanatorium.ViewModel;

public class EditingACustomerViewModel : CustomerViewModelBase
{
    private readonly Customer _customer;

    private string? _errorMessage;
    private string? _filePath = null;
    private IDbRepos _repos;

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

    public EditingACustomerViewModel(Customer customerToEdit)
    {
        _customer = customerToEdit;
        _repos = new DbEFRepos();

        Init(_customer);

        SaveChangesCommand = new ViewModelCommand(ExecuteSaveChangesCommand, CanExecuteSaveChangesCommand);
        EditImageCommand = new ViewModelCommand(ExecuteEditImageCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    private bool CanExecuteSaveChangesCommand(object obj)
    {
        bool validDate;

        if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(SecondName) ||
            string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(Phone) || Image == null)
            validDate = false;
        else
            validDate = true;

        return validDate;
    }

    private void Init(Customer customerToEdit)
    {
        SecondName = customerToEdit.SecondName;
        FirstName = customerToEdit.FirstName;
        Surname = customerToEdit.Surname;
        BirthDate = customerToEdit.BirthDate;
        Phone = customerToEdit.Phone;
        Image = ByteArrayToImageSource(customerToEdit.Image);
    }

    private void ExecuteEditImageCommand(object obj)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "Image files|*.bmp;*.jpg;*.png;*.jpeg;";
        fileDialog.FilterIndex = 1;

        if (fileDialog.ShowDialog() == true)
        {
            _filePath = fileDialog.FileName;
            UploadImage(_filePath);
        }
    }

    private void UploadImage(string filePath)
    {
        Image = new BitmapImage(new System.Uri(filePath));
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

    private void ExecuteSaveChangesCommand(object obj)
    {
        if (CheckCustomer(_repos))
        {
            ErrorMessage = "Такой клиент уже есть";
            return;
        }

        var customerInDb = _repos.Customers.GetItem(_customer.Id);

        UpdateCustomer(customerInDb);

        Close?.Invoke();
    }

    private void UpdateCustomer(Customer customerInDb)
    {
        if (customerInDb != null)
        {
            customerInDb.FirstName = FirstName;
            customerInDb.SecondName = SecondName;
            customerInDb.Surname = Surname;
            customerInDb.BirthDate = BirthDate;
            customerInDb.Phone = Phone;
            if (_filePath != null)
                customerInDb.Image = GetImageBytes();

            _repos.Save();
        }
    }

    private bool CheckCustomer(IDbRepos repos)
    {
        var customer = repos.Customers.GetCollection()
            .FirstOrDefault(a => a.FirstName == FirstName &&
            a.SecondName == SecondName && a.Surname == Surname &&
            a.Phone == Phone && a.BirthDate == BirthDate && a.Id != _customer.Id);

        return customer != null;
    }

    private byte[] GetImageBytes()
    {
        using (FileStream fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }

    private void ExecuteCancelCommand(object obj)
    {
        Close?.Invoke();
    }
}
