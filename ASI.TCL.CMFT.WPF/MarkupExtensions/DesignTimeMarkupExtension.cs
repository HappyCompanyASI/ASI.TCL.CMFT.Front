using System;
using System.Windows;

namespace ASI.TCL.CMFT.WPF.MarkupExtensions
{
    public class DesignTimeMarkupExtension : System.Windows.Markup.MarkupExtension
    {
        // 視圖類型
        public Type ViewType { get; set; }

        // 視圖模型類型
        public Type ViewModelType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // 檢查是否已設置 ViewType 和 ViewModelType
            if (ViewType == null || ViewModelType == null)
                throw new InvalidOperationException("Both ViewType and ViewModelType must be set.");

            // 使用反射創建視圖實例
            var view = Activator.CreateInstance(ViewType);

            // 使用反射創建視圖模型實例
            var viewModel = Activator.CreateInstance(ViewModelType);

            // 確保視圖有 DataContext 屬性並將其設置
            if (view is FrameworkElement viewAsFrameworkElement)
            {
                viewAsFrameworkElement.DataContext = viewModel;
            }

            return view;
        }
    }
}