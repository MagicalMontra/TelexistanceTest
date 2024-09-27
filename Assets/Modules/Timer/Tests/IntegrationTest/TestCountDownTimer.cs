using UniRx;
using Zenject;
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace SETHD.Timer.IntegrationTest
{
    public class TextCountDownTimer : ZenjectIntegrationTestFixture
    {
        [Inject]
        private ITimer<float> timer;
        
        private void CommonInstall()
        {
            PreInstall();
            
            Container.BindInterfacesAndSelfTo<CountDownTimer>().AsSingle();

            PostInstall();
        }
        
        [UnityTest]
        public IEnumerator TestPause()
        {
            CommonInstall();
            bool isCompleted = false;
            timer.Observable.Subscribe(time => Debug.Log(time), () => isCompleted = true);
            timer.Initialize(1f);
            timer.Start();
            
            yield return new WaitForSeconds(0.5f);
            
            timer.Pause();
            
            Assert.That(Mathf.Approximately(timer.Time.Value, 0.5f));
        }
        
        [UnityTest]
        public IEnumerator TestUnPause()
        {
            CommonInstall();
            bool isCompleted = false;
            timer.Observable.Subscribe(time => Debug.Log(time), () => isCompleted = true);
            timer.Initialize(1f);
            timer.Start();
            
            yield return new WaitForSeconds(0.5f);
            
            timer.Pause();
            
            yield return new WaitForEndOfFrame();
            
            timer.Pause();
            
            yield return new WaitForSeconds(0.5f);
            
            Assert.That(Mathf.Approximately(timer.Time.Value, 0f));
        }
        
        [UnityTest]
        public IEnumerator TestComplete()
        {
            CommonInstall();
            bool isCompleted = false;
            timer.Observable.Subscribe(time => Debug.Log(time), () => isCompleted = true);
            timer.Initialize(1f);
            timer.Start();
            
            yield return new WaitForSeconds(1.5f);
            yield return new WaitForEndOfFrame();
            
            Assert.IsTrue(isCompleted);
        }
        
        [UnityTest]
        [TestCase(1F, ExpectedResult = null)]
        [TestCase(2F, ExpectedResult = null)]
        [TestCase(3F, ExpectedResult = null)]
        [TestCase(4F, ExpectedResult = null)]
        [TestCase(5F, ExpectedResult = null)]
        public IEnumerator TestSeconds(float seconds)
        {
            CommonInstall();
            
            timer.Observable.Subscribe(time => Debug.Log(time), () => Debug.Log(timer.Time.Value));
            timer.Initialize(seconds);
            timer.Start();
            
            yield return new WaitForSeconds(seconds);
            yield return new WaitForSeconds(0.5f);
            
            Assert.AreEqual(0f, timer.Time.Value);
        }
        
        [UnityTest]
        public IEnumerator TestNegative()
        {
            CommonInstall();
            
            timer.Observable.Subscribe(time => Debug.Log(time), () => Debug.Log(timer.Time.Value));
            timer.Initialize(-1f);
            timer.Start();
            
            yield return new WaitForSeconds(1.5f);
            yield return new WaitForEndOfFrame();
            
            Assert.AreEqual(0f, timer.Time.Value);
        }
    }
}
