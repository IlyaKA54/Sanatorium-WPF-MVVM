using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using Sanatorium.Users;
using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sanatorium.ViewModel;

class TreatmentProgramViewModel : ViewModelBase
{
    private ObservableCollection<TreatmentProgram> _programs;
    private string? _searchText;

    private Window _currentActiveWindow;
    private IDbRepos _repos;
    private UserRoleInfo _user;
    public UserRoleInfo User => _user;
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
    public TreatmentProgramViewModel(UserRoleInfo user)
    {
        ShowAddTreatmentProgramCommand = new ViewModelCommand(ExecuteShowTreatmentProgramCommand);
        RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
        DeleteTreatmentProgramCommand = new ViewModelCommand(ExecuteDeleteTreatmentProgramCommand);
        ShowEditWindowCommand = new ViewModelCommand(ExecuteShowEditWindowCommand);

        _user = user;

        LoadTreatmentPrograms();
    }

    private void ExecuteShowEditWindowCommand(object obj)
    {
        if (obj is TreatmentProgram program)
        {
            if (_currentActiveWindow == null || _currentActiveWindow.DataContext == null)
            {
                var editViewModel = new EditingATreatmentProgramViewModel(program);

                editViewModel.Close += CloseWindow;

                _currentActiveWindow = new EditingATreatmentProgramView
                {
                    DataContext = editViewModel
                };

                _currentActiveWindow.Show();
            }
        }
    }


    private void ExecuteDeleteTreatmentProgramCommand(object obj)
    {
        if (obj is TreatmentProgram programToRemove)
        {
            Programs.Remove(programToRemove);
            _repos.TreatmentPrograms.Delete(programToRemove);

            _repos.Save();
        }
    }

    private void ExecuteRefreshCommand(object obj)
    {
        LoadTreatmentPrograms(_searchText);
    }

    private void ExecuteShowTreatmentProgramCommand(object obj)
    {
        if (_currentActiveWindow == null)
        {
            _currentActiveWindow = new AdditionTreatmentProgramView();
            (_currentActiveWindow.DataContext as AdditionTreatmentProgramViewModel).Close += CloseWindow;
            _currentActiveWindow.Show();
        }
    }

    private void CloseWindow()
    {
        if (_currentActiveWindow != null)
        {
            _currentActiveWindow?.Close();
            _currentActiveWindow = null;
            LoadTreatmentPrograms();
        }
    }

    private void LoadTreatmentPrograms(string str = null)
    {
        _repos = new DbEFRepos();

        if (string.IsNullOrEmpty(str))
            Programs = new ObservableCollection<TreatmentProgram>(_repos.TreatmentPrograms.GetCollection());
        else
            Programs = new ObservableCollection<TreatmentProgram>(_repos.TreatmentPrograms.GetCollection().Where(c => c.Name.StartsWith(str)).ToList());
    }

}
