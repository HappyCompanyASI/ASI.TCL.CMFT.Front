using System.Windows.Media;
using ASI.TCL.CMFT.WPF.Applications;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

namespace ASI.TCL.CMFT.WPF.ThemeSettings
{
    public class MaterialThemeSettings : IAppSettings
    {
        [JsonProperty("BaseTheme")]
        public BaseTheme BaseTheme { get; set; }

        [JsonProperty("ContrastRatio")]
        public float ContrastRatio { get; set; }

        [JsonProperty("Contrast")]
        public Contrast ContrastValue { get; set; }

        [JsonProperty("ColorSelection")]
        public ColorSelection ColorSelection { get; set; }

        [JsonProperty("PrimaryColor")]
        public Color Primary { get; set; }

        [JsonProperty("SecondaryColor")]
        public Color Secondary { get; set; }

        public MaterialThemeSettings() { }

        public MaterialThemeSettings(MaterialThemeSettings source)
        {
            BaseTheme = source.BaseTheme;
            ContrastRatio = source.ContrastRatio;
            ContrastValue = source.ContrastValue;
            ColorSelection = source.ColorSelection;
            Primary = source.Primary;
            Secondary = source.Secondary;
        }

        public void Validate()
        {
             
        }
    }
}