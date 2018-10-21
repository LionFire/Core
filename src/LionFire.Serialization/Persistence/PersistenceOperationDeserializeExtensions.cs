﻿//#define TRACE_SAVE
#define TRACE_LOAD

//using LionFire.Extensions.Collections;
using System.Collections.Generic;
using System.IO;
using LionFire.Serialization;

namespace LionFire.Persistence
{
    public static class PersistenceOperationDeserializeExtensions
    {
        public static T ToObject<T>(this PersistenceOperation op, PersistenceContext context = null)
        {
            if (context == null) context = op.Context;
            var resolver = (context?.SerializationProvider ?? Defaults.TryGet<ISerializationProvider>());

            IEnumerable<Stream> streams(ISerializationStrategy strategy)
            {
                foreach (var path in op.Deserialization.CandidatePaths)
                {
                    if (!op.IgnoreFileExtension && !strategy.SupportsExtension(Path.GetExtension(path)))
                    {
                        continue;
                    }

                    yield return context.Deserialization.PathToStream(path);
                }
            }

            return resolver.ToObject<T>(streams, op, context);
            
            
            //var strategyResults = .Strategies(streams(), op, context);
            
            //foreach (var strategyResult in strategyResults)
            //{
            //    var strategy = strategyResult.Strategy;
            //    foreach (var path in op.Deserialization.CandidatePaths)
            //    {
            //        if (!op.IgnoreFileExtension && !strategy.SupportsExtension(Path.GetExtension(path)))
            //        {
            //            continue;
            //        }

            //        //#if MONO
            //        //            fullDiskPath = fullDiskPath.Replace('\\', '/');
            //        //#else
            //        //                fullDiskPath = fullDiskPath.Replace('/', '\\'); // REVIEW
            //        //#endif

            //        using (var fs = context.Deserialization.PathToStream(path))
            //        {
            //            var (Object, Result) = strategy.ToObject<T>(fs, op, context);
            //            if (Result.IsSuccess)
            //            {
            //                return (Object,Result);
            //            }
            //        }
            //    }
            //}
            //return (default(T), );

            //            using (var fs = new FileStream(diskPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //            {
            //                if (firstRead)
            //                {
            //                    firstRead = false;
            //                }
            //                else
            //                {
            //                    fs.Seek(0, SeekOrigin.Begin);
            //                }

            //                var op = ((Func<PersistenceOperation>)(() => new SerializePersistenceOperation()
            //                {
            //                    Path = diskPath,
            //                    PathIsMissingExtension = null,
            //                })).ToLazy();


            //#if TRACE_LOAD
            //                l.Debug("[FS LOAD] " + diskPath);
            //#endif
            //                if (fs.Length == 0)
            //                {
            //                    l.Error("FsPersistence.GetFromPath found empty file. " + (AutoDeleteEmptyFiles ? "Autodeleting it." : "") + " - " + diskPath);
            //                    deleteFile = AutoDeleteEmptyFiles;
            //                    return null;
            //                }

            //                object obj = Deserialize(fs, type, diskPath, operation, context);
            //                return obj;
            //            }
        }
    }
}
