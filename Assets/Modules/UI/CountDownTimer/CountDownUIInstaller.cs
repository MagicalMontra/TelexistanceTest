using Zenject;
using SETHD.Global;
using UnityEngine;

namespace SETHD.UI.CountDownTimer
{
    public class CountDownUIInstaller : MonoInstaller<CountDownUIInstaller>
    {
        [Inject]
        private GlobalCanvas globalCanvas;

        [SerializeField]
        private CountDownTimerUI prefab;
        
        public override void InstallBindings()
        {
            Container.Bind<CountDownTimerUI>().FromComponentsInNewPrefab(prefab).UnderTransform(globalCanvas.RectTransform).AsSingle().NonLazy();
        }
    }
}