using System.Collections;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using ASI.TCL.CMFT.WPF.Module.DMD.DataTypes;
using ASI.TCL.CMFT.WPF.Module.DMD.Dtos;

namespace ASI.TCL.CMFT.WPF.Module.DMD.TriggerActions
{
    public class StationDirectionTriggerAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(IList),
            typeof(StationDirectionTriggerAction),
            new PropertyMetadata(null));

        public IList Source
        {
            get => (IList)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            nameof(Target),
            typeof(IList),
            typeof(StationDirectionTriggerAction),
            new PropertyMetadata(null));

        public IList Target
        {
            get => (IList)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public static readonly DependencyProperty TargetOperationProperty = DependencyProperty.Register(
            nameof(TargetOperation),
            typeof(string),
            typeof(StationDirectionTriggerAction),
            new PropertyMetadata(null));

        public string TargetOperation
        {
            get => (string)GetValue(TargetOperationProperty);
            set => SetValue(TargetOperationProperty, value);
        }

        public static readonly DependencyProperty OperationIDProperty = DependencyProperty.Register(
            nameof(OperationID),
            typeof(int),
            typeof(StationDirectionTriggerAction),
            new PropertyMetadata(0));

        public int OperationID
        {
            get => (int)GetValue(OperationIDProperty);
            set => SetValue(OperationIDProperty, value);
        }

        public static readonly DependencyProperty MessageDirectionProperty = DependencyProperty.Register(
            nameof(MessageDirection),
            typeof(eMessageDirection),
            typeof(StationDirectionTriggerAction),
            new PropertyMetadata(eMessageDirection.None));

        public eMessageDirection MessageDirection
        {
            get => (eMessageDirection)GetValue(MessageDirectionProperty);
            set => SetValue(MessageDirectionProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += (args, e) =>
            {
                if (!(AssociatedObject is FrameworkElement source) || !(Target is FrameworkElement target))
                    return;
            };
        }

        protected override void Invoke(object parameter)
        {
            if (Source == null || Target == null || string.IsNullOrEmpty(TargetOperation))
                return;

            // 調試輸出
            System.Diagnostics.Debug.WriteLine($"StationDirectionTriggerAction: Operation={TargetOperation}, MessageDirection={MessageDirection}");

            switch (TargetOperation)
            {
                case "Add":
                    foreach (var sourceItem in Source)
                    {
                        if (sourceItem is SYSStationDto station && !ContainsStation(Target, station.StationID))
                        {
                            var originalName = station.StationName;
                            // 直接修改原物件的 StationName，而不是創建新物件
                            station.StationName = GetStationDisplayName(station, MessageDirection);
                            System.Diagnostics.Debug.WriteLine($"Adding station: {originalName} -> {station.StationName}");
                            Target.Add(station);
                        }
                    }
                    break;

                case "Remove":
                    foreach (var sourceItem in Source)
                    {
                        if (sourceItem is SYSStationDto station)
                        {
                            System.Diagnostics.Debug.WriteLine($"Removing station: {station.StationName}");
                            RemoveStationFromTarget(Target, station.StationID);
                        }
                    }
                    break;
            }
        }

        private bool ContainsStation(IList targetList, eStationID stationID)
        {
            foreach (var item in targetList)
            {
                if (item is SYSStationDto station && station.StationID == stationID)
                    return true;
            }
            return false;
        }

        private void RemoveStationFromTarget(IList targetList, eStationID stationID)
        {
            for (int i = targetList.Count - 1; i >= 0; i--)
            {
                if (targetList[i] is SYSStationDto station && station.StationID == stationID)
                {
                    targetList.RemoveAt(i);
                }
            }
        }


        private string GetStationDisplayName(SYSStationDto station, eMessageDirection direction)
        {
            // 確保取得原始車站名稱，移除可能已存在的方向標籤
            var baseStationName = GetBaseStationName(station.StationName);
            
            return direction switch
            {
                eMessageDirection.Message => $"{baseStationName} [訊息]",
                eMessageDirection.Up => $"{baseStationName} [上行]",
                eMessageDirection.Down => $"{baseStationName} [下行]",
                _ => baseStationName
            };
        }

        private string GetBaseStationName(string stationName)
        {
            if (string.IsNullOrEmpty(stationName))
                return stationName;

            // 移除可能的方向標籤
            var patterns = new[] { " [訊息]", " [上行]", " [下行]" };
            foreach (var pattern in patterns)
            {
                if (stationName.EndsWith(pattern))
                {
                    return stationName.Substring(0, stationName.Length - pattern.Length);
                }
            }
            
            return stationName;
        }
    }
}