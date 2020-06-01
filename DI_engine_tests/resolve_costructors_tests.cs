using Microsoft.VisualStudio.TestTools.UnitTesting;
using DI_engine;
using System;

namespace DI_engine_tests
{
    [TestClass]
    public class ResolveConstructorsTests
    {
        [TestMethod]
        public void ClassWithOneParameterConstructorCreation()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithSimpleField a = c.Resolve<ClassWithSimpleField>();

            Assert.IsNotNull(a.foo);
        }

        [TestMethod]
        public void ClassWithPrivateConstructorCreation()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                var a = c.Resolve<ClassWithPrivateConstructor>();
            });
        }

        [TestMethod]
        public void ClassWithTwoConstructorsCreation()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithTwoConstructors a = c.Resolve<ClassWithTwoConstructors>();

            Assert.IsNotNull(a.foo);
        }

        [TestMethod]
        public void ClassWithTwoLongestConstructorsCreation()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                var a = c.Resolve<ClassWithTwoLongestConstructors>();
            });
        }

        [TestMethod]
        public void ClassWithConstructorParameterCreatedBefore()
        {
            SimpleContainer c = new SimpleContainer();

            Foo foo = new Foo();
            c.RegisterInstance<Foo>(foo);

            ClassWithSimpleField a = c.Resolve<ClassWithSimpleField>();

            Assert.AreEqual(a.foo, foo);
        }
    }
}
