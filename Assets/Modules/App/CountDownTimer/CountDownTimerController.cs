using UniRx;
using System;
using Zenject;
using SETHD.Echo;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SETHD.Timer;
using SETHD.UI.CountDownTimer;
using PlayMode = SETHD.Echo.PlayMode;

namespace SETHD.App.CountDownTimer
{
    public class CountDownTimerController : IInitializable, ILateDisposable
    {
        private const int HOURS_AS_SECONDS_MULTIPLER = 3600;
        private const int MINUTES_AS_SCEONDS_MULTIPLER = 60;
        private const string ON_COMPLETED_SOUND_KEY = "CountDownEnd";

        private readonly ITimer<float> timer;
        private readonly CountDownTimerUI ui;
        private readonly IAudioChannel audioChannel;

        private float seconds;
        private float hourAsSeconds;
        private float minuteAsSeconds;

        private IDisposable textDisposable;
        private IDisposable stopDisposable;
        private IDisposable timerDisposable;
        private IDisposable startDisposable;
        private IDisposable pauseDisposable;
        
        public CountDownTimerController(ITimer<float> timer, CountDownTimerUI ui, IAudioChannel audioChannel)
        {
            this.ui = ui;
            this.timer = timer;
            this.audioChannel = audioChannel;
        }

        public void Initialize()
        {
            ui.TargetSeconds.Subscribe(value => seconds = value);
            ui.TargetHours.Subscribe(value => hourAsSeconds = value * HOURS_AS_SECONDS_MULTIPLER);
            ui.TargetMinutes.Subscribe(value => minuteAsSeconds = value * MINUTES_AS_SCEONDS_MULTIPLER);
            
            textDisposable = timer.Time.Where(time => time > 0).Subscribe(ui.SetTimeText);
            stopDisposable = ui.StopButton.OnClickAsObservable().Subscribe(_ => OnStop());
            startDisposable = ui.StartButton.OnClickAsObservable().Subscribe(_ => OnStart());
            pauseDisposable = ui.PauseButton.OnClickAsObservable().Subscribe(_ => OnPause());
            
            timerDisposable = timer.Observable.Subscribe(OnNext, OnComplete);
        }

        public void LateDispose()
        {
            textDisposable.Dispose();
            stopDisposable.Dispose();
            startDisposable.Dispose();
            pauseDisposable.Dispose();
            timerDisposable.Dispose();
        }

        public void SetActive(bool isEnabled)
        {
            ui.gameObject.SetActive(isEnabled);
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

        private void OnPause()
        {
            timer.Pause();
            ui.PauseButtonText.text = timer.IsPause.Value ? "Pause" : "Resume";
        }

        private void OnStop()
        {
            OnStopAsync().Forget();
        }
        
        private async UniTaskVoid OnStopAsync()
        {
            timer.Stop();
            ui.PauseButtonText.text = "Pause";
            await ui.EndMask();
            timer.Initialize(ui.TargetSeconds.Value);
        }
        
        private void OnNext(float time)
        {
#if UNITY_EDITOR
            Debug.Log($"{time:F2}");
#endif
        }

        private void OnComplete()
        {
            OnCompleteAsync().Forget();
        }
        
        private async UniTaskVoid OnCompleteAsync()
        {
#if UNITY_EDITOR
            Debug.Log($"Count Down Completed");
#endif
            
            audioChannel.Play(ON_COMPLETED_SOUND_KEY, PlayMode.StartOver).Forget();
            await ui.EndMask();
            timer.Initialize(hourAsSeconds + minuteAsSeconds + seconds);
            timer.Observable.Subscribe(OnNext, OnComplete);
        }
    }
}