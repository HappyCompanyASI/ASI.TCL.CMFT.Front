namespace ASI.TCL.CMFT.WPF.Dialogs
{
    //7個按鈕 6種組合
    public enum eDialogFooterButtons
    {
        //訊息方塊包含[中止]、[重試] 和[忽略] 按鈕
        AbortRetryIgnore,
        //訊息方塊包含[確定] 按鈕
        OK,
        //訊息方塊包含[確定] 和[取消] 按鈕
        OKCancel,
        //訊息方塊包含[重試] 和[取消] 按鈕
        RetryCancel,
        //訊息方塊包含[是] 和[否] 按鈕
        YesNo,
        //訊息方塊包含 [是]、[否] 和 [取消] 按鈕
        YesNoCancel
    }
}