namespace ASI.TCL.CMFT.WPF.Module.DLTS.Dtos
{
    public class DLTPhoneDto 
    {
        public string PhoneNumber { get; set; }
        public string PhoneLocation { get; set; }
        public DLTPhoneGroupDto BelongGroup { get; set; }
        public string Id { get; set; }
    }
}
