using TMPro;
using Zenject;
using UnityEngine;

namespace SETHD.Global
{
    [CreateAssetMenu(menuName = "Create ProjectInstaller", fileName = "ProjectInstaller", order = 0)]
    public class ProjectInstaller : ScriptableObjectInstaller<ProjectInstaller>
    {
        [SerializeField]
        private GlobalCanvasInstaller globalCanvasInstaller;
        
        public override void InstallBindings()
        {
            Application.targetFrameRate = 120;
            SignalBusInstaller.Install(Container);
            Container.Bind<GlobalCanvas>().FromSubContainerResolve().ByNewContextPrefab(globalCanvasInstaller).AsSingle().NonLazy();
            Container.BindFactory<Object, TextMeshProUGUI, PlaceholderFactory<Object, TextMeshProUGUI>>().FromFactory<PrefabFactory<TextMeshProUGUI>>();
        }
    }
}
