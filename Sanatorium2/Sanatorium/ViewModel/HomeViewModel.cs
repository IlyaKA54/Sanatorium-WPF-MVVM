using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sanatorium.ViewModel;

public class HomeViewModel : ViewModelBase
{
    private ObservableCollection<Order> _orders;
    private IDbRepos _repos;

    public ObservableCollection<Order> Orders
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
    public ICommand ShowInfoWindowCommand { get; private set; }

    public HomeViewModel()
    {
        ShowCustomerSelectionCommand = new ViewModelCommand(ExecuteShowCustomerSelectionCommand);
        ShowInfoWindowCommand = new ViewModelCommand(ExecuteShowInfoWindowCommand);

        _repos = new DbEFRepos();
        LoadOrders();
    }

    private void ExecuteShowInfoWindowCommand(object obj)
    {
        if(obj is Order order)
        {
            var newWindow = new OrderInfoView(new OrderInfoViewModel(order));
            newWindow.Show();
        }
    }

    private void ExecuteShowCustomerSelectionCommand(object obj)
    {
        var newWindow = new CustomerSelectionView();
        newWindow.Show();
    }

    private void LoadOrders()
    {
        var currentDate = DateTime.Now;

        var orders = _repos.Orders
            .GetCollection()
            .Where(co => co.DateOfDeparture >= currentDate && co.ArrivalDate <= currentDate)
            .ToList();

        Orders = new ObservableCollection<Order>(orders);
    }
}
