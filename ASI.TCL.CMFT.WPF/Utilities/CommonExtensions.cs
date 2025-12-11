using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Utilities
{
    public static class CommonExtensions
    {
        #region IEnumerable
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            foreach (T element in source) act(element);
            return source;
        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> enumerableList)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();



            foreach (PropertyDescriptor prop in properties)
            {
                var temp1 = Nullable.GetUnderlyingType(prop.PropertyType);
                Type temp2 = temp1 ?? prop.PropertyType;

                //因為enum最後print出來會變數字,所以不指定type,讓他變string
                if (temp2.IsEnum)
                {
                    table.Columns.Add(prop.Name);
                }
                else
                {
                    table.Columns.Add(prop.Name, temp2);
                }
            }

            foreach (T item in enumerableList)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }
            return table;
        }
        public static Dictionary<TValue, TKey> Swap<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerableList)
        {
            var dictionary = new Dictionary<TValue, TKey>();
            if (enumerableList != null)
            {
                foreach (var item in enumerableList)
                {
                    dictionary.Add(item.Value, item.Key);
                }
            }
            return dictionary;
        }
 
        public static ObservableCollection<T> ToObservableCollection <T>(this IEnumerable<T> enumerableList)
        {
            if (enumerableList != null)
            {
                // Create an emtpy observable collection object
                var observableCollection = new ObservableCollection<T>();

                // Loop through all the records and add to observable collection object
                foreach (var item in enumerableList)
                {
                    observableCollection.Add(item);
                }

                // Return the populated observable collection
                return observableCollection;
            }
            return null;
        }

        public static IEnumerable<T> Removes<T>(this ICollection<T> enumerableList,IEnumerable<T> removelist)
        {
            if (enumerableList != null)
            {
                foreach (var removeItem in removelist)
                {
                    enumerableList.Remove(removeItem);
                }
            }
            return null;
        }
        public static ObservableCollection<KeyValuePair<TKey, TValue>> ToObservableCollection<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerableList)
        {
            if (enumerableList != null)
            {
                // Create an emtpy observable collection object
                var observableCollection = new ObservableCollection<KeyValuePair<TKey, TValue>>();

                // Loop through all the records and add to observable collection object
                foreach (var item in enumerableList)
                {
                    observableCollection.Add(item);
                }

                // Return the populated observable collection
                return observableCollection;
            }
            return null;
        }
        #endregion

        #region String
        public static bool IsNumeric(this string @this)
        {
            return !Regex.IsMatch(@this, "[^0-9]");
        }
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        #endregion

        #region Color
      
        public static Color KnownColorToColor(this string @this)
        {
            return (Color)ColorConverter.ConvertFromString(@this);
        }
        public static string KnownColorToHex(this string @this)
        {
          
            return ((Color)ColorConverter.ConvertFromString(@this)).ColorToHex();
        }

        public static Color HexToColor(this string @this) 
        {
            if ((Color?)ColorConverter.ConvertFromString(@this) is Color color)
            {
                return color;
            }
            return new Color();
        }
        public static Color BrushToColor(this SolidColorBrush @this) 
        {
            return @this.Color;
        }
        public static SolidColorBrush HexToBrush(this string @this)
        {
            var color = @this.HexToColor();
            return new SolidColorBrush(color);
        }
        public static SolidColorBrush ColorToBrush(this Color @this) 
        {
            return new SolidColorBrush(@this);
        }
        public static string ColorToHex(this Color @this) 
        {
            string lowerHexString(int i) => i.ToString("X2").ToLower();
           
            var hex = lowerHexString(@this.R) +
                      lowerHexString(@this.G) +
                      lowerHexString(@this.B);
            return "#" + hex;
        }
        public static string BrushToHex(this SolidColorBrush @this) {

            string lowerHexString(int i) => i.ToString("X2").ToLower();
        
            var hex = lowerHexString(@this.Color.R) +
                      lowerHexString(@this.Color.G) +
                      lowerHexString(@this.Color.B);
            return "#" + hex;




        }
        #endregion

        #region Prism Parameters
        public static NavigationParameters ToNavigationParameters(this IDialogParameters dialogParameters)
        {
            var navigationParameters = new NavigationParameters();
            foreach (var parameterKey in dialogParameters.Keys)
            {
                navigationParameters.Add(parameterKey, dialogParameters.GetValue<object>(parameterKey));
            }
            return navigationParameters;
        }
        public static DialogParameters ToDialogParameters(this NavigationParameters navigationParameters)
        {
            var dialogParameters = new DialogParameters();
            foreach (var parameter in navigationParameters)
            {
                dialogParameters.Add(parameter.Key, parameter.Value);
            }
            return dialogParameters;
        }
        #endregion
    }
}