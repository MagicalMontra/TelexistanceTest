using SETHD.Global;
using UnityEngine;
using Zenject;

namespace SETHD.UI.Time
{
    public class TimeUIInstaller : MonoInstaller<TimeUIInstaller>
    {
        [Inject]
        private GlobalCanvas globalCanvas;

        [SerializeField]
        private TimeUI prefab; 
        
        public override void InstallBindings()
        {
            Container.Bind<TimeUI>().FromComponentInNewPrefab(prefab).UnderTransform(globalCanvas.RectTransform).AsSingle();
        }
    }
}