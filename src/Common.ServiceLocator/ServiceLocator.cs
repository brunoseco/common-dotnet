using Common.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.ServiceLocator
{
    public abstract class ServiceLocator : IServiceLocator
    {
        private IDictionary<Type, Type> servicesType;

        private IDictionary<object, object> services;

        private IDictionary<Type, object> instantiatedServices;

        private Object thisLockOne = new Object();
        private Object thisLockTwo = new Object();

        public ServiceLocator()
        {
            this.servicesType = new Dictionary<Type, Type>();
            this.instantiatedServices = new Dictionary<Type, object>();
            this.services = new ConcurrentDictionary<object, object>();

            this.BuildServiceTypesMap();
        }

        public void ServiceTypeAdd(Type key, Type value)
        {
            var exists = this.servicesType.Where(_ => _.Key == key).IsAny();
            if (!exists)
                this.servicesType.Add(key, value);
        }

        public T GetService<T>()
        {
            try
            {
                return (T)services[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException(string.Format("The requested service {0} is not registered", typeof(T)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("The requested service {0} has error - [{1}]", typeof(T), ex.Message));
            }
        }
        public T GetServiceLazyCache<T>(params object[] args)
        {
            if (this.instantiatedServices.ContainsKey(typeof(T)))
            {
                return (T)this.instantiatedServices[typeof(T)];
            }
            else
            {
                try
                {
                    var serviceMapItem = servicesType[typeof(T)];
                    T service = (T)Activator.CreateInstance(serviceMapItem, args);
                    var exists = instantiatedServices.Where(_ => _.Key == typeof(T)).IsAny();
                    if (!exists)
                        instantiatedServices.Add(typeof(T), service);

                    return service;

                }
                catch (KeyNotFoundException)
                {
                    throw new ApplicationException(string.Format("The requested service {0} is not registered", typeof(T)));
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(string.Format("The requested service {0} has error - [{1}]", typeof(T), ex.Message));
                }
            }
        }

        public T GetServiceLazy<T>(params object[] args)
        {
            if (this.servicesType.ContainsKey(typeof(T)))
            {
                var serviceMapItem = servicesType[typeof(T)];
                T service = (T)Activator.CreateInstance(serviceMapItem, args);
                return service;
            }
            else
            {
                throw new ApplicationException(string.Format("The requested service {0} is not registered", typeof(T)));
            }
        }

        public abstract void BuildServiceTypesMap();

    }
}
