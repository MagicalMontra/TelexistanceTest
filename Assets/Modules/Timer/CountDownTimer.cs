using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;

namespace SETHD.Timer
{
    public class CountDownTimer : ITimer<float>, IFixedTickable, ILateDisposable
    {
        public IReactiveProperty<float> Time { get; private set; }
        public IObservable<float> Observable => observable;

        private bool isTicking;
        private float time;
        private float endTime;
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
            Assert.IsTrue(endTime > 0);
            isTicking = true;
            time = endTime;
        }

        public void Pause()
        {
            isTicking = !isTicking;
        }

        public void Stop()
        {
            isTicking = false;
            time = endTime;
        }
        
        private void UpdateTime(float deltaTime)
        {
            if (!isTicking)
                return;
            
            if (time <= 0)
                return;
            
            time -= deltaTime;
            time = Math.Max(time, 0);
                
            observer.OnNext(time);

            if (time > 0)
                return;

            observer.OnCompleted();
            Stop();
        }

        private IDisposable GetObserver(IObserver<float> observer)
        {
            this.observer = observer;
            return Disposable.Empty;
        }
    }
}