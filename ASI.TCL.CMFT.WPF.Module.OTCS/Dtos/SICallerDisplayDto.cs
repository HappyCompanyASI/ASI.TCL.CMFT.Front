namespace ASI.TCL.CMFT.WPF.Module.OTCS.Dtos
{
    //司機員通訊來電顯示
    public class SICallerDisplayDto 
    {
        public OTCSTarinDto Train { get; set; }
        //列車車廂
        public string Car { get; set; }

        public string CommLocation => Car;
        
        //基地台
        public string BaseStation { get; set; }
        public string Id { get; set; }
    }
}
