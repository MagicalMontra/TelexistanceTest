using UniRx;
using System;
using Zenject;
using System.Collections.Generic;

namespace SETHD.Timer
{
    public class LapStopWatch : ITimer<List<float>>, IFixedTickable, ILateDisposable
    {
        public IReactiveProperty<List<float>> Time { get; private set; }
        
        public IObservable<float> Observable => observable;
        
        private bool isTicking;
        private IObserver<float> observer;
        private IObservable<float> observable;

        public LapStopWatch()
        {
            Time = new ReactiveProperty<List<float>>(new List<float>());
            observable = UniRx.Observable.Create<float>(GetObserver);
        }
        
        public void Initialize(List<float> times = null)
        {
            Time.Value.Clear();
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
        }

        public void Stop()
        {
            isTicking = false;
            observer.OnCompleted();
            Time.Value.Clear();
        }
        
        protected virtual void UpdateTime(float deltaTime)
        {
            if (!isTicking)
                return;

            for (int i = 0; i < Time.Value.Count; i++)
                Time.Value[i] += deltaTime;
            
            observer.OnNext(deltaTime);
        }

        private IDisposable GetObserver(IObserver<float> observer)
        {
            this.observer = observer;
            return Disposable.Empty;
        }
    }
}