using System;
using UniRx;

namespace SETHD.Timer
{
    public interface ITimer
    {
        IReactiveProperty<float> Time { get; }
        IObservable<float> Observable { get; }
        void Initialize();
        void Start();
        void Pause();
        void Stop();
    }
    
    public interface ITimer<T>
    {
        IReactiveProperty<T> Time { get; }
        IObservable<float> Observable { get; }
        void Initialize(T generic);
        void Start();
        void Pause();
        void Stop();
    }
}
