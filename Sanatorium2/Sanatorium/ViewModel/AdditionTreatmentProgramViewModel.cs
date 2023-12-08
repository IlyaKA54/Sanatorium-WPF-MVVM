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

class AdditionTreatmentProgramViewModel : ViewModelBase
{
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

    public Action Close;

    public ICommand AddProgramCommand { get; private set; }
    public ICommand UploadImageCommand { get; private set; }
    public ICommand CloseWindowCommand { get; private set; }

    public AdditionTreatmentProgramViewModel()
    {
        AddProgramCommand = new ViewModelCommand(ExecuteAddProgramCommand, CanExecuteAddProgramCommand);
        UploadImageCommand = new ViewModelCommand(ExecuteUploadImageCommand);
        CloseWindowCommand = new ViewModelCommand(ExecuteCloseWindowCommand);
    }

    private void ExecuteCloseWindowCommand(object obj)
    {
        Close?.Invoke();
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

    private TreatmentProgram GetTreamentProgramm()
    {
        var newTreatmentProgram = new TreatmentProgram()
        {
            Name = this.Name,
            Price = decimal.Parse(this.Price),
            Description = this.Description,
            Image = GetImageBytes(),
        };

        return newTreatmentProgram;
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

    private bool CanExecuteAddProgramCommand(object obj)
    {
        bool validDate;

        if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_description) || Image == null || decimal.Parse(Price) <= 0)
            validDate = false;
        else
            validDate = true;

        return validDate;
    }

    private void ExecuteAddProgramCommand(object obj)
    {
        using (var context = new SanatoriumContext())
        {
            context.TreatmentPrograms.Add(GetTreamentProgramm());
            context.SaveChanges();
            Close?.Invoke();
        }
    }
}
