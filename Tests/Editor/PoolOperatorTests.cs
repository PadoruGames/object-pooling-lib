using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Padoru.ObjectPooling.Tests
{
    [TestFixture]
    public class PoolOperatorTests
    {
        private class TestClass { }

        [Test]
        public void GetObject_WhenPoolObjectsNull_ShouldThrow()
        {
            var maxCapacity = 2;
            List<TestClass> poolObjects = null;
            List<TestClass> usedObjects = new List<TestClass>();
            var poolOperator = new PoolOperator<TestClass>();

            Assert.Throws<Exception>(() => poolOperator.GetObject("", poolObjects, usedObjects, maxCapacity, CreateObjectCallback));
        }

        [Test]
        public void GetObject_WhenUsedObjectsNull_ShouldThrow()
        {
            var maxCapacity = 2;
            List<TestClass> poolObjects = new List<TestClass>();
            List<TestClass> usedObjects = null;
            var poolOperator = new PoolOperator<TestClass>();

            Assert.Throws<Exception>(() => poolOperator.GetObject("", poolObjects, usedObjects, maxCapacity, CreateObjectCallback));
        }

        [Test]
        public void GetObject_WhenObjectNull_ShouldThrowAndRemoveFromList()
        {
            var maxCapacity = 2;
            List<TestClass> poolObjects = new List<TestClass>();
            List<TestClass> usedObjects = new List<TestClass>();
            var poolOperator = new PoolOperator<TestClass>();

            poolObjects.Add(NullCreateObjectCallback());

            var previousCount = poolObjects.Count;

            Assert.Throws<Exception>(() => poolOperator.GetObject("", poolObjects, usedObjects, maxCapacity, CreateObjectCallback));
            Assert.AreEqual(poolObjects.Count, previousCount - 1);
        }

        [Test]
        public void GetObject_WhenHasAmountAndHasCapacity_ShouldNotBeNullAndShouldBeUsed()
        {
            var maxCapacity = 2;
            List<TestClass> poolObjects = new List<TestClass>();
            List<TestClass> usedObjects = new List<TestClass>();
            var poolOperator = new PoolOperator<TestClass>();

            poolObjects.Add(CreateObjectCallback());

            var obj = poolOperator.GetObject("", poolObjects, usedObjects, maxCapacity, CreateObjectCallback);

            Assert.IsNotNull(obj);
            Assert.IsTrue(usedObjects.Contains(obj));
        }

        [Test]
        public void GetObject_WhenDoesNotHaveAmountAndHasCapacity_ShouldNotBeNullAndShouldBeUsed()
        {
            var maxCapacity = 1;
            List<TestClass> poolObjects = new List<TestClass>();
            List<TestClass> usedObjects = new List<TestClass>();
            var poolOperator = new PoolOperator<TestClass>();

            var obj = poolOperator.GetObject("", poolObjects, usedObjects, maxCapacity, CreateObjectCallback);

            Assert.IsNotNull(obj);
            Assert.IsTrue(usedObjects.Contains(obj));
        }

        [Test]
        public void GetObject_WhenDoesNotHaveAmountAndDoesNotHaveCapacity_ShouldThrow()
        {
            var maxCapacity = 0;
            List<TestClass> poolObjects = new List<TestClass>();
            List<TestClass> usedObjects = new List<TestClass>();
            var poolOperator = new PoolOperator<TestClass>();

            Assert.Throws<Exception>(() => poolOperator.GetObject("", poolObjects, usedObjects, maxCapacity, CreateObjectCallback));
        }

        [Test]
        public void GetObject_WhenDoesNotHaveAmountAndHasCapacityAndCreateObjectCallbackReturnsNull_ShouldThrow()
        {
            var maxCapacity = 1;
            List<TestClass> poolObjects = new List<TestClass>();
            List<TestClass> usedObjects = new List<TestClass>();
            var poolOperator = new PoolOperator<TestClass>();

            Assert.Throws<Exception>(() => poolOperator.GetObject("", poolObjects, usedObjects, maxCapacity, NullCreateObjectCallback));
        }

        [Test]
        public void ReturnObject_WhenPoolObjectsNull_ShouldThrow()
        {
            List<TestClass> poolObjects = null;
            List<TestClass> usedObjects = new List<TestClass>();
            var poolOperator = new PoolOperator<TestClass>();

            usedObjects.Add(CreateObjectCallback());

            Assert.Throws<Exception>(() => poolOperator.ReturnObject(usedObjects[0], "", poolObjects, usedObjects));
        }

        [Test]
        public void ReturnObject_WhenUsedObjectsNull_ShouldThrow()
        {
            List<TestClass> poolObjects = new List<TestClass>();
            List<TestClass> usedObjects = null;
            var poolOperator = new PoolOperator<TestClass>();

            Assert.Throws<Exception>(() => poolOperator.ReturnObject(null, "", poolObjects, usedObjects));
        }

        [Test]
        public void ReturnObject_WhenObjectNull_ShouldThrow()
        {
            List<TestClass> poolObjects = new List<TestClass>();
            List<TestClass> usedObjects = new List<TestClass>();
            var poolOperator = new PoolOperator<TestClass>();

            Assert.Throws<Exception>(() => poolOperator.ReturnObject(null, "", poolObjects, usedObjects));
        }

        [Test]
        public void ReturnObject_WhenObjectBelongs_ShouldNotThrow()
        {
            List<TestClass> poolObjects = new List<TestClass>();
            List<TestClass> usedObjects = new List<TestClass>();
            var poolOperator = new PoolOperator<TestClass>();

            usedObjects.Add(CreateObjectCallback());

            Assert.DoesNotThrow(() => poolOperator.ReturnObject(usedObjects[0], "", poolObjects, usedObjects));
        }

        [Test]
        public void ReturnObject_WhenObjectDoesNotBelong_ShouldThrow()
        {
            List<TestClass> poolObjects = new List<TestClass>();
            List<TestClass> usedObjects = new List<TestClass>();
            var poolOperator = new PoolOperator<TestClass>();

            Assert.Throws<Exception>(() => poolOperator.ReturnObject(CreateObjectCallback(), "", poolObjects, usedObjects));
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