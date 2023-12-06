using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sanatorium.ViewModel
{
    public class CustomerSelectionViewModel : ViewModelBase
    {
        private ObservableCollection<Customer> _allCustomers;
        private ObservableCollection<Customer> _selectedCustomers;

        public ObservableCollection<Customer> AllCustomers
        {
            get
            {
                return _allCustomers;
            }
            set
            {
                _allCustomers = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Customer> SelectedCustomers
        {
            get
            {
                return _selectedCustomers;
            }
            set
            {
                _selectedCustomers = value;
                OnPropertyChanged();
            }
        }

        public ICommand MoveACustomerCommand { get; private set; }
        public CustomerSelectionViewModel()
        {
            _allCustomers = new ObservableCollection<Customer>();
            _selectedCustomers = new ObservableCollection<Customer>();

            MoveACustomerCommand = new ViewModelCommand(ExecuteMoveACustomerCommand);

            LoadCustomers();
        }

        private void ExecuteMoveACustomerCommand(object obj)
        {
            if(obj is Customer customer)
            {
                if(AllCustomers.Contains(customer))
                {
                    AllCustomers.Remove(customer);
                    SelectedCustomers.Add(customer);
                }
                else
                {
                    SelectedCustomers.Remove(customer);
                    AllCustomers.Add(customer);
                }
            }
        }

        private void LoadCustomers(string? str = null)
        {
            using (var context = new SanatoriumContext())
            {
                AllCustomers = new ObservableCollection<Customer>(context.Customers.ToList());
            }
        }
    }
}
