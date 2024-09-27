using Zenject;
using UnityEngine;
using SETHD.Global;
using SETHD.UI.App;
using SETHD.UI.Time;
using SETHD.App.LapStopWatch;
using SETHD.App.CountDownTimer;

namespace SETHD.App
{
    public class AppInstaller : MonoInstaller<AppInstaller>
    {
        [Inject]
        private GlobalCanvas globalCanvas;
        
        [SerializeField]
        private MenuButtonUI menuButtonUIPrefab;
        
        [SerializeField]
        private TimeUIInstaller timeUIInstaller;
        
        [SerializeField]
        private LapStopWatchInstaller lapStopWatchInstaller;
        
        [SerializeField]
        private CountDownTimerInstaller countDownTimerInstaller;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AppController>().AsSingle();
            Container.Bind<TimeUI>().FromSubContainerResolve().ByNewContextPrefab(timeUIInstaller).AsSingle();
            Container.Bind<LapStopWatchController>().FromSubContainerResolve().ByNewContextPrefab(lapStopWatchInstaller).AsSingle();
            Container.Bind<CountDownTimerController>().FromSubContainerResolve().ByNewContextPrefab(countDownTimerInstaller).AsSingle();
            Container.Bind<MenuButtonUI>().FromComponentInNewPrefab(menuButtonUIPrefab).UnderTransform(globalCanvas.RectTransform).AsSingle();
        }
    }
}