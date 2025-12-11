namespace ASI.TCL.CMFT.WPF.Module.OTCS.Dtos
{
    //旅客緊急通訊來電顯示
    public class PICallerDisplayDto 
    {
        public OTCSTarinDto Train { get; set; }

        //列車車廂
        public string Car { get; set; }

        //旅客緊急通訊對講機位置
        public string PINumber { get; set; }
        
        //基地台
        public string Source { get; set; }
        public string Id { get; set; }
    }
}
