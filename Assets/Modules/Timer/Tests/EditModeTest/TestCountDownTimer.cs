using Zenject;
using UnityEngine;
using NUnit.Framework;

namespace SETHD.Timer.EditModeTest
{
    [TestFixture]
    public class TestCountDownTimer : ZenjectUnitTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.BindInterfacesAndSelfTo<CountDownTimer>().AsSingle();
        }
        
        [Test]
        public void TestBinding()
        {
            var timer = Container.Resolve<ITimer<float>>();
            Assert.IsNotNull(timer);
        }

        [Test]
        public void TestInitialize()
        {
            var timer = Container.Resolve<ITimer<float>>();
            timer.Initialize(10);
            Assert.That(Mathf.Approximately(timer.Time.Value, 10));
        }
    }
}
