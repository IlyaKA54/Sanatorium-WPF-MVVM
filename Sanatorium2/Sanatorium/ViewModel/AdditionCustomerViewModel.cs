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
using System.Windows.Media.Imaging;

namespace Sanatorium.ViewModel
{
    public class AdditionCustomerViewModel : CustomerViewModelBase
    {
        private string? _errorMessage;
        private string _filePath;
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

        public ICommand AddCommand { get; private set; }
        public ICommand UploadImageCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }


        public Action Close;
        public AdditionCustomerViewModel()
        {
            AddCommand = new ViewModelCommand(ExecuteAddCustomerCommand, CanExecuteAddCustomerCommand);
            UploadImageCommand = new ViewModelCommand(ExecuteUploadImageCommand);
            CloseCommand = new ViewModelCommand(ExecuteCloseCommand);
            _repos = new DbEFRepos();
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

            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(SecondName) ||
                string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(Phone) || Image == null)
                validDate = false;
            else
                validDate = true;

            return validDate;
        }

        private void ExecuteAddCustomerCommand(object obj)
        {
            if(CheckCustomer(_repos))
            {
                ErrorMessage = "Такой клиент уже есть";
                return;
            }

            _repos.Customers.Create(GetNewCustomer());
            _repos.Save();

            Close?.Invoke();
        }

        private bool CheckCustomer(IDbRepos repos)
        {
            var customer = repos.Customers.GetCollection()
                .FirstOrDefault(a => a.FirstName == FirstName && 
                a.SecondName == SecondName && a.Surname == Surname && 
                a.Phone == Phone && a.BirthDate == BirthDate);

            return customer != null;
        }

        private Customer GetNewCustomer()
        {
            var newCustomer = new Customer
            {
                FirstName = this.FirstName,
                SecondName = this.SecondName,
                Surname = this.Surname,
                BirthDate = this.BirthDate,
                Phone = this.Phone,
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
