using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.ThemeSettings
{
    public class ThemeSettingDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IMaterialThemeService _materialThemeService;

        #region Constructors
        public ThemeSettingDialogViewModel()
        {
            //if (CMFTCore.IsDesignTime)
            //{
            //    _materialThemeService = new MaterialThemeService();
            //    SelectedSetting = DesignTimeWork.MaterialThemeSetting;
            //    UpdateUI();
            //}
        }
        public ThemeSettingDialogViewModel(IMaterialThemeService materialThemeService)
        {
            _materialThemeService = materialThemeService;
        }
        #endregion
 
        #region IDialogAware
        public string Title => "主題設定";
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed()
        {
        }
        public async void OnDialogOpened(IDialogParameters parameters)
        {
            _currentSettings = await _materialThemeService.GetThemeAsync();
            _currentContrastRatio = _currentSettings.ContrastRatio;
            _currentContrastValue = _currentSettings.ContrastValue;
            _currentColorSelection = _currentSettings.ColorSelection;
            // 先設置為一個不同的值
            SelectedColorScheme = ColorSelection.None;
            RaisePropertyChanged(nameof(SelectedColorScheme));
            SelectedSettings = new MaterialThemeSettings(_currentSettings);
            UpdateUI();
        }
        #endregion

        #region Properties
        private MaterialThemeSettings _currentSettings; 
        private float _currentContrastRatio;
        private Contrast _currentContrastValue;
        private ColorSelection _currentColorSelection;

        public MaterialThemeSettings SelectedSettings { get; set; }
        public IEnumerable<Contrast> ContrastValues => _materialThemeService.ContrastValues;
        public IEnumerable<ColorSelection> ColorSelections => _materialThemeService.ColorSelections;
        public IEnumerable<ISwatch> Swatches => _materialThemeService.Swatches;
        public ColorSelection SelectedColorScheme { get; set; } 

        public Color? SelectedColor { get; set; } = new Color();
        public bool? IsColorAdjusted { get; set; } = false;
        #endregion

        #region DelegateCommands
        private DelegateCommand<bool?> _changeBaseThemeCommand;
        public DelegateCommand<bool?> ChangeBaseThemeCommand => _changeBaseThemeCommand ??= new DelegateCommand<bool?>(ExcuteChangeBaseThemeCommand);
        private async void ExcuteChangeBaseThemeCommand(bool? isDark)
        {
            SelectedSettings.BaseTheme = isDark is true ? BaseTheme.Dark : BaseTheme.Light;
            await Task.Delay(200);//先更新畫面
            await _materialThemeService.SetThemeAsync(SelectedSettings);
        }

        private DelegateCommand _changeColorAdjustmentCommand;
        public DelegateCommand  ChangeColorAdjustmentCommand => _changeColorAdjustmentCommand ??= new DelegateCommand(ExcuteChangeColorAdjustmentCommand);
        private async void ExcuteChangeColorAdjustmentCommand()
        {
            if(IsColorAdjusted is false)
            {
                _currentContrastRatio = SelectedSettings.ContrastRatio;
                _currentContrastValue = SelectedSettings.ContrastValue;
                _currentColorSelection = SelectedSettings.ColorSelection;
                SelectedSettings.ContrastRatio = 1.0f;
                SelectedSettings.ContrastValue = Contrast.None;
                SelectedSettings.ColorSelection = ColorSelection.None;
            }
            else
            {
                SelectedSettings.ContrastRatio = _currentContrastRatio;
                SelectedSettings.ContrastValue = _currentContrastValue;
                SelectedSettings.ColorSelection = _currentColorSelection;
            }
            RaisePropertyChanged(nameof(SelectedSettings));
            await Task.Delay(200); //先更新畫面
            await _materialThemeService.SetThemeAsync(SelectedSettings);
        }

        private DelegateCommand _changeColorAdjustmentParamCommand;
        public DelegateCommand ChangeColorAdjustmentParamCommand => _changeColorAdjustmentParamCommand ??= new DelegateCommand(ExcuteChangeColorAdjustmentParamCommand);
        private async void ExcuteChangeColorAdjustmentParamCommand()
        {
            RaisePropertyChanged(nameof(SelectedSettings));
            await Task.Delay(200); //先更新畫面
            await _materialThemeService.SetThemeAsync(SelectedSettings);
        }

        private DelegateCommand _changeCurrentSchemeCommand;
        public DelegateCommand ChangeCurrentSchemeCommand => _changeCurrentSchemeCommand ??= new DelegateCommand(ExcuteChangeCurrentSchemeCommand);
        private async void ExcuteChangeCurrentSchemeCommand()
        {
            SelectedColor = SelectedColorScheme switch
            {
                ColorSelection.Primary => SelectedSettings.Primary,
                ColorSelection.Secondary => SelectedSettings.Secondary,
               
                _ => throw new ArgumentOutOfRangeException()
            };
            RaisePropertyChanged(nameof(SelectedColor));
            await Task.Delay(200);//先更新畫面
            await _materialThemeService.SetThemeAsync(SelectedSettings);
        }

        private DelegateCommand _changeCurrentColorCommand;
        public DelegateCommand ChangeCurrentColorCommand => _changeCurrentColorCommand ??= new DelegateCommand(ExcuteChangeCurrentColorCommand);
        private async void ExcuteChangeCurrentColorCommand()
        {
            if (SelectedColor is not { } color)
                return;
            switch(SelectedColorScheme)
            {
                case ColorSelection.Primary:
                    SelectedSettings.Primary = SelectedColor.Value;
                    break;
                case ColorSelection.Secondary:
                    SelectedSettings.Secondary = SelectedColor.Value;
                    break;
             
                default:
                    throw new ArgumentOutOfRangeException();
            }
            RaisePropertyChanged(nameof(SelectedSettings));
            await Task.Delay(200);//先更新畫面
            await _materialThemeService.SetThemeAsync(SelectedSettings);
        }

        private DelegateCommand _restoreCommand;
        public DelegateCommand RestoreCommand => _restoreCommand ??= new DelegateCommand(ExcuteRestoreCommand);
        private async void ExcuteRestoreCommand()
        {
            _currentContrastRatio = _currentSettings.ContrastRatio;
            _currentContrastValue = _currentSettings.ContrastValue;
            _currentColorSelection = _currentSettings.ColorSelection;

            SelectedSettings = new MaterialThemeSettings(_currentSettings);
            UpdateUI();

            await _materialThemeService.SetThemeAsync(SelectedSettings);
        }

        private DelegateCommand _defaultCommand;
        public DelegateCommand DefaultCommand => _defaultCommand ??= new DelegateCommand(ExcuteDefaultCommand);
        private async void ExcuteDefaultCommand()
        {
            SelectedSettings = new MaterialThemeSettings
            {
                BaseTheme = BaseTheme.Dark,
                ContrastRatio = 1.0f,
                ContrastValue = Contrast.None,
                ColorSelection = ColorSelection.None,
                Primary = default,
            
                Secondary = default,
             
            };
            UpdateUI();
            await _materialThemeService.SetThemeAsync(SelectedSettings);
        }

        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand => _okCommand ??= new DelegateCommand(ExcuteOKCommand);
        private void ExcuteOKCommand() 
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters() { {nameof(SelectedSettings), SelectedSettings}})); 
        } 

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(ExcuteCancelCommand);
        private async void ExcuteCancelCommand()
        {
            await _materialThemeService.SetThemeAsync(_currentSettings);
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }
        #endregion

        private void UpdateUI()
        {
            RaisePropertyChanged(nameof(SelectedSettings));
            SelectedColorScheme = ColorSelection.Primary;
            RaisePropertyChanged(nameof(SelectedColorScheme));

            SelectedColor = SelectedSettings.Primary;
            RaisePropertyChanged(nameof(SelectedColor));


            IsColorAdjusted = SelectedSettings.ContrastValue != Contrast.None
                              && SelectedSettings.ColorSelection != ColorSelection.None;
            RaisePropertyChanged(nameof(IsColorAdjusted));
        }

    }
}