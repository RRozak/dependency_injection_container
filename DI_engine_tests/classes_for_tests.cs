using System;

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
        public ClassWithTwoLongestConstructors(int x)
        { }

        public ClassWithTwoLongestConstructors(char y)
        { }
    }
}
