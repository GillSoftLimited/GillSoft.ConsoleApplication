﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GillSoft.ConsoleApplication
{
    /// <summary>
    /// Interface to be implemented by DI/IOC container class.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Registers an interface and the concrete class with DI container.
        /// </summary>
        /// <typeparam name="TFrom">Interface</typeparam>
        /// <typeparam name="TTo">Concrete class</typeparam>
        void RegisterType<TFrom, TTo>() where TTo : TFrom;

        /// <summary>
        /// Registers an interface and an instance of a concrete class  with DI container.
        /// </summary>
        /// <typeparam name="TInterface">Interface</typeparam>
        /// <param name="instance">Instance of the class that implements TInterface</param>
        void RegisterInstance<TInterface>(TInterface instance);

        /// <summary>
        /// Checks if a type is registered with DI container.
        /// </summary>
        /// <typeparam name="T">Interface</typeparam>
        /// <returns></returns>
        bool IsRegistered<T>();

        /// <summary>
        /// Returns a resolved instance of a type with all dependencies injected,
        /// </summary>
        /// <typeparam name="T">Interface</typeparam>
        /// <returns></returns>
        T Resolve<T>();
    }
}
