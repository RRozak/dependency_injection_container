using System;
using System.Linq;
using System.Reflection;

namespace DI_engine
{
    public static class ConstructorFinder
    {
        public static ConstructorInfo FindMostConcreteConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();

            if (constructors.Length == 0)
            {
                throw new ArgumentException("There are no public constructor for the object");
            }

            ConstructorInfo dependencyConstructor = FindDependencyConstructor(constructors);

            if (dependencyConstructor != null)
            {
                return dependencyConstructor;
            }
            return FindLongestConstructor(constructors);
        }

        public static ConstructorInfo FindDependencyConstructor(ConstructorInfo[] constructors)
        {
            var dependencyConstructors = Array.FindAll(constructors,
                c => c.CustomAttributes.Select((attr, _) => attr.AttributeType).Contains(typeof(DependencyConstructor)));

            if (dependencyConstructors.Length > 1)
            {
                throw new ArgumentException("There are more than 1 public constructors"
                                            + "with DependencyConstrutor attribute");
            }
            else if (dependencyConstructors.Length == 1)
            {
                return dependencyConstructors[0];
            }
            return null;
        }

        public static ConstructorInfo FindLongestConstructor(ConstructorInfo[] constructors)
        {
            var constructorsLength = constructors.Select((constructor, _) => constructor.GetParameters().Length);
            int constructorsMaxLength = constructorsLength.Aggregate(Math.Max);

            var longestConstructors = Array.FindAll(constructors,
                constructor => constructor.GetParameters().Length == constructorsMaxLength);

            if (longestConstructors.Length > 1)
            {
                throw new ArgumentException("There are more than 1 constructors with the biggest number of parameters");
            }
            return longestConstructors[0];
        }
    }
}
