using Microsoft.VisualStudio.TestTools.UnitTesting;
using DI_engine;
using System;

namespace DI_engine_tests
{
    [TestClass]
    public class BuildingUpInstancesTests
    {
        [TestMethod]
        public void PropertyWithSetterAndGetterBuildUp()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithProperties a = new ClassWithProperties();
            c.BuildUp(a);

            Assert.IsNotNull(a.TheFoo);
        }

        [TestMethod]
        public void PropertyWithGetterOnlyBuildUpShouldFail()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithProperties a = new ClassWithProperties();
            c.BuildUp(a);

            Assert.IsNull(a.TheFoo2);
        }

        [TestMethod]
        public void PropertyBuildUpWithoutAttributeShouldFail()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithProperties a = new ClassWithProperties();
            c.BuildUp(a);

            Assert.IsNull(a.TheFoo3);
        }

        [TestMethod]
        public void BuildUpOfTwoProperties()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithProperties a = new ClassWithProperties();
            c.BuildUp(a);

            Assert.IsNotNull(a.TheFoo);
            Assert.IsNotNull(a.TheBar);
        }

        [TestMethod]
        public void BuildUpOfOnePropertyAndNotChangeTheInitializedOne()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithProperties a = new ClassWithProperties();

            Foo foo = new Foo();
            a.TheFoo = foo;

            c.BuildUp(a);

            Assert.AreEqual(a.TheFoo, foo);
            Assert.IsNotNull(a.TheBar);
        }

        [TestMethod]
        public void BuildUpOfNonResolvableTypeShouldFail()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithStringProperty a = new ClassWithStringProperty();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                c.BuildUp(a);
            });
        }

        [TestMethod]
        public void BuldUpOfTypeRegisteredBefore()
        {
            SimpleContainer c = new SimpleContainer();

            string s = "abc";
            c.RegisterInstance<string>(s);

            ClassWithStringProperty a = new ClassWithStringProperty();

            c.BuildUp(a);

            Assert.AreEqual(a.TheString, s);
        }

        [TestMethod]
        public void BuildUpOfInterfaceTypeProperty()
        {
            SimpleContainer c = new SimpleContainer();


            ClassWithInterfaceAsProperty a = new ClassWithInterfaceAsProperty();

            c.RegisterType<IBaz, Baz>(false);
            c.BuildUp(a);

            Assert.IsNotNull(a.TheIbaz);
        }

        [TestMethod]
        public void BuildUpOfNonRegisteredInterfaceTypePropertyShouldFail()
        {
            SimpleContainer c = new SimpleContainer();
            ClassWithInterfaceAsProperty a = new ClassWithInterfaceAsProperty();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                c.BuildUp(a);
            });
        }

        [TestMethod]
        public void BuildUpOfTypeRegistredAsSingletonInTwoObjects()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo>(true);

            ClassWithProperties a1 = new ClassWithProperties();
            ClassWithProperties a2 = new ClassWithProperties();

            c.BuildUp(a1);
            c.BuildUp(a2);

            Assert.IsNotNull(a1.TheFoo);
            Assert.AreEqual(a1.TheFoo, a2.TheFoo);
        }

        [TestMethod]
        public void BuildUpOfTypeRegistredAsSingletonInOneObject()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo>(true);

            ClassWithTwoPropertiesOfSameType a = new ClassWithTwoPropertiesOfSameType();
            c.BuildUp(a);

            Assert.IsNotNull(a.TheFoo);
            Assert.AreEqual(a.TheFoo, a.TheFoo2);
        }

        [TestMethod]
        [Timeout(2000)]
        public void BuildUpOfCyclicProperty()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithCyclicProperty a = new ClassWithCyclicProperty();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                c.BuildUp(a);
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void BuildUpOfIndirectCyclicClassAsProperty()
        {
            SimpleContainer c = new SimpleContainer();

            ClassWithIndirectCyclicClassAsProperty a = new ClassWithIndirectCyclicClassAsProperty();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                c.BuildUp(a);
            });
        }
    }
}
