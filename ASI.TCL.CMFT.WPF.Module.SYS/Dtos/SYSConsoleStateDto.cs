namespace ASI.TCL.CMFT.WPF.Module.SYS.Dtos
{
    public class SYSConsoleStateDto
    {
        public SYSConsoleDto Console { get; set; }
        //public AccountDto Login {  get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DLTState {  get; set; }
        public string TetraState { get; set; }
        public string OTCSState {  get; set; }
        public string PAStation { get; set; }
    }
}