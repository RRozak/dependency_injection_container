using Microsoft.VisualStudio.TestTools.UnitTesting;
using DI_engine;
using System;

namespace DI_engine_tests
{
    [TestClass]
    public class PropertiesInjectionTests
    {
        [TestMethod]
        public void PropertyWithSetterAndGetterInjection()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithProperties a = c.Resolve<ClassWithProperties>();

            Assert.IsNotNull(a.TheFoo);
        }

        [TestMethod]
        public void PropertyWithGetterOnlyInjectionShouldFail()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithProperties a = c.Resolve<ClassWithProperties>();

            Assert.IsNull(a.TheFoo2);
        }

        [TestMethod]
        public void PropertyInjectionWithoutAttributeShouldFail()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithProperties a = c.Resolve<ClassWithProperties>();

            Assert.IsNull(a.TheFoo3);
        }

        [TestMethod]
        public void InjectionOfTwoProperties()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithProperties a = c.Resolve<ClassWithProperties>();

            Assert.IsNotNull(a.TheFoo);
            Assert.IsNotNull(a.TheBar);
        }

        [TestMethod]
        public void InjectionOfNonResolvableTypeShouldFail()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                ClassWithStringProperty a = c.Resolve<ClassWithStringProperty>();
            });
        }

        [TestMethod]
        public void InjectionOfTypeRegisteredBefore()
        {
            SimpleContainer c = new SimpleContainer();

            string s = "abc";
            c.RegisterInstance<string>(s);

            ClassWithStringProperty a = c.Resolve<ClassWithStringProperty>();

            Assert.AreEqual(a.TheString, s);
        }

        [TestMethod]
        public void InjectionOfInterfaceTypeProperty()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<IBaz, Baz>(false);
            ClassWithInterfaceAsProperty a = c.Resolve<ClassWithInterfaceAsProperty>();

            Assert.IsNotNull(a.TheIbaz);
        }

        [TestMethod]
        public void InjectionOfNonRegisteredInterfaceTypePropertyShouldFail()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                ClassWithInterfaceAsProperty a = c.Resolve<ClassWithInterfaceAsProperty>();
            });
        }

        [TestMethod]
        public void InjectionOfTypeRegistredAsSingletonInTwoObjects()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo>(true);

            ClassWithProperties a1 = c.Resolve<ClassWithProperties>();
            ClassWithProperties a2 = c.Resolve<ClassWithProperties>();

            Assert.IsNotNull(a1.TheFoo);
            Assert.AreEqual(a1.TheFoo, a2.TheFoo);
        }

        [TestMethod]
        public void InjectionOfTypeRegistredAsSingletonInOneObject()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo>(true);

            ClassWithTwoPropertiesOfSameType a = c.Resolve<ClassWithTwoPropertiesOfSameType>();

            Assert.IsNotNull(a.TheFoo);
            Assert.AreEqual(a.TheFoo, a.TheFoo2);
        }

        [TestMethod]
        [Timeout(2000)]
        public void InjectionOfCyclicProperty()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                var a = c.Resolve<ClassWithCyclicProperty>();
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void InjectionOfIndirectCyclicClassAsProperty()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                var a = c.Resolve<ClassWithIndirectCyclicClassAsProperty>();
            });
        }
    }
}
