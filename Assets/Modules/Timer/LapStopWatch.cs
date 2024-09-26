using UniRx;
using System;
using Zenject;
using System.Collections.Generic;

namespace SETHD.Timer
{
    public class LapStopWatchController : IInitializable, ILateDisposable
    {
        private readonly ITimer<float> timer;
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void LateDispose()
        {
            throw new NotImplementedException();
        }
    }
    
    public class LapStopWatch : ITimer<List<float>>, IFixedTickable, ILateDisposable
    {
        public IReactiveProperty<List<float>> Time { get; private set; }
        public IObservable<float> Observable => observable;
        
        private bool isTicking;
        private IObserver<float> observer;
        private IObservable<float> observable;

        public LapStopWatch()
        {
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
        }

        public void Stop()
        {
            isTicking = false;
            observer.OnCompleted();
            observable = UniRx.Observable.Create<float>(GetObserver);
            Time.Value.Clear();
            Time.Value.Add(0);
        }
        
        protected virtual void UpdateTime(float deltaTime)
        {
            if (!isTicking)
                return;

            for (int i = 0; i < Time.Value.Count; i++)
                Time.Value[i] += deltaTime;
        }

        private IDisposable GetObserver(IObserver<float> observer)
        {
            this.observer = observer;
            return Disposable.Empty;
        }
    }
}