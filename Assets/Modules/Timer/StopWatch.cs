using UniRx;
using System;
using Zenject;

namespace SETHD.Timer
{
    public class StopWatch : ITimer, IFixedTickable, ILateDisposable
    {
        public IReactiveProperty<float> Time { get; private set; }
        public IObservable<float> Observable => observable;
        
        private bool isTicking;
        private IObserver<float> observer;
        private IObservable<float> observable;

        public StopWatch()
        {
            Time = new ReactiveProperty<float>(0);
            observable = UniRx.Observable.Create<float>(GetObserver);
        }
        
        public void Initialize()
        {
            Time.Value = 0;
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
            Time.Value = 0;
        }

        public void Pause()
        {
            isTicking = !isTicking;
        }

        public void Stop()
        {
            isTicking = false;
            observer.OnCompleted();
            Time.Value = 0;
        }
        
        protected virtual void UpdateTime(float deltaTime)
        {
            if (!isTicking)
                return;
            
            Time.Value = 0;
            observer.OnNext(Time.Value);
        }

        private IDisposable GetObserver(IObserver<float> observer)
        {
            this.observer = observer;
            return Disposable.Empty;
        }
    }
}