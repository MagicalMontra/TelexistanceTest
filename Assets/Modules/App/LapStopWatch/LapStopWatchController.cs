using UniRx;
using System;
using Zenject;
using SETHD.Timer;
using UnityEngine;
using SETHD.UI.LapStopWatch;
using System.Collections.Generic;

namespace SETHD.App.LapStopWatch
{
    public class LapStopWatchController : IInitializable, ILateDisposable
    {
        private const string FORMAT = @"mm\:ss\.ff";
        
        private readonly LapStopWatchUI ui;
        private readonly ITimer<List<float>> timer;

        private IDisposable lapDisposable;
        private IDisposable resetDisposable;
        private IDisposable startDisposable;
        private IDisposable pauseDisposable;
        private IDisposable timerDisposable;
        
        public LapStopWatchController(LapStopWatchUI ui, ITimer<List<float>> timer)
        {
            this.ui = ui;
            this.timer = timer;
        }
        
        public void Initialize()
        {
            lapDisposable = ui.LapButton.OnClickAsObservable().Subscribe(_ => OnLap());
            pauseDisposable = ui.StopButton.OnClickAsObservable().Subscribe(_ => OnPause());
            resetDisposable = ui.ResetButton.OnClickAsObservable().Subscribe(_ => OnReset());
            startDisposable = ui.StartButton.OnClickAsObservable().Subscribe(_ => OnStart());
            
            timerDisposable = timer.Observable.Subscribe(OnNext, OnComplete);
            
            ui.LapButton.interactable = false;
            ui.StartButton.gameObject.SetActive(true);
            ui.StopButton.gameObject.SetActive(false);
            ui.ResetButton.gameObject.SetActive(false);
        }

        public void LateDispose()
        {
            lapDisposable.Dispose();
            timerDisposable.Dispose();
            resetDisposable.Dispose();
            startDisposable.Dispose();
            pauseDisposable.Dispose();
        }
        
        public void SetActive(bool isEnabled)
        {
            ui.gameObject.SetActive(isEnabled);
        }

        private void SubscribeTimeText(List<float> timeList)
        {
            if (timeList.Count == 0)
            {
                ui.ResetTime();
                return;
            }

            if (timeList.Count == 1)
            {
                ui.LapTimeText.gameObject.SetActive(false);
                ui.LapTimeText.text = "00:00.00";
                var span = TimeSpan.FromSeconds(timeList[0]);
                ui.TotalTimeText.text = span.ToString(FORMAT);
                return;
            }
            
            var lapSpan = TimeSpan.FromSeconds(timeList[^1]);
            var totalSpan = TimeSpan.FromSeconds(timeList[0]);
            ui.LapTimeText.gameObject.SetActive(true);
            ui.LapTimeText.text = lapSpan.ToString(FORMAT);
            ui.TotalTimeText.text = totalSpan.ToString(FORMAT);
        }

        private void OnStart()
        {
            ui.ResetTime();
            timer.Start();
            ui.LapButton.interactable = true;
            ui.LapButton.gameObject.SetActive(true);
            ui.StopButton.gameObject.SetActive(true);
            ui.StartButton.gameObject.SetActive(false);
            ui.ResetButton.gameObject.SetActive(false);
        }

        private void OnReset()
        {
            timer.Stop();
            ui.StopButtonText.text = "Stop";
            ui.LapButton.interactable = false;
            ui.LapButton.gameObject.SetActive(true);
            ui.StartButton.gameObject.SetActive(true);
            ui.StopButton.gameObject.SetActive(false);
            ui.ResetButton.gameObject.SetActive(false);
            timerDisposable = timer.Observable.Subscribe(OnNext, OnComplete);
        }
        
        private void OnPause()
        {
            timer.Pause();
            ui.LapButton.interactable = timer.IsPause.Value;
            ui.LapButton.gameObject.SetActive(timer.IsPause.Value);
            ui.ResetButton.gameObject.SetActive(!timer.IsPause.Value);
            ui.StopButtonText.text = timer.IsPause.Value ? "Stop" : "Resume";
        }

        private void OnLap()
        {
            timer.Start();
            var lapSpan = TimeSpan.FromSeconds(timer.Time.Value[^2]);
            var totalSpan = TimeSpan.FromSeconds(timer.Time.Value[0]);
            
            if (timer.Time.Value.Count > 1)
                ui.AddLap(lapSpan, totalSpan);
            
            Debug.Log("lap");
        }
        
        private void OnNext(float time)
        {
#if UNITY_EDITOR
            // Debug.Log($"{time:F2}");
#endif

            SubscribeTimeText(timer.Time.Value);
        }

        private void OnComplete()
        {
#if UNITY_EDITOR
            Debug.Log($"Completed");
#endif
        }
    }
}