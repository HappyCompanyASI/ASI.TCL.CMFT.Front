using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace ASI.TCL.CMFT.WPF.ThemeSettings
{
    internal class MaterialThemeService : IMaterialThemeService
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        //取得調色盤
        public IEnumerable<ISwatch> Swatches => SwatchHelper.Swatches;
        public IEnumerable<Contrast> ContrastValues => Enum.GetValues(typeof(Contrast)).Cast<Contrast>();
        public IEnumerable<ColorSelection> ColorSelections => Enum.GetValues(typeof(ColorSelection)).Cast<ColorSelection>();

        //設定主題
        public async Task<MaterialThemeSettings> GetThemeAsync()
        {
            var CurrentApartmentState = System.Threading.Thread.CurrentThread.GetApartmentState();
            return await System.Windows.Application.Current.Dispatcher.Invoke(async () =>
            {
                var theme = _paletteHelper.GetTheme();
                var currentSetting = await MaterialThemeToMaterialThemeSettingAsync(theme);
                return currentSetting;
            });
        }
        public async Task SetThemeAsync(MaterialThemeSettings materialThemeSettings)
        {
            var CurrentApartmentState = System.Threading.Thread.CurrentThread.GetApartmentState();
            await System.Windows.Application.Current.Dispatcher.Invoke(async () =>
            {
                var theme = await MaterialThemeSettingToMaterialThemeAsync(materialThemeSettings);

                _paletteHelper.SetTheme(theme);
            });
        }
     
        private async Task<MaterialThemeSettings> MaterialThemeToMaterialThemeSettingAsync(Theme theme)
        {
            return await Task.Run(() =>
            {
                var materialThemeSetting = new MaterialThemeSettings();

                materialThemeSetting.BaseTheme = theme.GetBaseTheme();

                if (theme.ColorAdjustment != null)
                {
                    materialThemeSetting.ContrastRatio = theme.ColorAdjustment.DesiredContrastRatio;
                    materialThemeSetting.ContrastValue = theme.ColorAdjustment.Contrast;
                    materialThemeSetting.ColorSelection = theme.ColorAdjustment.Colors;
                }
                else
                {
                    materialThemeSetting.ContrastRatio = 0.0f;
                    materialThemeSetting.ContrastValue = Contrast.None;
                    materialThemeSetting.ColorSelection = ColorSelection.None;
                }

                materialThemeSetting.Primary = theme.PrimaryMid.Color;
           
                materialThemeSetting.Secondary = theme.SecondaryMid.Color;
            
                return materialThemeSetting;
            });
        }
        private async Task<Theme> MaterialThemeSettingToMaterialThemeAsync(MaterialThemeSettings materialThemeSettings)
        {
            return await Task.Run(() =>
            {
                var theme = Theme.Create(materialThemeSettings.BaseTheme, materialThemeSettings.Primary, materialThemeSettings.Secondary);
                theme.ColorAdjustment = new ColorAdjustment();
                theme.ColorAdjustment.DesiredContrastRatio = materialThemeSettings.ContrastRatio;
                theme.ColorAdjustment.Contrast = materialThemeSettings.ContrastValue;
                theme.ColorAdjustment.Colors = materialThemeSettings.ColorSelection;

                return theme;
            });
        }
    }
}