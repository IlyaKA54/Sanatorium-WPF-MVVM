using System;
using System.Windows.Media;

namespace Sanatorium.ViewModel.Base
{
    public interface ICustomer
    {
        string? FirstName { get; set; }
        string? SecondName { get; set; }
        string? Surname { get; set; }
        string? Phone { get; set; }
        DateTime BirthDate { get; set; }
        ImageSource Image { get; set; }
    }
}
