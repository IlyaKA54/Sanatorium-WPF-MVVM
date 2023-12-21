using System.Windows.Media;
using System;

namespace Sanatorium.ViewModel.Base;

public abstract class CustomerViewModelBase: ViewModelBase,ICustomer
{
    private string? _firstName;
    private string? _secondName;
    private string? _surname;
    private string? _phone;
    private int _numberOfVisits;
    private DateTime _birthDate { get; set; } = DateTime.Today;

    private ImageSource _image;

    public string? FirstName
    {
        get
        {
            return _firstName;

        }
        set
        {
            _firstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }
    public string? SecondName
    {
        get
        {
            return _secondName;
        }
        set
        {
            _secondName = value;
            OnPropertyChanged(nameof(SecondName));
        }
    }
    public string? Surname
    {
        get
        {
            return _surname;
        }
        set
        {
            _surname = value;
            OnPropertyChanged(nameof(Surname));
        }
    }
    public string? Phone
    {
        get
        {
            return _phone;
        }
        set
        {
            _phone = value;
            OnPropertyChanged(nameof(Phone));
        }
    }

    public int NumberOfVisits
    {
        get
        {
            return _numberOfVisits;
        }
        set
        {
            _numberOfVisits = value;
            OnPropertyChanged(nameof(Phone));
        }
    }
    public DateTime BirthDate
    {
        get
        {
            return _birthDate;
        }
        set
        {
            if (value.Year < 1900 || value > DateTime.Now)
                return;
            _birthDate = value;
            OnPropertyChanged(nameof(BirthDate));
        }
    }

    public ImageSource Image
    {
        get
        {
            return _image;
        }
        set
        {
            _image = value;
            OnPropertyChanged(nameof(Image));
        }
    }
}


