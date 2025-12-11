namespace ASI.TCL.CMFT.WPF.Module.Tetra.Dtos
{
  
    //無線電來電顯示
    public class TetraCallerDisplayDto 
    {
      
        //來電號碼
        public string PhoneNumber { get; set; }
        //基地台
        public string BaseStation { get; set; }
        //來電類型
        public eTetraCallType TetraCallType { get; set; }
        public string Id { get; set; }
    }
}
