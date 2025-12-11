using System;

namespace ASI.TCL.CMFT.WPF.Applications
{
    public interface ICloseWindow
    {
        Action Close { get; set; }
        bool? DialogResult { get; set; }
        bool CanClose();
    }
}