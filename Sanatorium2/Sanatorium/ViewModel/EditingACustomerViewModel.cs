﻿using Microsoft.Win32;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.ViewModel.Base;
using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sanatorium.ViewModel;

public class EditingACustomerViewModel : CustomerViewModelBase
{
    private readonly Customer _customer;

    private string? _errorMessage;
    private string? _filePath = null;

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

        Init(_customer);

        SaveChangesCommand = new ViewModelCommand(ExecuteSaveChangesCommand);
        EditImageCommand = new ViewModelCommand(ExecuteEditImageCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
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
        using (var context = new SanatoriumContext())
        {
            var customerInDb = context.Customers.Find(_customer.Id);

            if (customerInDb != null)
            {
                customerInDb.FirstName = FirstName;
                customerInDb.SecondName = SecondName;
                customerInDb.Surname = Surname;
                customerInDb.BirthDate = BirthDate;
                customerInDb.Phone = Phone;
                if(_filePath != null)
                    customerInDb.Image = GetImageBytes();
                context.SaveChanges();
            }
        }

        Close?.Invoke();
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