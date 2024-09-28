using Zenject;
using UnityEngine;
using SETHD.Global;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace SETHD.UI.LapStopWatch.IntergrationTest
{
    public class TestLapStopWatchUI : ZenjectIntegrationTestFixture
    {
        [Inject]
        private GlobalCanvas globalCanvas;
        
        private void CommonInstall()
        {
            PreInstall();
            PostInstall();
            Container.Bind<LapStopWatchUI>().FromComponentInNewPrefabResource("TestLapStopWatchUI").UnderTransform(globalCanvas.RectTransform).AsSingle();
            Container.BindFactory<Object, Transform, LapTimeItem, LapTimeItem.Factory>().FromFactory<LapTimeItemFactory>();
        }
        
        [UnityTest]
        public IEnumerator TestResolve()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            var ui = Container.Resolve<LapStopWatchUI>();
            Assert.IsNotNull(ui);
        }
        
        [UnityTest]
        public IEnumerator TestParent()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            var ui = Container.Resolve<LapStopWatchUI>();
            Assert.That(ui.transform.parent == globalCanvas.RectTransform);
        }
        
        [UnityTest]
        public IEnumerator TestComponents()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            var ui = Container.Resolve<LapStopWatchUI>();
            Assert.IsNotNull(ui.LapButton);
            Assert.IsNotNull(ui.StopButton);
            Assert.IsNotNull(ui.StartButton);
            Assert.IsNotNull(ui.ResetButton);
            Assert.IsNotNull(ui.LapTimeText);
            Assert.IsNotNull(ui.TotalTimeText);
            Assert.IsNotNull(ui.StopButtonText);
        }
        
        [UnityTest]
        public IEnumerator TestLapTimeItemFactory()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            var factory = Container.Resolve<LapTimeItem.Factory>();
            Assert.IsNotNull(factory);
        }
    }
}
