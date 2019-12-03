﻿using LionFire.Persistence;
using LionFire.Persistence.Filesystem;
using LionFire.Persistence.Filesystem.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using LionFire.Serialization;
using LionFire.Hosting;
using LionFire.Referencing;

namespace FileReadHandle_
{
    public class _Retrieve
    {
        public async void P_string()
        {
            await PersistersHost.Create()
            .Run(async () =>
            {
                var path = FsTestUtils.TestFile + ".txt";
                Assert.False(File.Exists(path));

                FileReference reference = path;
                Assert.Equal(reference.Path, path);

                var testContents = "testing123";
                File.WriteAllText(path, testContents);
                Assert.True(File.Exists(path));

                var readHandle = reference.GetReadHandle<string>();
                var persistenceResult = await readHandle.Retrieve();

                Assert.True(persistenceResult.Flags.HasFlag(PersistenceResultFlags.Success));
                Assert.Equal(testContents, readHandle.Value);

                File.Delete(path);
                Assert.False(File.Exists(path));
            });
        }
    }
}