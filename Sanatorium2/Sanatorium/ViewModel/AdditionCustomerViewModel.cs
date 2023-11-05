using Microsoft.Win32;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sanatorium.ViewModel
{
    public class AdditionCustomerViewModel : ViewModelBase
    {
        private string? _firstName;
        private string? _secondName;
        private string? _surname;
        private string? _phone;
        private DateTime _birthDate { get; set; } = DateTime.Today;
        private string? _errorMessage;

        private ImageSource _image;
        //private AdditionCustomerView _additionCustomerView;
        private string _filePath;
        public string? FirstName
        {
            get
            {
                return _firstName;

            }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string? SecondName
        {
            get
            {
                return _secondName;
            }
            set
            {
                _secondName = value;
                OnPropertyChanged();
            }
        }
        public string? Surname
        {
            get
            {
                return _surname;
            }
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }
        public string? Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }
        public DateTime BirthDate
        {
            get
            {
                return _birthDate;
            }
            set
            {
                if (value.Year < 1900 || value > DateTime.Now)
                    return;
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
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

        public ICommand AddCommand { get; private set; }
        public ICommand UploadImageCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }


        public Action Close;
        public AdditionCustomerViewModel()
        {
            AddCommand = new ViewModelCommand(ExecuteAddCustomerCommand, CanExecuteAddCustomerCommand);
            UploadImageCommand = new ViewModelCommand(ExecuteUploadImageCommand);
            CloseCommand = new ViewModelCommand(ExecuteCloseCommand);
            //_additionCustomerView = additionCustomerView;
        }

        private void ExecuteCloseCommand(object obj) => Close?.Invoke();

        private void ExecuteUploadImageCommand(object obj)
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

        private bool CanExecuteAddCustomerCommand(object obj)
        {
            bool validDate;

            if (string.IsNullOrEmpty(_firstName) || string.IsNullOrEmpty(_secondName) ||
                string.IsNullOrEmpty(_surname) || string.IsNullOrEmpty(_phone) || _image == null)
                validDate = false;
            else
                validDate = true;

            return validDate;
        }

        private void ExecuteAddCustomerCommand(object obj)
        {
            using (var context = new SanatoriumContext())
            {
                context.Customers.Add(GetNewCustomer());
                context.SaveChanges();
            }

            ExecuteCloseCommand(new object());

        }

        private Customer GetNewCustomer()
        {
            var newCustomer = new Customer
            {
                FirstName = _firstName,
                SecondName = _secondName,
                Surname = _surname,
                BirthDate = _birthDate,
                Phone = _phone,
                NumberOfVisits = 0,
                Image = GetImageBytes(),
            };

            return newCustomer;

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

    }
}
