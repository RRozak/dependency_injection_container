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
            T instance = GetInstance<T>(typeof(T), new Type[0]);

            foreach (var property in InjectablePropertiesFinder.GetInjectableProperties(instance))
            {
                this.InjectProperty(instance, property.GetSetMethod(), new Type[0]);
            }
            return instance;
        }

        public void BuildUp<T>(T instance) where T : class
        {
            foreach (var property in InjectablePropertiesFinder.GetInjectableProperties(instance))
            {
                if (property.GetValue(instance) == null)
                    this.InjectProperty(instance, property.GetSetMethod(), new Type[0]);
            }
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

        private IEnumerable<object> GetParameterInstances(MethodBase method, IEnumerable<Type> history)
        {
            var parameterTypes = method.GetParameters().Select(
                (parameter, _) => parameter.ParameterType);

            var parameterInstances = parameterTypes.Select(
                (paramType, _) => this.GetInstance<object>(paramType, history));

            return parameterInstances;
        }

        private T CreateInstance<T>(Type type, IEnumerable<Type> history) where T : class
        {
            if (history.Contains(type))
            {
                throw new ArgumentException("Cyclic dependency detected");
            }
            var constructor = ConstructorFinder.FindMostConcreteConstructor(type);

            var parameterInstances = this.GetParameterInstances(constructor, history.Append(type));

            return constructor.Invoke(parameterInstances.ToArray()) as T;
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

        private void InjectProperty<T>(T instance, MethodInfo setter, IEnumerable<Type> history)
        {
            var parameterInstances = this.GetParameterInstances(setter, history.Append(typeof(T)));

            setter.Invoke(instance, parameterInstances.ToArray());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
