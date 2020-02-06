﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LionFire.Applications.Hosting;
using LionFire.Hosting;
using LionFire.Services;
using LionFire.ObjectBus;
using LionFire.ObjectBus.ExtensionlessFs;
using LionFire.ObjectBus.Filesystem;
using LionFire.ObjectBus.Filesystem.Tests;
using LionFire.ObjectBus.Testing;
using LionFire.Referencing;
using Xunit;
using LionFire.Serialization;
using LionFire.Persistence.Filesystem.Tests;
using LionFire;

namespace Handle
{
    public class _Get_Object_FromDisk
    {
        [Fact]
        public async void Pass_WithoutExtension() => await _Pass(withExtension: false);
        [Fact]
        public async void Pass_WithExtension() => await _Pass(withExtension: true);

        private async Task _Pass(bool withExtension)
        {
            var host = FilesystemTestHost.Create(
                //serializers: s => s.AddJson()
                );

            if(!withExtension)
            {
                host.ConfigureServices(services=> services.AddExtensionlessFSOBus();
            }

            await host
                    .AddObjectBus<FSOBus>()                    
                    .Run(() =>
                    {
                        var pathWithoutExtension = FsTestUtils.TestFile;
                        var path = pathWithoutExtension + ".json";

                        File.WriteAllText(path, PersistenceTestUtils.TestClass1Json);

                        IReference reference;
                        if (withExtension)
                        {
                            reference = new FileReference(path);
                        }
                        else
                        {                            
                            reference = new ExtensionlessFileReference(pathWithoutExtension); 
                        }

                        var h = reference.ToHandle<TestClass1>();

                        var obj = h.Value; // --------------- Object

                        Assert.NotNull(obj);
                        Assert.IsType<TestClass1>(obj);
                        FsTestUtils.AssertEqual(TestClass1.Create, obj);

                        FsTestUtils.CleanPath(path);
                    });
        }
    }
}