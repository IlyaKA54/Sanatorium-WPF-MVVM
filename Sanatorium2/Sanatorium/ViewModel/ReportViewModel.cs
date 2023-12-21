using CsvHelper.Configuration;
using CsvHelper;
using LiveCharts;
using LiveCharts.Wpf;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;
using System.Text;

namespace Sanatorium.ViewModel;

public class ReportViewModel : ViewModelBase
{
    private ObservableCollection<TreatmentProgram> _programs;
    private SeriesCollection _seriesCollection;
    private DateTime _firstDay = DateTime.Now.AddDays(-1);
    private DateTime _secondDate = DateTime.Now.AddYears(1);
    private Dictionary<string, int> _peopleCountByProgram;

    private IDbRepos _repos;
    private string _filePath;

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
    public ICommand CreateReportCommand { get; private set; }

    public ReportViewModel()
    {
        RefreshDataCommand = new ViewModelCommand(ExecuteRefreshDataCommand, CanExecuteRefreshDataCommand);
        CreateReportCommand = new ViewModelCommand(ExecuteCreateReportCommand);
        _repos = new DbEFRepos();
        LoadOrder();
    }

    private void ExecuteCreateReportCommand(object obj)
    {
        _filePath = OpenFolderDialog();

        if (!string.IsNullOrEmpty(_filePath))
        {
            string filePath = Path.Combine(_filePath, "output.csv");

            GenerateCsvReport(_peopleCountByProgram, filePath);
        }
    }

    private bool CanExecuteRefreshDataCommand(object obj)
    {
        if (_firstDay >= _secondDate)
            return false;

        return true;
    }

    private void ExecuteRefreshDataCommand(object obj)
    {
        LoadOrder();
    }

    public void LoadOrder()
    {
        LoadPrograms();

        var customerOrders = GetFilteredCustomerOrders();

        _peopleCountByProgram = CountPeopleByProgram(customerOrders);

        PopulateSeriesCollection(_peopleCountByProgram);
    }

    private void LoadPrograms()
    {
        Programs = new ObservableCollection<TreatmentProgram>(_repos.TreatmentPrograms.GetCollection());
    }

    private List<CustomerOrder> GetFilteredCustomerOrders()
    {
        return _repos.CustomerOrders
            .GetCollection()
            .Where(co => co.IdOrder.ArrivalDate >= FirstDate && co.IdOrder.DateOfDeparture <= SecondDate)
            .ToList();
    }

    private Dictionary<string, int> CountPeopleByProgram(List<CustomerOrder> customerOrders)
    {
        var peopleCountByProgram = new Dictionary<string, int>();

        foreach (var order in customerOrders)
        {
            var programName = order.TreatmentProgram?.Name;

            if (!string.IsNullOrEmpty(programName))
            {
                if (peopleCountByProgram.ContainsKey(programName))
                    peopleCountByProgram[programName]++;
                else
                    peopleCountByProgram[programName] = 1;

            }
        }

        return peopleCountByProgram;
    }

    private void PopulateSeriesCollection(Dictionary<string, int> peopleCountByProgram)
    {
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

    public string OpenFolderDialog()
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Выберите папку",
            Filter = "Все файлы (*.*)|*.*",
            CheckFileExists = false,
            CheckPathExists = true,
            FileName = "Папка выбора"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            return Path.GetDirectoryName(openFileDialog.FileName);
        }

        return null;
    }

    public static void GenerateCsvReport(Dictionary<string, int> data, string filePath)
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            ShouldUseConstructorParameters = _ => false // отключаем создание конструктора для анонимных типов
        };

        using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, csvConfig))
        {
            csv.WriteRecords(data.Select(item => new { Name = item.Key, Count = item.Value }));
        }
    }
}
