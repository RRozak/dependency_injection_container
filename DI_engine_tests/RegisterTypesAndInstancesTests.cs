using Microsoft.VisualStudio.TestTools.UnitTesting;
using DI_engine;
using System;

namespace DI_engine_tests
{
    [TestClass]
    public class RegisterTypesAndInstancesTests
    {
        [TestMethod]
        public void SimpleClassCreation()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo>(false);

            Foo foo = c.Resolve<Foo>();
            Assert.IsNotNull(foo);
        }

        [TestMethod]
        public void SimpleSingletonCreation()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo>(true);

            Foo foo1 = c.Resolve<Foo>();
            Foo foo2 = c.Resolve<Foo>();

            Assert.IsNotNull(foo1);
            Assert.AreEqual(foo1, foo2);
        }

        [TestMethod]
        public void SimpleNonSingletonCreation()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo>(false);

            Foo foo1 = c.Resolve<Foo>();
            Foo foo2 = c.Resolve<Foo>();

            Assert.AreNotEqual(foo1, foo2);
        }

        [TestMethod]
        public void DerivingClassCreation()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo, Bar>(false);

            Foo bar = c.Resolve<Foo>();

            Assert.IsInstanceOfType(bar, typeof(Bar));
        }

        [TestMethod]
        public void DerivingSingletonClassCreation()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo, Bar>(true);

            Foo bar1 = c.Resolve<Foo>();
            Foo bar2 = c.Resolve<Foo>();

            Assert.IsInstanceOfType(bar1, typeof(Bar));
            Assert.AreEqual(bar1, bar2);
        }

        [TestMethod]
        public void DerivingNonSingletonClassCreation()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo, Bar>(false);

            Foo bar1 = c.Resolve<Foo>();
            Foo bar2 = c.Resolve<Foo>();

            Assert.AreNotEqual(bar1, bar2);
        }

        [TestMethod]
        public void UnregisteredTypeClassCreation()
        {
            SimpleContainer c = new SimpleContainer();

            Foo foo1 = c.Resolve<Foo>();
            Foo foo2 = c.Resolve<Foo>();

            Assert.IsNotNull(foo1);
            Assert.AreNotEqual(foo1, foo2);
        }

        [TestMethod]
        public void BaseClassChangeCreation()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo, Bar>(false);
            Foo bar = c.Resolve<Foo>();

            c.RegisterType<Foo, Bar2>(false);
            Foo bar2 = c.Resolve<Foo>();

            Assert.IsInstanceOfType(bar, typeof(Bar));
            Assert.IsInstanceOfType(bar2, typeof(Bar2));
        }

        [TestMethod]
        public void AttemptToInterfaceClassCreation()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() => { IBaz baz = c.Resolve<IBaz>(); });
        }

        [TestMethod]
        public void AttemptToAbstractClassCreation()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() => { AFoo foo = c.Resolve<AFoo>(); });
        }

        [TestMethod]
        public void RegistringRegistredTypeCLassCreation()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo, Bar>(false);
            c.RegisterType<Bar, FooBar>(false);

            Foo foobar = c.Resolve<Foo>();

            Assert.IsInstanceOfType(foobar, typeof(FooBar));
        }

        [TestMethod]
        public void RegisterInstance()
        {
            SimpleContainer c = new SimpleContainer();

            Foo foo1 = new Foo();
            c.RegisterInstance<Foo>(foo1);

            Foo foo2 = c.Resolve<Foo>();

            Assert.AreEqual(foo1, foo2);
        }

        [TestMethod]
        public void RegisterInterfaceInstance()
        {
            SimpleContainer c = new SimpleContainer();

            IBaz baz1 = new Baz();
            c.RegisterInstance<IBaz>(baz1);

            IBaz baz2 = c.Resolve<IBaz>();

            Assert.AreEqual(baz2, baz1);
        }

        [TestMethod]
        public void RegisterInstanceAfterInstance()
        {
            SimpleContainer c = new SimpleContainer();

            IBaz baz1 = new Baz();
            c.RegisterInstance<IBaz>(baz1);

            IBaz baz2 = new Baz();
            c.RegisterInstance<IBaz>(baz2);

            IBaz baz3 = c.Resolve<IBaz>();

            Assert.AreEqual(baz3, baz2);
            Assert.AreNotEqual(baz3, baz1);
        }

        [TestMethod]
        public void RegisterTypeNonSingletonAfterInstance()
        {
            SimpleContainer c = new SimpleContainer();

            IBaz baz1 = new Baz();
            c.RegisterInstance<IBaz>(baz1);

            c.RegisterType<IBaz, Baz>(false);

            IBaz baz2 = c.Resolve<IBaz>();
            IBaz baz3 = c.Resolve<IBaz>();

            Assert.IsInstanceOfType(baz2, typeof(Baz));
            Assert.AreNotEqual(baz2, baz1);
            Assert.AreNotEqual(baz3, baz2);
        }

        [TestMethod]
        public void RegisterTypeSingletonAfterInstance()
        {
            SimpleContainer c = new SimpleContainer();

            IBaz baz1 = new Baz();
            c.RegisterInstance<IBaz>(baz1);

            c.RegisterType<IBaz, Baz>(true);

            IBaz baz2 = c.Resolve<IBaz>();
            IBaz baz3 = c.Resolve<IBaz>();

            Assert.IsInstanceOfType(baz2, typeof(Baz));
            Assert.AreNotEqual(baz2, baz1);
            Assert.AreEqual(baz3, baz2);
        }

        [TestMethod]
        public void RegisterInstanceAfterType()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<IBaz, Baz>(true);
            IBaz baz1 = c.Resolve<IBaz>();

            IBaz baz2 = new Baz();
            c.RegisterInstance<IBaz>(baz2);

            IBaz baz3 = c.Resolve<IBaz>();

            Assert.AreNotEqual(baz2, baz1);
            Assert.AreEqual(baz3, baz2);
        }

        [TestMethod]
        public void RegisterSingletonAfterNonSingleton()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Baz>(false);
            Baz baz1 = c.Resolve<Baz>();

            c.RegisterType<Baz>(true);
            Baz baz2 = c.Resolve<Baz>();

            Baz baz3 = c.Resolve<Baz>();

            Assert.AreNotEqual(baz2, baz1);
            Assert.AreEqual(baz3, baz2);
        }

        [TestMethod]
        public void RegisterNonSingletonAfterSingleton()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Baz>(true);
            Baz baz1 = c.Resolve<Baz>();

            c.RegisterType<Baz>(false);
            Baz baz2 = c.Resolve<Baz>();

            Baz baz3 = c.Resolve<Baz>();

            Assert.AreNotEqual(baz2, baz1);
            Assert.AreNotEqual(baz3, baz2);
        }
    }
}
