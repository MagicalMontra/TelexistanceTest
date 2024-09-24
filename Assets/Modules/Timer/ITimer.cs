using System;

namespace SETHD.Timer
{
    public interface ITimer<T>
    {
        event Action OnStopped;
        float Time { get; }
        void Initialize(T generic);
        void Dispose();
        void Start();
        void Pause();
        void Stop();
    }
}
