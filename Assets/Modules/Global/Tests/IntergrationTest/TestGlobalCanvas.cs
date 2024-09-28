using Zenject;
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace SETHD.Global.IntergrationTest
{
    public class TestGlobalCanvas : ZenjectIntegrationTestFixture
    {
        [Inject]
        private GlobalCanvas canvas;
        
        private void CommonInstall()
        {
            PreInstall();
            PostInstall();
        }
        
        [UnityTest]
        public IEnumerator TestResolve()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            Assert.IsNotNull(canvas);
        }
        
        [UnityTest]
        public IEnumerator TestRectTransform()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            Assert.IsNotNull(canvas.RectTransform);
        }
        
        [UnityTest]
        public IEnumerator TestEventSystem()
        {
            CommonInstall();
            yield return new WaitForEndOfFrame();
            Assert.IsNotNull(canvas.EventSystem);
        }
    }
}
