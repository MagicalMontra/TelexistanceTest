using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace SETHD.Global
{
    public class GlobalCanvasInstaller : MonoInstaller<GlobalCanvasInstaller>
    {
        [SerializeField]
        private EventSystem eventSystem;
        
        [SerializeField]
        private RectTransform rectTransform;
        
        public override void InstallBindings()
        {
            Container.Bind<GlobalCanvas>().AsSingle();
            Container.Bind<EventSystem>().FromComponentInNewPrefab(eventSystem).AsSingle();
            Container.Bind<RectTransform>().FromComponentInNewPrefab(rectTransform).AsSingle();
        }
    }
}