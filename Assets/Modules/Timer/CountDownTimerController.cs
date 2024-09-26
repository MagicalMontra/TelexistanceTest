using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SETHD.UI.CountDownTimer;
using UniRx;
using UnityEngine;
using Zenject;

namespace SETHD.Timer
{
    public class CountDownTimerController : IInitializable, ILateDisposable
    {
        private const int HOUR_AS_MINUTE_MULTIPLER = 3600;
        private const int MINUTE_AS_MINUTE_MULTIPLER = 60;
        
        private readonly ITimer<float> timer;
        private readonly CountDownTimerUI ui;

        private float seconds;
        private float hourAsSeconds;
        private float minuteAsSeconds;

        private IDisposable textDisposable;
        private IDisposable stopDisposable;
        private IDisposable startDisposable;
        private IDisposable pauseDisposable;
        
        public CountDownTimerController(ITimer<float> timer, CountDownTimerUI ui)
        {
            this.timer = timer;
            this.ui = ui;
        }

        public void Initialize()
        {
            ui.TargetSeconds.Subscribe(value => seconds = value);
            ui.TargetHours.Subscribe(value => hourAsSeconds = value * HOUR_AS_MINUTE_MULTIPLER);
            ui.TargetMinutes.Subscribe(value => minuteAsSeconds = value * MINUTE_AS_MINUTE_MULTIPLER);
            
            textDisposable = timer.Time.Where(time => time > 0).Subscribe(ui.SetTimeText);
            stopDisposable = ui.StopButton.OnClickAsObservable().Subscribe(_ => OnStop());
            startDisposable = ui.StartButton.OnClickAsObservable().Subscribe(_ => OnStart());
            pauseDisposable = ui.PauseButton.OnClickAsObservable().Subscribe(_ => timer.Pause());
            
            timer.Observable.Subscribe(time => Debug.Log($"{time}"), OnComplete);
        }

        public void LateDispose()
        {
            textDisposable.Dispose();
            stopDisposable.Dispose();
            startDisposable.Dispose();
            pauseDisposable.Dispose();
        }

        private void OnStart()
        {
            if (hourAsSeconds + minuteAsSeconds + seconds <= 0)
                return;
            
            OnStartAsync().Forget();
        }

        private async UniTaskVoid OnStartAsync()
        {
            timer.Initialize(hourAsSeconds + minuteAsSeconds + seconds);
            await ui.StartMask();
            timer.Start();
        }

        private void OnStop()
        {
            OnStopAsync().Forget();
        }
        
        private async UniTaskVoid OnStopAsync()
        {
            timer.Stop();
            await ui.EndMask();
            timer.Initialize(ui.TargetSeconds.Value);
        }

        private void OnComplete()
        {
            OnCompleteAsync().Forget();
        }
        
        private async UniTaskVoid OnCompleteAsync()
        {
            await ui.EndMask();
            timer.Initialize(hourAsSeconds + minuteAsSeconds + seconds);
            timer.Observable.Subscribe(time => Debug.Log($"{time}"), OnComplete);
        }
    }
}