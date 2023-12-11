using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sanatorium.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private ObservableCollection<CustomerOrder> _orders;

        public ObservableCollection<CustomerOrder> Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowCustomerSelectionCommand { get; private set; }

        public HomeViewModel()
        {
            ShowCustomerSelectionCommand = new ViewModelCommand(ExecuteShowCustomerSelectionCommand);
            LoadOrders();
        }

        private void ExecuteShowCustomerSelectionCommand(object obj)
        {
            var newWindow = new CustomerSelectionView();
            newWindow.Show();
        }

        private void LoadOrders()
        {
            using (var context = new SanatoriumContext())
            {

                Orders = new ObservableCollection<CustomerOrder>(context.CustomerOrders.Include(co => co.IdOrder)
                    .Include(co => co.Room).Where(co => co.IdOrder.DateOfDeparture > DateTime.Now).ToList());

            }
        }
    }
}
