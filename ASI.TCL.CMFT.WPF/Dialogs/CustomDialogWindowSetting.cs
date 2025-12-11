using System;
using System.Collections.Generic;
using System.Windows;

namespace ASI.TCL.CMFT.WPF.Dialogs
{
    public class CustomDialogWindowSetting : Dictionary<string, object>
    {
        internal void ApplyTo(Window window)
        {
            foreach (var setting in this)
            {
                var propertyInfo = window.GetType().GetProperty(setting.Key);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    try
                    {
                        var value = Convert.ChangeType(setting.Value, propertyInfo.PropertyType);
                        propertyInfo.SetValue(window, value);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Failed to apply setting '{setting.Key}' with value '{setting.Value}'", ex);
                    }
                }
            }
        }
    }
}