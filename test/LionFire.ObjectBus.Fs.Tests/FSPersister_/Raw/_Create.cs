﻿using LionFire.ObjectBus.Filesystem.Tests;
using LionFire.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using LionFire.ObjectBus.Filesystem;
using System.IO;
using LionFire.ObjectBus;
using LionFire;
using LionFire.Serialization;
using LionFire.Hosting;
using Microsoft.Extensions.Hosting;
using LionFire.Applications.Hosting;
using LionFire.Persistence;
using LionFire.ObjectBus.Testing;
using LionFire.Serialization.Json.Newtonsoft;

namespace FSPersister_
{
    namespace Raw
    {
#if TODO
        public class _Create
        {
            [Fact]
            public async void P_string()
            {
                await PersistersHost.Create().Run(async () =>
                {
                    var path = FsTestUtils.TestFile;
                    var testContents = "testing123";

                    Assert.False(File.Exists(path));

                    await Singleton<FSPersister>.Instance.Create(path.ToFileReference(), testContents);

                    Assert.True(File.Exists(path));

                    var fromFile = File.ReadAllText(path);
                    Assert.Equal(testContents, fromFile);

                    File.Delete(path);

                    Assert.False(File.Exists(path));
                });
            }

            [Fact]
            public async void P_bytes()
            {
                await PersistersHost.Create().Run(async () =>
                {

                    var path = FsTestUtils.TestFile;
                    var testContents = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 32, 33, 34, 35, 64, 65, 66, 67, 68 };

                    Assert.False(File.Exists(path));

                    await Singleton<FSPersister>.Instance.Create(path.ToFileReference(), testContents);

                    Assert.True(File.Exists(path));

                    var fromFile = File.ReadAllBytes(path);
                    Assert.Equal(testContents, fromFile);

                    File.Delete(path);

                    Assert.False(File.Exists(path));
                });
            }

            [Fact]
            public async void P_stream()
            {
                await PersistersHost.Create().Run(async () =>
                {
                    var path = FsTestUtils.TestFile;
                    var testContents = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 32, 33, 34, 35, 64, 65, 66, 67, 68 };

                    var ms = new MemoryStream();
                    ms.Write(new ReadOnlySpan<byte>(testContents));
                    ms.Seek(0, SeekOrigin.Begin);

                    Assert.False(File.Exists(path));

                    await Singleton<FSPersister>.Instance.Create(path.ToFileReference(), ms.StreamToBytes());

                    Assert.True(File.Exists(path));

                    var fromFile = File.ReadAllBytes(path);
                    Assert.Equal(testContents, fromFile);

                    File.Delete(path);

                    Assert.False(File.Exists(path));
                });
            }
        }
#endif
    }

}