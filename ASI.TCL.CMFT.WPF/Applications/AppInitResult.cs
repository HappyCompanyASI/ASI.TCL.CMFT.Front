namespace ASI.TCL.CMFT.WPF.Applications
{
    public class AppInitResult(bool isEntryable, string entryMessage)
    {
        public bool IsEntryable { get; set; } = isEntryable;
        public string EntryMessage { get; set; } = entryMessage;
    }
}