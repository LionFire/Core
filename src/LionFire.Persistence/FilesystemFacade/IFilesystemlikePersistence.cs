﻿//#define TRACE_SAVE
#define TRACE_LOAD

using System;
using System.Threading.Tasks;
using LionFire.IO;
using LionFire.Referencing;
using LionFire.Serialization;

namespace LionFire.Persistence.Filesystemlike
{

    /// <summary>
    /// TODO - Work in progress
    /// </summary>
    public interface IFilesystemlikePersistence<TReference, TPersistenceOptions>
        where TReference : IReference
        where TPersistenceOptions : FilesystemlikePersisterOptionsBase, IPersistenceOptions
    {
        Task<bool?> CanDelete(string fsPath);

        /// <returns>True if it deleted something, False if it didn't, and null if it couldn't tell</returns>
        Task<bool?> Delete(string fsPath);
        Task<bool> Exists(string fsPath);
        Task<bool> Exists<T>(string fsPath);
        //Task<bool> TryDelete<T>(string fsPath, bool preview = false);
        //Task<IRetrieveResult<T>> TryRetrieve<T>(TReference fileReference, Lazy<PersistenceOperation> operation = null, PersistenceContext context = null);
        Task<IPersistenceResult> Update<T>(string fsPath, T obj, Type type = null, PersistenceContext context = null);
        Task<IPersistenceResult> Upsert<T>(string fsPath, T obj, Type type = null, PersistenceContext context = null);
        Task<IPersistenceResult> Write<T>(string fsPath, T obj, ReplaceMode replaceMode = ReplaceMode.Upsert, PersistenceContext context = null, Type type = null);
    }
}
