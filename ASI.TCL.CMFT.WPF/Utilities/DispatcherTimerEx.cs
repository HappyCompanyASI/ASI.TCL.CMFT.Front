using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ASI.TCL.CMFT.WPF.Utilities
{
    public class DispatcherTimerEx
    {
        #region TimerManager

        public static Dictionary<Guid, DispatcherTimerEx> DispatcherTimers { get; } = new Dictionary<Guid, DispatcherTimerEx>();

        public static DispatcherTimerEx CreateTimer(string dispatcherName, int seconds, string belongViewModel)
        {
            return CreateTimer(dispatcherName, TimeSpan.FromSeconds(seconds), belongViewModel);
        }

        public static DispatcherTimerEx CreateTimer(string dispatcherName, double milliseconds, string belongViewModel)
        {
            return CreateTimer(dispatcherName, TimeSpan.FromMilliseconds(milliseconds), belongViewModel);
        }

        private static DispatcherTimerEx CreateTimer(string dispatcherName, TimeSpan timeSpan, string belongViewModel)
        {
            var timer = new DispatcherTimerEx(dispatcherName, timeSpan, belongViewModel);
            DispatcherTimers.Add(timer._timerID, timer);
            return timer;
        }

        #endregion

        public string TimerName { get; private set; }
        public DateTime StartTime { get; private set; }
        public TimeSpan CurrentCountTime { get; private set; }
        public TimeSpan Interval { get; private set; }
        public string BelongViewModel { get; private set; }

        private readonly Guid _timerID;
        private DispatcherTimer _dispatcherTimer;
        public Func<Task> OnUpdateTask;
        public Func<TimeSpan, Task> OnUpdateUI;
        private DispatcherTimerEx(string timerName, TimeSpan timeSpan, string belongViewModel)
        {
            TimerName = timerName;
            Interval = timeSpan;
            BelongViewModel = belongViewModel;
            CurrentCountTime = new TimeSpan(0);

            _timerID = Guid.NewGuid();
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = Interval;
            _dispatcherTimer.Tick += CountCurrentCountTime;
        }

        public void Start()
        {
            if (_dispatcherTimer == null) return;

            StartTime = DateTime.Now;
            _dispatcherTimer.Start();
        }

        public void Dispose()
        {
            if (_dispatcherTimer == null) return;

            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick -= CountCurrentCountTime;
            _dispatcherTimer = null;
            DispatcherTimers.Remove(_timerID);
        }
        //檢查線程用
        //private ApartmentState a1 = ApartmentState.Unknown;
        //private ApartmentState a2 = ApartmentState.Unknown;
        //private ApartmentState a3 = ApartmentState.Unknown;
        //a2 = Thread.CurrentThread.GetApartmentState();
        private void CountCurrentCountTime(object sender, EventArgs e)
        {
            // STA畫面線程處理
            // 更新UI
            OnUpdateUI?.Invoke(DateTime.Now - StartTime);

            // MTA 背景線程處理
            Task.Factory.StartNew(() =>
            {
                // 啟動背景任務進行其他處理
               
                OnUpdateTask?.Invoke();
            });
        }
    }
}