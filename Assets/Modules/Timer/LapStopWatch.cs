using UniRx;
using System;
using Zenject;
using System.Collections.Generic;

namespace SETHD.Timer
{
    public class LapStopWatch : ITimer<List<float>>, IFixedTickable, ILateDisposable
    {
        public IReactiveProperty<List<float>> Time { get; private set; }
        public IReactiveProperty<bool> IsPause { get; private set; }
        public IObservable<float> Observable => observable;
        
        private bool isTicking;
        private IObserver<float> observer;
        private IObservable<float> observable;

        public LapStopWatch()
        {
            IsPause = new ReactiveProperty<bool>(false);
            observable = UniRx.Observable.Create<float>(GetObserver);
            Time = new ReactiveProperty<List<float>>(new List<float>());
        }
        
        public void Initialize(List<float> times = null)
        {
            Time.Value.Clear();
            Time.Value.Add(0);
        }
        
        public void FixedTick()
        {
            UpdateTime(UnityEngine.Time.fixedDeltaTime);
        }

        public void LateDispose()
        {
            isTicking = false;
            Time.Value.Clear();
            Time = null;
            observer = null;
            observable = null;
        }
        
        public void Start()
        {
            isTicking = true;
            Time.Value.Add(0);
        }

        public void Pause()
        {
            isTicking = !isTicking;
            IsPause.Value = isTicking;
        }

        public void Stop()
        {
            isTicking = false;
            observer.OnCompleted();
            observable = UniRx.Observable.Create<float>(GetObserver);
            Time.Value.Clear();
        }
        
        protected virtual void UpdateTime(float deltaTime)
        {
            if (!isTicking)
                return;

            for (int i = 0; i < Time.Value.Count; i++)
                Time.Value[i] += deltaTime;
            
            observer.OnNext(Time.Value[0]);
        }

        private IDisposable GetObserver(IObserver<float> observer)
        {
            this.observer = observer;
            return Disposable.Empty;
        }
    }
}