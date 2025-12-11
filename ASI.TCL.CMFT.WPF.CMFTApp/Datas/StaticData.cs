using System.ComponentModel;
using ASI.TCL.CMFT.WPF.CMFTApp.DataTypes;

namespace ASI.TCL.CMFT.WPF.CMFTApp.Datas
{
    public class StaticData  
    {
        private static eStep? _currentStep = eStep.Step1;
        public static eStep? CurrentStep
        {
            get => _currentStep;
            set
            {
                if (_currentStep == value) return;
                _currentStep = value;
                OnStaticPropertyChanged(nameof(CurrentStep));
            }
        }
        

        private static bool? _isEditMode = false;
        public static bool? IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode == value) return;
                _isEditMode = value;
                OnStaticPropertyChanged(nameof(IsEditMode));
            }
        }


        public static event PropertyChangedEventHandler StaticPropertyChanged;
        protected static void OnStaticPropertyChanged(string propertyName)=> StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));

       
    }
}
