namespace ASI.TCL.CMFT.WPF.Module.SYS.Dtos
{
    public class AccountDto 
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string Description { get; set; }
        public RoleDto RoleDto { get; set; }
    }
}
