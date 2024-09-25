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
            SignalBusInstaller.Install(Container);
            Container.Bind<GlobalCanvas>().FromSubContainerResolve().ByNewContextPrefab(globalCanvasInstaller).AsSingle().NonLazy();
        }
    }
}
