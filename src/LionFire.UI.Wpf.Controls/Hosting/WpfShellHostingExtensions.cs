﻿using LionFire.Alerting;
using LionFire.Applications;
using LionFire.Services.DependencyMachines;
using LionFire.Shell;
using LionFire.Shell.Wpf;
using LionFire.Threading;
using LionFire.UI.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using LionFire.Settings;
using LionFire.UI.Windowing;
using LionFire.Vos;
using LionFire.Referencing;
using LionFire.Assets;
using LionFire.Persistence;

namespace LionFire.Hosting
{
    public static class WpfShellHostingExtensions
    {
        /// <summary>
        /// Wire up minimal relationships for WPF to run:
        ///  - Application
        ///  - Dispatcher
        /// </summary>
        /// <param name="services"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public static IServiceCollection AddWpfRuntime(this IServiceCollection services, Application application)
          => services
              .SetWpfApplication(application)
              .AddDispatcher(application)
              ;

        public static IServiceCollection SetWpfApplication(this IServiceCollection services, Application application)
            => services
                .AddSingleton(application ?? throw new ArgumentNullException(nameof(application)))
                ;

        public static IServiceCollection AddDispatcher(this IServiceCollection services, Application application)
            => services
                .AddSingleton<WpfDispatcherAdapter>(serviceProvider => new WpfDispatcherAdapter(application))
                .AddSingleton<IDispatcher, WpfDispatcherAdapter>(serviceProvider => serviceProvider.GetRequiredService<WpfDispatcherAdapter>());


        public static IServiceCollection AddWpfShell(this IServiceCollection services)
        {
            WpfCommandManager.Initialize();

            return services
                    .Configure<SettingsOptions>(o =>
                    {
                        //o.Handles.Add(new VobReference<WindowSettings>("$AppSettings/WindowSettings").GetReadWriteHandle<WindowSettings>());
                        //o.Handles.Add(new AssetReference<WindowSettings>.Channel("$AppSettings").GetReadWriteHandle<WindowSettings>());
                        o.Handles.Add(AppSettings2<WindowSettings>.H);
                    })
                   //// REVIEW - a way to merge these 3 lines?
                   // .AddSingleton<WpfShell>()
                   // .AddSingleton<IWpfShell>(p => p.GetRequiredService<WpfShell>()) 
                   // .AddHostedServiceDependency<WpfShell>()
                   .AddSingletonHostedServiceDependency<WpfShell>()

                  //.AddSingleton<IWindowManager, LionFireWindowManager>()

                  .AddTransient<IShellPresenter, WpfShellPresenter>()
                  //.AddSingleton<IShellPresenter>(s=>s.GetRequiredService<WpfShellPresenter>())
                  //.AddSingleton<WpfShellPresenter>()

                  .AddSingleton<IAlerter, WpfShellAlerter>()
                  //.Configure<AppOptions>(o => o.SetHostedServicesIfEmpty(typeof(WpfShell)))
                  ;
        }
    }
    public static class AppSettings2<T>
    {
        public static IReadWriteHandle<T> H
            => AssetReference<T>.Channel("$AppSettings").GetReadWriteHandle<T>();
        public static IReadHandle<T> R
             => AssetReference<T>.Channel("$AppSettings").GetReadHandle<T>();

    }
}