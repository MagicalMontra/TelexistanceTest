using UniRx;
using System;
using UnityEngine;
using Zenject;
using UnityEngine.Assertions;

namespace SETHD.Timer
{
    public class CountDownTimer : ITimer<float>, IFixedTickable, ILateDisposable
    {
        public IReactiveProperty<float> Time { get; private set; }
        public IObservable<float> Observable => observable;

        private bool isTicking;
        private IObserver<float> observer;
        private IObservable<float> observable;

        public CountDownTimer()
        {
            Time = new ReactiveProperty<float>(0);
            observable = UniRx.Observable.Create<float>(GetObserver);
        }
        
        public void Initialize(float endTime)
        {
            Time.Value = endTime;
        }
        
        public void FixedTick()
        {
            UpdateTime(UnityEngine.Time.fixedDeltaTime);
        }

        public void LateDispose()
        {
            isTicking = false;
            observer = null;
            observable = null;
        }
        
        public void Start()
        {
            isTicking = true;
        }

        public void Pause()
        {
            isTicking = !isTicking;
        }

        public void Stop()
        {
            isTicking = false;
        }
        
        private void UpdateTime(float deltaTime)
        {
            if (!isTicking)
                return;
            
            if (Time.Value <= 0)
                return;
            
            Time.Value -= deltaTime;
            Time.Value = Math.Max(Time.Value, 0);
                
            observer.OnNext(Time.Value);

            if (Time.Value > 0)
                return;

            observer.OnCompleted();
            Stop();
        }

        private IDisposable GetObserver(IObserver<float> obs)
        {
            observer = obs;
            return Disposable.Empty;
        }
    }
}