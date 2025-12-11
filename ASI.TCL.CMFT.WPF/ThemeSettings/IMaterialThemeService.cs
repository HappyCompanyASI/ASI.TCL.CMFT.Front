using System.Collections.Generic;
using System.Threading.Tasks;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace ASI.TCL.CMFT.WPF.ThemeSettings
{
    public interface IMaterialThemeService
    {
        //取得調色盤
        IEnumerable<ISwatch> Swatches { get; }
        IEnumerable<Contrast> ContrastValues { get; }
        IEnumerable<ColorSelection> ColorSelections { get; }

        Task<MaterialThemeSettings> GetThemeAsync();
        Task SetThemeAsync(MaterialThemeSettings materialThemeSettings);
    }
}