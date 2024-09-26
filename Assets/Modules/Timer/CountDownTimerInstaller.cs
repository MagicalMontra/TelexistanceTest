using SETHD.UI.CountDownTimer;
using UnityEngine;
using Zenject;

namespace SETHD.Timer
{
    public class CountDownTimerInstaller : MonoInstaller<CountDownTimerInstaller>
    {
        [SerializeField]
        private CountDownUIInstaller uiInstaller;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CountDownTimer>().AsSingle();
            Container.BindInterfacesAndSelfTo<CountDownTimerController>().AsSingle();
            Container.Bind<CountDownTimerUI>().FromSubContainerResolve().ByNewContextPrefab(uiInstaller).AsSingle();
        }
    }
}