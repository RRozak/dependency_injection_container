using System;
using DI_engine;

namespace DI_engine_tests
{
    class Foo
    { }

    class Bar : Foo
    { }

    class FooBar : Bar
    { }

    class Bar2 : Foo
    { }

    interface IBaz
    { }

    class Baz : IBaz
    {}

    abstract class AFoo
    { }

    class ClassWithSimpleField
    {
        public Foo foo;
        public ClassWithSimpleField(Foo foo)
        {
            this.foo = foo;
        }
    }

    class ClassWithPrivateConstructor
    {
        private ClassWithPrivateConstructor()
        { }
    }

    class ClassWithTwoConstructors
    {
        public Foo foo;

        public ClassWithTwoConstructors()
        { }

        public ClassWithTwoConstructors(Foo foo)
        {
            this.foo = foo;
        }
    }

    class ClassWithTwoLongestConstructors
    {
        public ClassWithTwoLongestConstructors(Foo foo)
        { }

        public ClassWithTwoLongestConstructors(Bar bar)
        { }
    }

    class ClassWithInterfaceAsField
    {
        public IBaz ibaz;
        public ClassWithInterfaceAsField(IBaz ibaz)
        {
            this.ibaz = ibaz;
        }
    }

    class ClassWithComplicatedField
    {
        public ClassWithInterfaceAsField cl;
        public ClassWithComplicatedField(ClassWithInterfaceAsField cl)
        {
            this.cl = cl;
        }
    }

    class ClassWithOneAttributedConstructor
    {
        public string s;

        [DependencyConstructor]
        public ClassWithOneAttributedConstructor()
        {
            this.s = "";
        }

        public ClassWithOneAttributedConstructor(string s)
        {
            this.s = s;
        }
    }

    class ClassWithTwoAttributedConstructors
    {
        public string s;

        [DependencyConstructor]
        public ClassWithTwoAttributedConstructors()
        {
            this.s = "";
        }

        [DependencyConstructor]
        public ClassWithTwoAttributedConstructors(string s)
        {
            this.s = s;
        }
    }

    class ClassWithStringField
    {
        public string s;
        public ClassWithStringField(string s)
        {
            this.s = s;
        }
    }

    class CyclicClass
    {
        public CyclicClass cyclicClass;
        public CyclicClass(CyclicClass cyclicClass)
        {
            this.cyclicClass = cyclicClass;
        }
    }

    class IndirectCyclicClassA
    {
        public IndirectCyclicClassB cyclicClass;
        public IndirectCyclicClassA(IndirectCyclicClassB cyclicClass)
        {
            this.cyclicClass = cyclicClass;
        }
    }

    class IndirectCyclicClassB
    {
        public IndirectCyclicClassA cyclicClass;
        public IndirectCyclicClassB(IndirectCyclicClassA cyclicClass)
        {
            this.cyclicClass = cyclicClass;
        }
    }
}
