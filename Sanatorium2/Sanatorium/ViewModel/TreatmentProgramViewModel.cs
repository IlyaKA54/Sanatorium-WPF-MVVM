using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Sanatorium.ViewModel;

class TreatmentProgramViewModel : ViewModelBase
{
    private ObservableCollection<TreatmentProgram> _programs;
    private string? _searchText;

    private AdditionTreatmentProgramView _currentAdditionTreatmentProgramViewModel;
    private EditingATreatmentProgramView _activeEditingATreatmentProgramView;

    public string? SearchText
    {
        get
        {
            return _searchText;
        }

        set
        {
            _searchText = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<TreatmentProgram> Programs
    {
        get
        {
            return _programs;
        }
        set
        {
            _programs = value;
            OnPropertyChanged();
        }
    }

    public ICommand ShowAddTreatmentProgramCommand { get; private set; }
    public ICommand ShowEditWindowCommand { get; private set; }
    public ICommand RefreshCommand { get; private set; }
    public ICommand DeleteTreatmentProgramCommand { get; private set; }
    public TreatmentProgramViewModel()
    {
        _programs = new ObservableCollection<TreatmentProgram>();

        ShowAddTreatmentProgramCommand = new ViewModelCommand(ExecuteShowTreatmentProgramCommand);
        RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
        DeleteTreatmentProgramCommand = new ViewModelCommand(ExecuteDeleteTreatmentProgramCommand);
        ShowEditWindowCommand = new ViewModelCommand(ExecuteShowEditWindowCommand);

        LoadTreatmentPrograms();
    }

    private void ExecuteShowEditWindowCommand(object obj)
    {
        if (obj is TreatmentProgram program)
        {
            if (_activeEditingATreatmentProgramView == null || _activeEditingATreatmentProgramView.DataContext == null)
            {
                var editViewModel = new EditingATreatmentProgramViewModel(program);

                editViewModel.Close += CloseEditingATreatmentProgramView;

                _activeEditingATreatmentProgramView = new EditingATreatmentProgramView
                {
                    DataContext = editViewModel
                };

                _activeEditingATreatmentProgramView.Show();
            }
        }
    }

    private void CloseEditingATreatmentProgramView()
    {
        if (_activeEditingATreatmentProgramView != null)
        {
            _activeEditingATreatmentProgramView.Close();
            _activeEditingATreatmentProgramView = null;
            LoadTreatmentPrograms();
        }
    }

    private void ExecuteDeleteTreatmentProgramCommand(object obj)
    {
        if (obj is TreatmentProgram programToRemove)
        {
            Programs.Remove(programToRemove);

            using (var context = new SanatoriumContext())
            {
                context.TreatmentPrograms.Remove(programToRemove);
                context.SaveChanges();
            }
        }
    }

    private void ExecuteRefreshCommand(object obj)
    {
        LoadTreatmentPrograms(_searchText);
    }

    private void ExecuteShowTreatmentProgramCommand(object obj)
    {
        if (_currentAdditionTreatmentProgramViewModel == null)
        {
            _currentAdditionTreatmentProgramViewModel = new AdditionTreatmentProgramView();
            (_currentAdditionTreatmentProgramViewModel.DataContext as AdditionTreatmentProgramViewModel).Close += CloseAdditionalTreatmnetProgramWindow;
            _currentAdditionTreatmentProgramViewModel.Show();
        }
    }

    private void CloseAdditionalTreatmnetProgramWindow()
    {
        if (_currentAdditionTreatmentProgramViewModel != null)
        {
            _currentAdditionTreatmentProgramViewModel?.Close();
            _currentAdditionTreatmentProgramViewModel = null;
            LoadTreatmentPrograms();
        }
    }

    private void LoadTreatmentPrograms(string str = null)
    {
        using (var context = new SanatoriumContext())
        {
            if (str == null || string.IsNullOrEmpty(str))
                Programs = new ObservableCollection<TreatmentProgram>(context.TreatmentPrograms.ToList());
            else
                Programs = new ObservableCollection<TreatmentProgram>(context.TreatmentPrograms.Where(c => c.Name.StartsWith(str)));
        }
    }

}
