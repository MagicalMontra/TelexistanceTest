using Zenject;
using UnityEngine;
using SETHD.UI.CountDownTimer;

namespace SETHD.App.CountDownTimer
{
    public class CountDownTimerInstaller : MonoInstaller<CountDownTimerInstaller>
    {
        [SerializeField]
        private CountDownUIInstaller uiInstaller;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Timer.CountDownTimer>().AsSingle();
            Container.BindInterfacesAndSelfTo<CountDownTimerController>().AsSingle();
            Container.Bind<CountDownTimerUI>().FromSubContainerResolve().ByNewContextPrefab(uiInstaller).AsSingle();
        }
    }
}