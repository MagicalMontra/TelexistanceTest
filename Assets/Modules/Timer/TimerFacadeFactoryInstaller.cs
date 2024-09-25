using UnityEngine;
using Zenject;

namespace SETHD.Timer
{
    public class TimerFacadeFactoryInstaller<T> : Installer<TimerFacadeFactoryInstaller<T>>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<T, Object, TimerFacade<T>, TimerFacadePlaceholderFactory<T>>().FromFactory<TimerFacadeFactory<T>>();
        }
    }
}