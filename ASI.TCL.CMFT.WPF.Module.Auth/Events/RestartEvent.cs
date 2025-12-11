using Prism.Events;

namespace ASI.TCL.CMFT.WPF.Module.Auth.Events
{
    /// <summary>
    /// 發佈「重新啟動程式」的事件，沒有負載資料（payload）。
    /// </summary>
    public class RestartEvent : PubSubEvent { }
}
