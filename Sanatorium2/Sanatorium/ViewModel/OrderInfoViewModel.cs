using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Sanatorium.ViewModel;

public class OrderInfoViewModel : ViewModelBase
{
    private readonly IDbRepos _repos;

    public Order Order { get; private set; }
    public IEnumerable<CustomerOrder> CustomerOrders { get; private set; }
    public ICommand CloseCommand { get; private set; }

    public Action Close;
    public OrderInfoViewModel(Order order)
    {
        _repos = new DbEFRepos();

        Order = order;

        CloseCommand = new ViewModelCommand(ExecuteCloseCommand);

        LoadCustomerOrders(order);
    }

    private void ExecuteCloseCommand(object obj)
    {
        Close?.Invoke();
    }

    private void LoadCustomerOrders(Order order)
    {
        CustomerOrders = _repos.CustomerOrders.GetCollection().Where(a => a.IdOrder.Id == order.Id).ToList();
    }
}
