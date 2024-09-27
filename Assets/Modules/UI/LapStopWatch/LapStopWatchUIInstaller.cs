using SETHD.Global;
using UnityEngine;
using Zenject;

namespace SETHD.UI.LapStopWatch
{
    public class LapStopWatchUIInstaller : MonoInstaller<LapStopWatchUIInstaller>
    {
        [Inject]
        private GlobalCanvas globalCanvas;

        [SerializeField]
        private LapStopWatchUI prefab;
        
        public override void InstallBindings()
        {
            Container.Bind<LapStopWatchUI>().FromComponentsInNewPrefab(prefab).UnderTransform(globalCanvas.RectTransform).AsSingle().NonLazy();
        }
    }
}