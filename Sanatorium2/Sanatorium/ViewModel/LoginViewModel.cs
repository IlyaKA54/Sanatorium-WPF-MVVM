using Sanatorium.ViewModel.Base;
using System.Windows.Input;
using System.Net;
using System.Threading;
using System.Security.Principal;
using Sanatorium.Model.Repositories;
using System;
using Sanatorium.Users;

namespace Sanatorium.ViewModel;

public class LoginViewModel : ViewModelBase
{
    private string _username;
    private string _password;
    private string _errorMessage;
    private bool _isViewVisible = true;
    private IUserRepository _userRepository;

    public string Username
    {
        get
        {
            return _username;
        }
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get
        {
            return _password;
        }
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public string ErrorMessage
    {
        get
        {
            return _errorMessage;
        }
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public bool IsViewVisible
    {
        get
        {
            return _isViewVisible;
        }
        set
        {
            _isViewVisible = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoginCommand { get; private set; }

    public Action Close;

    public LoginViewModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);

    }

    private bool CanExecuteLoginCommand(object obj)
    {
        bool validDate;

        if (string.IsNullOrEmpty(Username) || Password == null)
            validDate = false;
        else
            validDate = true;

        return validDate;
    }

    private void ExecuteLoginCommand(object obj)
    {
        var isValidUser = _userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
        User user;

        if(isValidUser)
        {
            
            user = GetUser();
            var mainView = new MainWindow { DataContext = new MainViewModel(user) };
            mainView.Show();
            Close?.Invoke();
        }

        else
        {
            ErrorMessage = "* Неправильное имя пользователя или пароль";
        }
    }

    private User GetUser()
    {
        switch (Username)
        {
            case "admin":
                return new Admin();
            case "recept":
                return new Receptionist();
            case "clean":
                return new Cleaner();
            default:
                return null;
        }
    }
}
