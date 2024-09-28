using Zenject;
using UnityEngine;
using SETHD.Global;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace SETHD.UI.CountDownTimer.IntergrationTest
{
    public class TestCountDownTimerUI : ZenjectIntegrationTestFixture
    {
        [Inject]
        private GlobalCanvas globalCanvas;
        
        private void CommonInstall()
        {
            PreInstall();
            PostInstall();
            Container.Bind<CountDownTimerUI>().FromComponentInNewPrefabResource("TestCountDownTimerUI").UnderTransform(globalCanvas.RectTransform).AsSingle();
        }
        
        [UnityTest]
        public IEnumerator TestResolve()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            var ui = Container.Resolve<CountDownTimerUI>();
            Assert.IsNotNull(ui);
        }
        
        [UnityTest]
        public IEnumerator TestParent()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            var ui = Container.Resolve<CountDownTimerUI>();
            Assert.That(ui.transform.parent == globalCanvas.RectTransform);
        }
        
        [UnityTest]
        public IEnumerator TestComponents()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            var ui = Container.Resolve<CountDownTimerUI>();
            Assert.IsNotNull(ui.StopButton);
            Assert.IsNotNull(ui.PauseButton);
            Assert.IsNotNull(ui.StartButton);
            Assert.IsNotNull(ui.PauseButtonText);
        }
    }
}
