using FontAwesome.Sharp;
using Sanatorium.Model.Repositories;
using Sanatorium.ViewModel.Base;
using System;
using System.Windows.Input;

namespace Sanatorium.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;
        private IUserRepository _userRepository;
        private bool _isHomeButtonVisible = true;
        private bool _isCustomerButtonVisible = false;
        private bool _isReportButtonVisible = false;
        private bool _isRoomButtonVisible = false;
        private bool _isTreatmentProgramsButtonVisible = true;

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged();
            }
        }
        public string Caption
        {
            get
            {
                return _caption;
            }

            set
            {
                _caption = value;
                OnPropertyChanged();
            }
        }
        public IconChar Icon
        {
            get
            {
                return _icon;
            }

            set
            {
                _icon = value;
                OnPropertyChanged();
            }
        }

        public bool IsHomeButtonVisible { get => _isHomeButtonVisible; set { _isHomeButtonVisible = value; OnPropertyChanged(); } }
        public bool IsCustomerButtonVisible { get => _isCustomerButtonVisible; set { _isCustomerButtonVisible = value; OnPropertyChanged(); } }
        public bool IsReportButtonVisible { get => _isReportButtonVisible; set { _isReportButtonVisible = value; OnPropertyChanged(); }  }
        public bool IsRoomButtonVisible { get => _isRoomButtonVisible; set { _isRoomButtonVisible = value; OnPropertyChanged(); } }
        public bool IsTreatmentProgramsButtonVisible { get => _isTreatmentProgramsButtonVisible; set { _isTreatmentProgramsButtonVisible = value; OnPropertyChanged(); } }

        public ICommand ShowHomeViewCommand { get; }

        public ICommand ShowCustomerViewCommand { get; }

        public ICommand ShowRoomViewCommand { get; }
        public ICommand ShowTreatmentProgramsViewCommand { get; }
        public ICommand ShowReportViewCommand { get; }

        public MainViewModel()
        {
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);
            ShowRoomViewCommand = new ViewModelCommand(ExecuteShowRoomViewCommand);
            ShowTreatmentProgramsViewCommand = new ViewModelCommand(ExecuteShowTreatmentProgramViewCommand);
            ShowReportViewCommand = new ViewModelCommand(ExecuteShowReportViewCommand);

            ExecuteShowHomeViewCommand(new object());
        }

        private void ExecuteShowReportViewCommand(object obj)
        {
            CurrentChildView = new ReportViewModel();
            Caption = "Отчет";
            Icon = IconChar.PieChart;
        }

        private void ExecuteShowTreatmentProgramViewCommand(object obj)
        {
            CurrentChildView = new TreatmentProgramViewModel();
            Caption = "Программы лечения";
            Icon = IconChar.HouseMedical;
        }

        private void ExecuteShowCustomerViewCommand(object obj)
        {
            CurrentChildView = new CustomersViewModel();
            Caption = "Клиенты";
            Icon = IconChar.UserGroup;
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Главная";
            Icon = IconChar.Home;
        }

        private void ExecuteShowRoomViewCommand(object obj)
        {
            CurrentChildView = new RoomsViewModel();
            Caption = "Комнаты";
            Icon = IconChar.Bed;
        }

    }
}
