using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MaterialDesignColors;

namespace ASI.TCL.CMFT.WPF.CustomControl
{
    [TemplatePart(Name = HexTextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = ColorItemsControlPartName, Type = typeof(ItemsControl))]
    [TemplatePart(Name = ColorPickerName, Type = typeof(MaterialDesignThemes.Wpf.ColorPicker))]
    [TemplatePart(Name = TabControlName, Type = typeof(TabControl))]
    public class ColorTool : Control
    {
        public static IEnumerable<ISwatch> Swatches { get; }  = SwatchHelper.Swatches;
        private DispatcherTimer _debounceTimer;

        private const string TabControlName = "PART_TabControl";
        private const string HexTextBoxPartName = "PART_TextBlock";
        private const string ColorItemsControlPartName = "PART_ItemsControl";
        private const string ColorPickerName = "PART_ColorPicker";

        private const string RTextBoxColorName = "PART_RColorTextBox";
        private const string GTextBoxColorName = "PART_GColorTextBox";
        private const string BTextBoxColorName = "PART_BColorTextBox";
        private const string RSliderColorName = "PART_RColorSlider";
        private const string GSliderColorName = "PART_GColorSlider";
        private const string BSliderColorName = "PART_BColorSlider";
        
        public static readonly RoutedCommand ChangeSelectedColorCommand = new RoutedCommand();

        static ColorTool()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorTool), new FrameworkPropertyMetadata(typeof(ColorTool)));
        }
        public ColorTool()
        {
            CommandBindings.Add(new CommandBinding(ChangeSelectedColorCommand, ChangeSelectedColorHandler));
        }

        private TabControl _tabControl;
        private TextBox _textblock;
        private ItemsControl _listbox;
        private MaterialDesignThemes.Wpf.ColorPicker _mdColorPicker;
        private Slider _rSlider;
        private Slider _gSlider;
        private Slider _bSlider;
        private TextBox _rTextbox;
        private TextBox _gTextbox;
        private TextBox _bTextbox;
     
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            nameof(SelectedColor)
            , typeof(Color?)
            , typeof(ColorTool)
            , new FrameworkPropertyMetadata(default(Color?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));
      
        public Color? SelectedColor
        {
            get => (Color?)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
        private static void OnSelectedColorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is not ColorTool colorPicker)
                return;

            // 設置 debounce timer 避免 UI 卡頓
            if (colorPicker._debounceTimer == null)
            {
                colorPicker._debounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
                colorPicker._debounceTimer.Tick += (s, args) =>
                {
                    colorPicker._debounceTimer.Stop();
                    colorPicker.RaiseEvent(new RoutedEventArgs(SelectionChangedEvent));
                };
            }

            // 每次變更選擇顏色時重置 timer
            colorPicker._debounceTimer.Stop();
            colorPicker._debounceTimer.Start();
        }
        
        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent(
            name: "SelectionChanged",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(ColorTool));
        public event RoutedEventHandler SelectionChanged
        {
            add => AddHandler(SelectionChangedEvent, value);
            remove => RemoveHandler(SelectionChangedEvent, value);
        }

        public override void OnApplyTemplate()
        {
            if (_tabControl != null) _tabControl.SelectionChanged -= TabControl_SelectionChanged;
            if (_textblock != null){}
            if (_listbox != null){}
            if (_mdColorPicker != null){}
            if (_rSlider != null) _rSlider.ValueChanged -= OnRValueChanged;
            if (_gSlider != null) _gSlider.ValueChanged -= OnGValueChanged;
            if (_bSlider != null) _bSlider.ValueChanged -= OnBValueChanged;
            if (_rTextbox != null) _rTextbox.TextChanged -= OnRTextChanged;
            if (_gTextbox != null) _gTextbox.TextChanged -= OnGTextChanged;
            if (_bTextbox != null) _bTextbox.TextChanged -= OnBTextChanged;

            _tabControl = GetTemplateChild(TabControlName) as TabControl;
            _textblock = GetTemplateChild(HexTextBoxPartName) as TextBox;
            _listbox = GetTemplateChild(ColorItemsControlPartName) as ItemsControl;
            _mdColorPicker = GetTemplateChild(ColorPickerName) as MaterialDesignThemes.Wpf.ColorPicker;
            _rSlider = GetTemplateChild(RSliderColorName) as Slider;
            _gSlider = GetTemplateChild(GSliderColorName) as Slider;
            _bSlider = GetTemplateChild(BSliderColorName) as Slider;
            _rTextbox = GetTemplateChild(RTextBoxColorName) as TextBox;
            _gTextbox = GetTemplateChild(GTextBoxColorName) as TextBox;
            _bTextbox = GetTemplateChild(BTextBoxColorName) as TextBox;

            if (_tabControl != null) _tabControl.SelectionChanged += TabControl_SelectionChanged;
            if (_textblock != null) { }
            if (_listbox != null) { }
            if (_mdColorPicker != null) { }
            if (_rSlider != null) _rSlider.ValueChanged += OnRValueChanged;
            if (_gSlider != null) _gSlider.ValueChanged += OnGValueChanged;
            if (_bSlider != null) _bSlider.ValueChanged += OnBValueChanged;
            if (_rTextbox != null) _rTextbox.TextChanged += OnRTextChanged;
            if (_gTextbox != null) _gTextbox.TextChanged += OnGTextChanged;
            if (_bTextbox != null) _bTextbox.TextChanged += OnBTextChanged;


            //if (_mdColorPicker != null) { _mdColorPicker.Color = SelectedColor.Value; }


            base.OnApplyTemplate();
        }

        private void OnRValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is not Slider slider || SelectedColor == null) 
                return;
            var strValue = Convert.ToInt32(slider.Value).ToString();
            var byteValue = byte.Parse(strValue);
            var temp = Color.FromRgb(byteValue, SelectedColor.Value.G, SelectedColor.Value.B);
            if (temp != SelectedColor)
                SelectedColor = temp;
        }
        private void OnGValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is not Slider slider || SelectedColor == null) 
                return;
            var strValue = Convert.ToInt32(slider.Value).ToString();
            var byteValue = byte.Parse(strValue);
            var temp = Color.FromRgb(SelectedColor.Value.R, byteValue, SelectedColor.Value.B);
            if (temp != SelectedColor)
                SelectedColor = temp;
        }
        private void OnBValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is not Slider slider || SelectedColor == null) 
                return;
            var strValue = Convert.ToInt32(slider.Value).ToString();
            var byteValue = byte.Parse(strValue);
            var temp = Color.FromRgb(SelectedColor.Value.R, SelectedColor.Value.G, byteValue);
            if (temp != SelectedColor)
                SelectedColor = temp;
        }

        private void OnRTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textbox || string.IsNullOrEmpty(textbox.Text) || SelectedColor == null) 
                return;
            var byteValue = byte.Parse(textbox.Text);
            var temp = Color.FromRgb(byteValue, SelectedColor.Value.G, SelectedColor.Value.B);
            if (temp != SelectedColor)
                SelectedColor = temp;
        }
        private void OnGTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textbox || string.IsNullOrEmpty(textbox.Text) || SelectedColor == null)
                return;
            var byteValue = byte.Parse(textbox.Text);
            var temp = Color.FromRgb(SelectedColor.Value.R, byteValue, SelectedColor.Value.B);
            if (temp != SelectedColor)
                SelectedColor = temp;
        }
        private void OnBTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textbox || string.IsNullOrEmpty(textbox.Text) || SelectedColor == null)
                return;
            var byteValue = byte.Parse(textbox.Text);
            var temp = Color.FromRgb(SelectedColor.Value.R, SelectedColor.Value.G, byteValue);
            if (temp != SelectedColor)
                SelectedColor = temp;
        }

        private void ChangeSelectedColorHandler(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            SelectedColor = (Color)executedRoutedEventArgs.Parameter;
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is TabItem tabItem)
            {
                switch (tabItem.Header)
                {
                    case "標準":
                        break;
                  
                }
            }
        }
    }
}