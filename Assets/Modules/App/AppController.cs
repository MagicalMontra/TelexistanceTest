using UniRx;
using System;
using Zenject;
using SETHD.UI.App;
using SETHD.UI.Time;
using SETHD.App.LapStopWatch;
using SETHD.App.CountDownTimer;

namespace SETHD.App
{
    public class AppController : IInitializable, ILateDisposable
    {
        private const string FORMAT = "hh:mm tt";
        
        private readonly TimeUI timeUI;
        private readonly MenuButtonUI menuButtonUI;
        private readonly CountDownTimerController countDownController;
        private readonly LapStopWatchController lapStopWatchController;

        private IDisposable timerButtonDisposable;
        private IDisposable currentTimeDisposable;
        private IDisposable stopWatchButtonDisposable;
        
        public AppController(
            TimeUI timeUI,
            MenuButtonUI menuButtonUI,
            CountDownTimerController countDownController, 
            LapStopWatchController lapStopWatchController)
        {
            this.timeUI = timeUI;
            this.menuButtonUI = menuButtonUI;
            this.countDownController = countDownController;
            this.lapStopWatchController = lapStopWatchController;
        }
        
        public void Initialize()
        {
            OpenCountDownMenu();
            timeUI.transform.SetAsLastSibling();
            menuButtonUI.transform.SetAsLastSibling();
            currentTimeDisposable = UniRx.Observable.EveryLateUpdate().Subscribe(UpdateCurrentTime);
            timerButtonDisposable = menuButtonUI.TimerButton.OnClickAsObservable().Subscribe(_ => OpenCountDownMenu());
            stopWatchButtonDisposable = menuButtonUI.StopWatchButton.OnClickAsObservable().Subscribe(_ => OpenStopWatchMenu());
        }

        public void LateDispose()
        {
            currentTimeDisposable.Dispose();
            timerButtonDisposable.Dispose();
            stopWatchButtonDisposable.Dispose();
        }

        private void UpdateCurrentTime(long value)
        {
            timeUI.TimeZoneText.text = TimeZoneInfo.Local.DisplayName;
            timeUI.TimeText.text = DateTime.Now.ToString(FORMAT);
        }

        private void OpenStopWatchMenu()
        {
            menuButtonUI.TimerButtonText.color = menuButtonUI.NormalColor;
            menuButtonUI.StopWatchButtonText.color = menuButtonUI.ActiveColor;
            menuButtonUI.TimerButton.interactable = true;
            menuButtonUI.StopWatchButton.interactable = false;
            countDownController.SetActive(false);
            lapStopWatchController.SetActive(true);
        }
        
        private void OpenCountDownMenu()
        {
            menuButtonUI.TimerButtonText.color = menuButtonUI.ActiveColor;
            menuButtonUI.StopWatchButtonText.color = menuButtonUI.NormalColor;
            menuButtonUI.TimerButton.interactable = false;
            menuButtonUI.StopWatchButton.interactable = true;
            countDownController.SetActive(true);
            lapStopWatchController.SetActive(false);
        }
    }
}
