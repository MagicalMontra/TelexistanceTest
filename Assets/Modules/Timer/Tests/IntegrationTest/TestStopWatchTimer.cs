using UniRx;
using Zenject;
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using System.Collections.Generic;

namespace SETHD.Timer.IntegrationTest
{
    public class TestStopWatchTimer : ZenjectIntegrationTestFixture
    {
        [Inject]
        private ITimer<List<float>> timer;
        
        private void CommonInstall()
        {
            PreInstall();
            
            Container.BindInterfacesAndSelfTo<LapStopWatch>().AsSingle();

            PostInstall();
        }
        
        [UnityTest]
        public IEnumerator TestPause()
        {
            CommonInstall();
            timer.Observable.Subscribe(time => Debug.Log(time));
            timer.Initialize();
            timer.Start();
            
            yield return new WaitForSeconds(0.5f);
            
            timer.Pause();
            
            Assert.Less(timer.Time.Value[0], 0.58f);
            Assert.Greater(timer.Time.Value[0], 0.4999f);
        }
        
        [UnityTest]
        public IEnumerator TestUnPause()
        {
            CommonInstall();
            timer.Observable.Subscribe(time => Debug.Log(time));
            timer.Initialize();
            timer.Start();
            
            yield return new WaitForSeconds(0.5f);
            
            timer.Pause();
            
            yield return new WaitForSeconds(0.25f);
            
            timer.Pause();
            
            yield return new WaitForSeconds(0.5f);
            
            Assert.Less(timer.Time.Value[0], 1.08f);
            Assert.Greater(timer.Time.Value[0], 0.999f);
        }
        
        [UnityTest]
        public IEnumerator TestOnTick()
        {
            CommonInstall();
            float seconds = 1f;
            timer.Observable.Subscribe();
            timer.Initialize();
            timer.Start();
            
            yield return new WaitForSeconds(0.1f);

            Assert.Less(timer.Time.Value[0], seconds);
        }
        
        [UnityTest]
        [TestCase(1f, ExpectedResult = null)]
        [TestCase(2f, ExpectedResult = null)]
        [TestCase(3f, ExpectedResult = null)]
        [TestCase(4f, ExpectedResult = null)]
        [TestCase(5f, ExpectedResult = null)]
        public IEnumerator TestSeconds(float seconds)
        {
            CommonInstall();
            
            timer.Observable.Subscribe(time => Debug.Log(time), () => Debug.Log(timer.Time.Value));
            timer.Initialize();
            timer.Start();
            
            yield return new WaitForSeconds(seconds);
            // yield return new WaitForSeconds(0.5f);
            
            Assert.Less(timer.Time.Value[0], seconds + 0.08f);
            Assert.Greater(timer.Time.Value[0], seconds - 0.001f);
        }
        
        [UnityTest]
        public IEnumerator TestLap()
        {
            CommonInstall();
            timer.Observable.Subscribe();
            timer.Initialize();
            timer.Start();
            
            yield return new WaitForSeconds(0.1f);
            
            timer.Start();
            float snapShotValue = timer.Time.Value[^1];
            Assert.Less(snapShotValue, 0.18f);
            
            yield return new WaitForSeconds(0.1f);
            
            timer.Start();
            snapShotValue = timer.Time.Value[^1];
            Assert.Less(snapShotValue, 0.18f);
            
            yield return new WaitForSeconds(0.1f);
            
            timer.Start();
            snapShotValue = timer.Time.Value[^1];
            Assert.Less(snapShotValue, 0.18f);
        }
        
        [UnityTest]
        public IEnumerator TestReset()
        {
            CommonInstall();
            timer.Observable.Subscribe();
            timer.Initialize();
            timer.Start();
            
            yield return new WaitForSeconds(0.1f);
            
            timer.Start();
            float snapShotValue = timer.Time.Value[^1];
            Assert.Less(snapShotValue, 0.18f);
            
            yield return new WaitForSeconds(0.1f);
            
            timer.Start();
            snapShotValue = timer.Time.Value[^1];
            Assert.Less(snapShotValue, 0.18f);
            
            yield return new WaitForSeconds(0.1f);
            
            timer.Start();
            snapShotValue = timer.Time.Value[^1];
            Assert.Less(snapShotValue, 0.18f);
            
            yield return new WaitForSeconds(0.1f);
            
            timer.Stop();
            Assert.That(timer.Time.Value.Count == 0);
        }
    }
}