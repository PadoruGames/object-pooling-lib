using NUnit.Framework;
using System;

namespace Padoru.ObjectPooling.Tests
{
    [TestFixture]
    public class PoolTests
    {
        private class TestClass { }

        [Test]
        public void GetObject_WhenHasAmountAndHasCapacity_ShouldNotBeNull()
        {
            var startAmount = 1;
            var maxCapacity = 2;
            var pool = new Pool<TestClass>("", startAmount, maxCapacity, CreateObjectCallback);

            var obj = pool.GetObject();

            Assert.IsNotNull(obj);
        }

        [Test]
        public void GetObject_WhenDoesNotHaveAmountAndHasCapacity_ShouldNotBeNull()
        {
            var startAmount = 0;
            var maxCapacity = 1;
            var pool = new Pool<TestClass>("", startAmount, maxCapacity, CreateObjectCallback);

            var obj = pool.GetObject();

            Assert.IsNotNull(obj);
        }

        [Test]
        public void GetObject_WhenDoesNotHaveAmountAndHasCapacityAndCreateObjectCallbackReturnsNull_ShouldThrow()
        {
            var startAmount = 0;
            var maxCapacity = 1;
            var pool = new Pool<TestClass>("", startAmount, maxCapacity, NullCreateObjectCallback);

            Assert.Throws<Exception>(() => pool.GetObject());
        }

        [Test]
        public void GetObject_WhenDoesNotHaveAmountAndDoesNotHaveCapacity_ShouldThrow()
        {
            var startAmount = 0;
            var maxCapacity = 0;
            var pool = new Pool<TestClass>("", startAmount, maxCapacity, CreateObjectCallback);

            Assert.Throws<Exception>(() => pool.GetObject());
        }

        [Test]
        public void ReturnObject_WhenBelongsToPool_ShouldNotThrow()
        {
            var startAmount = 1;
            var maxCapacity = 1;
            var pool = new Pool<TestClass>("", startAmount, maxCapacity, CreateObjectCallback);

            var obj = pool.GetObject();

            Assert.DoesNotThrow(() => pool.ReturnObject(obj));
        }

        [Test]
        public void ReturnObject_WhenDoesNotBelongToPool_ShouldThrow()
        {
            var startAmount = 1;
            var maxCapacity = 1;
            var pool = new Pool<TestClass>("", startAmount, maxCapacity, CreateObjectCallback);

            var obj = CreateObjectCallback();

            Assert.Throws<Exception>(() => pool.ReturnObject(obj));
        }

        [Test]
        public void ReturnObject_WhenNull_ShouldThrow()
        {
            var startAmount = 1;
            var maxCapacity = 1;
            var pool = new Pool<TestClass>("", startAmount, maxCapacity, CreateObjectCallback);

            Assert.Throws<Exception>(() => pool.ReturnObject(null));
        }

        private TestClass CreateObjectCallback()
        {
            return new TestClass();
        }

        private TestClass NullCreateObjectCallback()
        {
            return null;
        }
    }
}