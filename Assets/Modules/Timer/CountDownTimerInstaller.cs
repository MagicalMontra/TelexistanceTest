using Zenject;

namespace SETHD.Timer
{
    public class CountDownTimerInstaller : MonoInstaller<CountDownTimerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CountDownTimer>().AsSingle().NonLazy();
        }
    }

    public class CountDownTimerController : IInitializable, ILateDisposable
    {
        private readonly ITimer<float> timer;


        public void Initialize()
        {
            
        }

        public void LateDispose()
        {
            
        }
    }
}