﻿using GillSoft.ConsoleApplication;
using GillSoft.ConsoleApplication.Implementations;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GillSoft.ConsoleApplication
{
    /// <summary>
    /// Concrete Application class.
    /// </summary>
    public sealed class Application : IApplication
    {
        private readonly IUnityContainer container;
        private readonly ILogger logger;
        private readonly IApplication app;

        private int? exitCode;

        void IApplication.Run(Action<ILogger, IApplication> callback)
        {
            var exitCodeToReturn = Common.DefaultExitCodeWithSuccess;
            try
            {
                var commandlineArguments = app.Resolve<ICommandlineArguments>();

                if (commandlineArguments.Help)
                {
                    commandlineArguments.ShowHelp();
                }
                else
                {
                    callback(logger, this);
                }
            }
            catch (Exception ex)
            {
                exitCodeToReturn = Common.DefaultExitCodeWithError;
                logger.Error("Exception in Run", ex);
            }
            finally
            {
                exitCodeToReturn = exitCode.HasValue ? exitCode.Value : exitCodeToReturn;
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    Console.Write("Press RETURN to close (This messages apppears only while debugging)...");
                    Console.ReadLine();
                }
            }
            Environment.Exit(exitCodeToReturn);
        }

        /// <summary>
        /// Creates an instance of Application
        /// </summary>
        /// <returns></returns>
        public static IApplication Create()
        {
            var res = new Application();
            return res;
        }

        private Application()
        {
            this.container = new UnityContainer();

            this.app = this as IApplication;

            RegisterTypes();

            this.logger = app.Resolve<ILogger>();
        }

        private void RegisterTypes()
        {
            app.RegisterInstance<IApplication>(this);

            if (!app.IsRegistered<IOutput>())
            {
                app.RegisterType<IOutput, ConsoleOutput>();
            }

            if (!app.IsRegistered<ILogger>())
            {
                app.RegisterType<ILogger, ConsoleLogger>();
            }

            if (!app.IsRegistered<IApplicationConfiguration>())
            {
                app.RegisterType<IApplicationConfiguration, ApplicationConfigurationBase>();
            }

            if (!app.IsRegistered<IApplicationConfiguration>())
            {
                app.RegisterType<IApplicationConfiguration, ApplicationConfigurationBase>();
            }

            if (!app.IsRegistered<ICommandlineArguments>())
            {
                app.RegisterType<ICommandlineArguments, CommandlineArguments>();
            }

        }

        bool IApplication.IsRegistered<T>()
        {
            var res = container.IsRegistered<T>();
            return res;
        }

        void IApplication.RegisterInstance<TInterface>(TInterface instance)
        {
            this.container.RegisterInstance<TInterface>(instance);
        }

        T IApplication.Resolve<T>()
        {
            var res = this.container.Resolve<T>();
            return res;
        }

        void IApplication.RegisterType<TFrom, TTo>()
        {
            this.container.RegisterType<TFrom, TTo>();
        }

        void IApplication.SetExitCode(int exitCode)
        {
            this.exitCode = exitCode;
        }
    }
}
