using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace ASI.TCL.CMFT.WPF.Assist
{
    public static class ListBoxAssist
    {
        public static readonly DependencyProperty SelectedItemsProperty
            = DependencyProperty.RegisterAttached(
                "SelectedItems"
                , typeof(IList)
                , typeof(ListBoxAssist)
                , new PropertyMetadata(null, OnSelectedItemsChanged));
       
        public static void SetSelectedItems(DependencyObject element, IList value)
        {
            if (element != null) element.SetValue(SelectedItemsProperty, value);
        }
        
        public static IList GetSelectedItems(DependencyObject element) => (IList)element.GetValue(SelectedItemsProperty);

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox listBox)
            {
                if (listBox.SelectionMode == SelectionMode.Single)
                {
                    return;
                }
                listBox.SelectionChanged -= OnSelectionChanged;
                listBox.UnselectAll();
                if (e.NewValue is IList newList)
                {
                    var items = listBox.Items;
                    foreach (var item in newList)
                    {
                        var result = listBox.SelectedItems.Add(item);
                        if(result == -1)
                        {
                            throw new Exception($"{nameof(ListBoxAssist)} 的SelectedItems追加失敗 Type: + {item.GetType().Name}");
                        }
                    }
                }
                listBox.SelectionChanged += OnSelectionChanged;
            }
        }
        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            if (sender is ListBox listBox)
            {
                IList selectedItems = GetSelectedItems(listBox);
                if (selectedItems != null)
                {
                    selectedItems.Clear();
                    foreach ( var item in listBox.SelectedItems) 
                    { 
                        selectedItems.Add(item);
                    }
                }
            }
        }
    }
}