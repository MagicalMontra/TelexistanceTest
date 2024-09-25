using UnityEngine;
using Zenject;

namespace SETHD.Timer
{
    public class TimerFacadeFactory<T> : IFactory<T, Object, TimerFacade<T>>
    {
        private readonly DiContainer container;
        
        public TimerFacade<T> Create(T param1, Object prefab)
        {
            var instance = container.InstantiatePrefabForComponent<TimerFacade<T>>(prefab);
            return instance;
        }
    }
}