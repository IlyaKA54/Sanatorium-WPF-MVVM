namespace Sanatorium.Users;

public  abstract class UserRoleInfo
{
    private bool _isHomeButtonVisible;
    private bool _isCustomerButtonVisible;
    private bool _isReportButtonVisible;
    private bool _isRoomButtonVisible;
    private bool _isTreatmentProgramsButtonVisible;

    public bool IsHomeButtonVisible { get => _isHomeButtonVisible; }
    public bool IsCustomerButtonVisible { get => _isCustomerButtonVisible;}
    public bool IsReportButtonVisible { get => _isReportButtonVisible; }
    public bool IsRoomButtonVisible { get => _isRoomButtonVisible; }
    public bool IsTreatmentProgramsButtonVisible { get => _isTreatmentProgramsButtonVisible; }

    public UserRoleInfo(bool isHomeButtonVisible, bool isCustomerButtonVisible, bool isReportButtonVisible, bool isRoomButtonVisible, bool isTreatmentProgramsButtonVisible)
    {
        _isHomeButtonVisible = isHomeButtonVisible;
        _isCustomerButtonVisible = isCustomerButtonVisible;
        _isReportButtonVisible = isReportButtonVisible;
        _isRoomButtonVisible = isRoomButtonVisible;
        _isTreatmentProgramsButtonVisible = isTreatmentProgramsButtonVisible;
    }

}
