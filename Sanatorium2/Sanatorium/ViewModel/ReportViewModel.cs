using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sanatorium.ViewModel;

public class ReportViewModel : ViewModelBase
{
    private ObservableCollection<TreatmentProgram> _programs;
    private SeriesCollection _seriesCollection;
    private DateTime _firstDay = DateTime.Now.AddDays(-1);
    private DateTime _secondDate = DateTime.Now.AddYears(1);

    public DateTime FirstDate
    {
        get
        {
            return _firstDay;
        }
        set
        {
            _firstDay = value;
            OnPropertyChanged();
        }
    }

    public DateTime SecondDate
    {
        get
        {
            return _secondDate;
        }
        set
        {
            _secondDate = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<TreatmentProgram> Programs
    {
        get { return _programs; }
        set
        {
            _programs = value;
            OnPropertyChanged(nameof(Programs));
        }
    }

    public SeriesCollection SeriesCollection
    {
        get { return _seriesCollection; }
        set
        {
            _seriesCollection = value;
            OnPropertyChanged(nameof(SeriesCollection));
        }
    }

    public ICommand RefreshDataCommand { get; private set; }

    public ReportViewModel()
    {
        RefreshDataCommand = new ViewModelCommand(ExecuteRefreshDataCommand, CanExecuteRefreshDataCommand);

        Load();
    }

    private bool CanExecuteRefreshDataCommand(object obj)
    {
        if(_firstDay >= _secondDate) 
            return false;

        return true;
    }

    private void ExecuteRefreshDataCommand(object obj)
    {
        Load();
    }

    public void Load()
    {
        using (var context = new SanatoriumContext())
        {
            Programs = new ObservableCollection<TreatmentProgram>(context.TreatmentPrograms.ToList());

            var customerOrders = context.CustomerOrders
            .Include(co => co.TreatmentProgram)
            .Include(co => co.IdOrder)
            .Where(co => co.IdOrder.ArrivalDate >= FirstDate && co.IdOrder.DateOfDeparture <= SecondDate)
            .ToList();

            var peopleCountByProgram = new Dictionary<string, int>();

            foreach (var order in customerOrders)
            {
                var programName = order.TreatmentProgram?.Name;

                if (!string.IsNullOrEmpty(programName))
                {
                    if (peopleCountByProgram.ContainsKey(programName))
                    {
                        peopleCountByProgram[programName]++;
                    }
                    else
                    {
                        peopleCountByProgram[programName] = 1;
                    }
                }
            }

            SeriesCollection = new SeriesCollection();
            foreach (var program in Programs)
            {
                if (peopleCountByProgram.TryGetValue(program.Name, out var count))
                {
                    SeriesCollection.Add(new PieSeries
                    {
                        Title = program.Name,
                        Values = new ChartValues<int> { count }
                    });
                }
            }
        }
    }
}
