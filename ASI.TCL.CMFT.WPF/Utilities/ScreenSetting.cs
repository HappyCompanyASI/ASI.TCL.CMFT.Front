using System.Drawing;
using System.Linq;
using System.Windows;

namespace ASI.TCL.CMFT.WPF.Utilities
{
    public static class ScreenSetting
    {
        public static void SetScreen(bool isPrimary, Window window)
        {
            var allScreens = System.Windows.Forms.Screen.AllScreens;
            Rectangle ScreenArea;

            if (allScreens.Length > 1)
            {
                var screen = allScreens.Single(x => x.Primary == isPrimary);
                ScreenArea = screen.WorkingArea;
            }
            else
            {
                var screen = System.Windows.Forms.Screen.AllScreens.Single(x => x.Primary);
                ScreenArea = screen.WorkingArea;
            }

            WindowState windowState = window.WindowState;
            double windowWidth = window.Width;
            double windowHeight = window.Height;
            window.WindowState = WindowState.Minimized;
            window.Left = ScreenArea.Left + (ScreenArea.Width - windowWidth) * 0.5;
            window.Top = ScreenArea.Top + (ScreenArea.Height - windowHeight) * 0.5;

            window.Loaded += (_, _) =>
            {
                window.WindowState = windowState;
            };
        }
    }
}