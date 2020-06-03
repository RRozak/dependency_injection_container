using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DI_engine
{
    public class SimpleContainer
    {
        private Dictionary<Type, Type> _registeredType = new Dictionary<Type, Type>();
        private Dictionary<Type, bool> _ifSingleton = new Dictionary<Type, bool>();
        private Dictionary<Type, object> _singletonInstances = new Dictionary<Type, object>();

        public void RegisterType<T>(bool Singleton) where T : class
        {
            this.CleanType(typeof(T));

            this._ifSingleton[typeof(T)] = Singleton;
        }

        public void RegisterType<From, To>(bool Singleton) where To : From
        {
            this.CleanType(typeof(From));

            this._registeredType[typeof(From)] = typeof(To);
            this._ifSingleton[typeof(From)] = Singleton;
        }

        public void RegisterInstance<T>(T instance)
        {
            this.CleanType(typeof(T));

            this._ifSingleton[typeof(T)] = true;
            this._singletonInstances[typeof(T)] = instance;
        }

        public T Resolve<T>() where T : class
        {
            return GetInstance<T>(typeof(T), new Type[0]);
        }

        private T GetInstance<T>(Type type, IEnumerable<Type> history) where T : class
        {
            if (this._ifSingleton.ContainsKey(type)
                && this._ifSingleton[type])
            {
                if (!this._singletonInstances.ContainsKey(type))
                {
                    this._singletonInstances[type] =
                        this.CreateInstance<T>(this.GetResolvedType(type), history);
                }
                return this._singletonInstances[type] as T;
            }
            else
            {
                return this.CreateInstance<T>(this.GetResolvedType(type), history);
            }
        }

        private void CleanType(Type type)
        {
            this._registeredType.Remove(type);
            this._ifSingleton.Remove(type);
            this._singletonInstances.Remove(type);
        }

        private T CreateInstance<T>(Type type, IEnumerable<Type> history) where T : class
        {
            if(history.Contains(type))
            {
                throw new ArgumentException("Cyclic dependency detected");
            }
            var constructor = this.FindMostConcreteConstructor(type);

            var parameterTypes = constructor.GetParameters().Select(
                (parameter, _) => parameter.ParameterType);

            var parameterInstances = parameterTypes.Select(
                (paramType, _)  => this.GetInstance<object>(paramType, history.Append(type)));

            return constructor.Invoke(parameterInstances.ToArray()) as T;
        }

        private ConstructorInfo FindMostConcreteConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();

            if(constructors.Length == 0)
            {
                throw new ArgumentException("There are no public constructor for the object");
            }

            ConstructorInfo dependencyConstructor = this.FindDependencyConstructor(constructors);

            if(dependencyConstructor != null)
            {
                return dependencyConstructor;
            }
            return this.FindLongestConstructor(constructors);
        }

        private ConstructorInfo FindDependencyConstructor(ConstructorInfo[] constructors)
        {
            var dependencyConstructors = Array.FindAll(constructors,
                c => c.CustomAttributes.Select((attr, _) => attr.AttributeType).Contains(typeof(DependencyConstructor)));
            
            if(dependencyConstructors.Length > 1)
            {
                throw new ArgumentException("There are more than 1 public constructors"
                                            + "with DependencyConstrutor attribute");
            }
            else if(dependencyConstructors.Length == 1)
            {
                return dependencyConstructors[0];
            }
            return null;
        }

        private ConstructorInfo FindLongestConstructor(ConstructorInfo[] constructors)
        {
            var constructorsLength = constructors.Select((constructor, _ ) => constructor.GetParameters().Length);
            int constructorsMaxLength = constructorsLength.Aggregate(Math.Max);
            
            var longestConstructors = Array.FindAll(constructors,
                constructor => constructor.GetParameters().Length == constructorsMaxLength);

            if(longestConstructors.Length > 1)
            {
                throw new ArgumentException("There are more than 1 constructors with the biggest number of parameters");
            }
            return longestConstructors[0];
        }

        private Type GetResolvedType(Type type)
        {
            if (!this._registeredType.ContainsKey(type))
            {
                return type;
            }
            else
            {
                return this.GetResolvedType(this._registeredType[type]);
            }
        }
    }

    public class DependencyConstructor : Attribute
    { }

    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
