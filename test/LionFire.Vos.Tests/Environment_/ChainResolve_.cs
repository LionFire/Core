﻿using LionFire.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using LionFire.Services;
using LionFire.Vos;
using LionFire.Vos.Environment;
using LionFire.Resolves.ChainResolving;
using LionFire.FlexObjects;
using Microsoft.Extensions.DependencyInjection;

namespace Environment_
{
    public class ChainResolve_
    {
        [Fact]
        public void P()
        {
            VosHost.Create()
                .ConfigureServices((_, s) =>
                {
                    s
                    .AddChainResolver<string, VosReference>(s => new VosReference(s)) // ENH: look for ctor or static implicit operator on target type with source type
                    .VobEnvironment("key2", "value2")
                    ;
                })
                .RunAsync(serviceProvider =>
                {
                    var root = serviceProvider.GetRootVob();
                    var env = root.Environment();

                    var r = env.FlexDictionary.Values["key2"].ResolveTo<VosReference>();
                    Assert.IsType<VosReference>(r);
                    Assert.IsType<VosReference>(env.FlexDictionary.Values["key2"].SingleValueOrDefault());
                });
        }

#if TODO
        [Fact]
        public void P_FallThroughToDefault()
        {
            VosHost.Create()
                .ConfigureServices((_, s) =>
                {
                    s
                    .AddChainResolver<string, VosReference>(s => new VosReference(s)) // ENH: look for ctor or static implicit operator on target type with source type
                    .VobEnvironment("key2", "value2")
                    ;
                })
                .Run(serviceProvider =>
                {
                    var root = serviceProvider.GetRootVob();
                    var env = root.Environment();

                    var r = env.FlexDictionary.Values["key2"].ResolveTo<VosReference>();
                    Assert.IsType<VosReference>(r);
                    Assert.IsType<VosReference>(env.FlexDictionary.Values["key2"].SingleValueOrDefault());

                    // TODO use this from Default:                 new ChainResolverWorker(typeof(ILazilyResolves<object>), o => ((ILazilyResolves<object>)o).GetValue()),
                    Assert.True(false);
                });
        }
#endif
    }
}
