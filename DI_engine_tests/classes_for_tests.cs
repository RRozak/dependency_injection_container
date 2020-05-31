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
    }

}
