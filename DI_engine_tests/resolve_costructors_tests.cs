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
        public void ClassWithConstructorParameterNotResolvable()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                ClassWithStringField a = c.Resolve<ClassWithStringField>();
            });
        }

        [TestMethod]
        public void ClassWithConstructorParameterCreatedBefore()
        {
            SimpleContainer c = new SimpleContainer();

            string s = "abc";
            c.RegisterInstance<string>(s);

            ClassWithStringField a = c.Resolve<ClassWithStringField>();

            Assert.AreEqual(a.s, s);
        }

        [TestMethod]
        public void ClassWithInterfaceAsFieldCreation()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<IBaz, Baz>(false);
            ClassWithInterfaceAsField a = c.Resolve<ClassWithInterfaceAsField>();

            Assert.IsNotNull(a.ibaz);
        }

        [TestMethod]
        public void ClassWithInterfaceAsFieldWithoutRegistringType()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                ClassWithInterfaceAsField a = c.Resolve<ClassWithInterfaceAsField>();
            });
        }

        [TestMethod]
        public void ClassWithConstructorParameterRegistredAsSingleton()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<Foo>(true);

            ClassWithSimpleField a1 = c.Resolve<ClassWithSimpleField>();
            ClassWithSimpleField a2 = c.Resolve<ClassWithSimpleField>();

            Assert.IsNotNull(a1.foo);
            Assert.AreEqual(a1.foo, a2.foo);
        }

        [TestMethod]
        public void ClassWithComplicatedField()
        {
            SimpleContainer c = new SimpleContainer();

            c.RegisterType<IBaz, Baz>(false);
            ClassWithComplicatedField a = c.Resolve<ClassWithComplicatedField>();

            Assert.IsNotNull(a.cl.ibaz);
        }

        [TestMethod]
        public void ClassWithComplicatedFieldWithoutTypeRegistration()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                ClassWithComplicatedField a = c.Resolve<ClassWithComplicatedField>();
            });
        }

        [TestMethod]
        public void ClassWithOneAttributedConstructor()
        {
            SimpleContainer c = new SimpleContainer();
            
            c.RegisterInstance<string>("abc");
            ClassWithOneAttributedConstructor a = c.Resolve<ClassWithOneAttributedConstructor>();

            Assert.AreEqual(a.s, "");
        }

        [TestMethod]
        public void ClassWithTwoAttributedConstructors()
        {
            SimpleContainer c = new SimpleContainer();
            
            c.RegisterInstance<string>("abc");

            Assert.ThrowsException<ArgumentException>(() =>
            {
                ClassWithTwoAttributedConstructors a = c.Resolve<ClassWithTwoAttributedConstructors>();
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void CyclicClassCreation()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                var a = c.Resolve<CyclicClass>();
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void IndirectCyclicClassCreation()
        {
            SimpleContainer c = new SimpleContainer();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                var a = c.Resolve<IndirectCyclicClassA>();
            });
        }
    }
}
