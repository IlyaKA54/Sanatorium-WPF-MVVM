using FontAwesome.Sharp;
using Sanatorium.Model.Repositories;
using Sanatorium.ViewModel.Base;
using System.Windows.Input;

namespace Sanatorium.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;
        private IUserRepository _userRepository;

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

        public ICommand ShowRoomViewComand { get; }

        public MainViewModel()
        {
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);
            ShowRoomViewComand = new ViewModelCommand(ExecuteShowRoomViewCommand);
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
