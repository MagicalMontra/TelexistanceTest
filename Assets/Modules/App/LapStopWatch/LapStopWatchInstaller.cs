using Zenject;
using UnityEngine;
using SETHD.UI.LapStopWatch;

namespace SETHD.App.LapStopWatch
{
    public class LapStopWatchInstaller : MonoInstaller<LapStopWatchInstaller>
    {
        [SerializeField]
        private LapStopWatchUIInstaller uiInstaller;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Timer.LapStopWatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<LapStopWatchController>().AsSingle();
            Container.Bind<LapStopWatchUI>().FromSubContainerResolve().ByNewContextPrefab(uiInstaller).AsSingle();
        }
    }
}