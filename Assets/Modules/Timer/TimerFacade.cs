using UnityEngine;
using Zenject;

namespace SETHD.Timer
{
    public class TimerFacade<T> : MonoBehaviour
    {
        public ITimer<T> Timer { get; }

        private ITimer<T> timer;

        [Inject]
        private void Construct(ITimer<T> timer)
        {
            this.timer = timer;
        }
    }
}