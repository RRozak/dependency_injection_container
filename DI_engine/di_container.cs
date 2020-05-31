using System;
using System.Collections.Generic;

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
            if (this._ifSingleton.ContainsKey(typeof(T))
                && this._ifSingleton[typeof(T)])
            {
                if (!this._singletonInstances.ContainsKey(typeof(T)))
                {
                    this._singletonInstances[typeof(T)] =
                        this.CreateInstance<T>(this.GetResolvedType(typeof(T)));
                }
                return this._singletonInstances[typeof(T)] as T;
            }
            else
            {
                return this.CreateInstance<T>(this.GetResolvedType(typeof(T)));
            }
        }

        private void CleanType(Type type)
        {
            this._registeredType.Remove(type);
            this._ifSingleton.Remove(type);
            this._singletonInstances.Remove(type);
        }

        private T CreateInstance<T>(Type type) where T : class
        {
            try
            {
                return Activator.CreateInstance(type) as T;
            }
            catch (MissingMethodException)
            {
                throw new ArgumentException("Cannot create an instance of interface or abstract class");
            }
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

    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
