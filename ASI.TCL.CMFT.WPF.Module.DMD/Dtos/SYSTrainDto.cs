namespace ASI.TCL.CMFT.WPF.Module.DMD.Dtos
{
    public class SYSTrainDto 
    {
        public string Id { get; set; }
        public string TrainNumber { get; set; }
        public SYSTrainGroupDto BelongGroup { get; set; }
    }
}