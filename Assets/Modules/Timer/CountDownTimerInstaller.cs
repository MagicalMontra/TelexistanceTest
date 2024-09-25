using System;
using System.Globalization;
using SETHD.UI.CountDownTimer;
using UniRx;
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

    public class CountDownTimerController : IInitializable, ILateDisposable
    {
        private readonly ITimer<float> timer;
        private readonly CountDownTimerUI ui;

        private IDisposable textDisposable;
        private IDisposable stopDisposable;
        private IDisposable startDisposable;
        private IDisposable pauseDisposable;
        
        public CountDownTimerController(ITimer<float> timer, CountDownTimerUI ui)
        {
            this.timer = timer;
            this.ui = ui;
        }

        public void Initialize()
        {
            timer.Initialize(10);
            stopDisposable = ui.StopButton.OnClickAsObservable().Subscribe(_ => timer.Stop());
            startDisposable = ui.StartButton.OnClickAsObservable().Subscribe(_ => timer.Start());
            pauseDisposable = ui.PauseButton.OnClickAsObservable().Subscribe(_ => timer.Pause());
            textDisposable = timer.Time.Subscribe(time => ui.TimeText.text = time.ToString(CultureInfo.InvariantCulture));
            timer.Observable.Subscribe(time => Debug.Log(time), () => Debug.Log("completed"));
        }

        public void LateDispose()
        {
            textDisposable.Dispose();
            stopDisposable.Dispose();
            startDisposable.Dispose();
            pauseDisposable.Dispose();
        }
    }
}