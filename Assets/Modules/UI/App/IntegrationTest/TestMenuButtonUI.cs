using Zenject;
using UnityEngine;
using SETHD.Global;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace SETHD.UI.App.IntergrationTest
{
    public class TestMenuButtonUI : ZenjectIntegrationTestFixture
    {
        [Inject]
        private GlobalCanvas globalCanvas;
        
        private void CommonInstall()
        {
            PreInstall();
            PostInstall();
            Container.Bind<MenuButtonUI>().FromComponentInNewPrefabResource("TestMenuButtonUI").UnderTransform(globalCanvas.RectTransform).AsSingle();
        }
        
        [UnityTest]
        public IEnumerator TestResolve()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            var ui = Container.Resolve<MenuButtonUI>();
            Assert.IsNotNull(ui);
        }
        
        [UnityTest]
        public IEnumerator TestParent()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            var ui = Container.Resolve<MenuButtonUI>();
            Assert.That(ui.transform.parent == globalCanvas.RectTransform);
        }
        
        [UnityTest]
        public IEnumerator TestComponents()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            var ui = Container.Resolve<MenuButtonUI>();
            Assert.IsNotNull(ui.TimerButton);
            Assert.IsNotNull(ui.StopWatchButton);
            Assert.IsNotNull(ui.TimerButtonText);
            Assert.IsNotNull(ui.StopWatchButtonText);
        }
    }
}
