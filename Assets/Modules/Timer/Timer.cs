using System;
using Zenject;
using UnityEngine.Assertions;

namespace SETHD.Timer
{
    public class Timer : ITimer<float>, IFixedTickable
    {
        public event Action OnStopped;
        
        public float Time { get; }

        private bool isTicking;
        private float time;
        private float endTime;
        
        public void Initialize(float endTime)
        {
            this.endTime = endTime;
            OnStopped = null;
        }

        public void Dispose()
        {
            isTicking = false;
            OnStopped = null;
        }
        
        public void Start()
        {
            Assert.IsTrue(time > 0);
            Assert.IsTrue(time > endTime);
            
            isTicking = true;
            time = 0;
        }

        public void Pause()
        {
            isTicking = false;
        }

        public void Stop()
        {
            isTicking = false;
            time = 0;
        }

        public void FixedTick()
        {
            if (!isTicking)
                return;
            
            time += UnityEngine.Time.fixedDeltaTime;
        }
    }
}