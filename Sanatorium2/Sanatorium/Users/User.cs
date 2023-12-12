namespace Sanatorium.Users;

public  abstract class User
{
    private bool _isHomeButtonVisible;
    private bool _isCustomerButtonVisible;
    private bool _isReportButtonVisible;
    private bool _isRoomButtonVisible;
    private bool _isTreatmentProgramsButtonVisible;

    public bool IsHomeButtonVisible { get => _isHomeButtonVisible; set { _isHomeButtonVisible = value; } }
    public bool IsCustomerButtonVisible { get => _isCustomerButtonVisible; set { _isCustomerButtonVisible = value; } }
    public bool IsReportButtonVisible { get => _isReportButtonVisible; set { _isReportButtonVisible = value; } }
    public bool IsRoomButtonVisible { get => _isRoomButtonVisible; set { _isRoomButtonVisible = value; } }
    public bool IsTreatmentProgramsButtonVisible { get => _isTreatmentProgramsButtonVisible; set { _isTreatmentProgramsButtonVisible = value;} }


    public User(bool isHomeButtonVisible, bool isCustomerButtonVisible, bool isReportButtonVisible, bool isRoomButtonVisible, bool isTreatmentProgramsButtonVisible)
    {
        _isHomeButtonVisible = isHomeButtonVisible;
        _isCustomerButtonVisible = isCustomerButtonVisible;
        _isReportButtonVisible = isReportButtonVisible;
        _isRoomButtonVisible = isRoomButtonVisible;
        _isTreatmentProgramsButtonVisible = isTreatmentProgramsButtonVisible;
    }

}
