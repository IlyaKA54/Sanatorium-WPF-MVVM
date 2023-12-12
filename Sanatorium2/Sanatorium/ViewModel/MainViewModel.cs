using FontAwesome.Sharp;
using Sanatorium.Model.Repositories;
using Sanatorium.Users;
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

        private User _user;

        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }


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
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowCustomerViewCommand { get; }
        public ICommand ShowRoomViewCommand { get; }
        public ICommand ShowTreatmentProgramsViewCommand { get; }
        public ICommand ShowReportViewCommand { get; }

        public MainViewModel(User user)
        {
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);
            ShowRoomViewCommand = new ViewModelCommand(ExecuteShowRoomViewCommand);
            ShowTreatmentProgramsViewCommand = new ViewModelCommand(ExecuteShowTreatmentProgramViewCommand);
            ShowReportViewCommand = new ViewModelCommand(ExecuteShowReportViewCommand);
            User = user;
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
