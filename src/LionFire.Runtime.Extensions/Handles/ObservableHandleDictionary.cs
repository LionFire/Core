﻿using LionFire.Persistence;
using System;
using System.Reflection;

namespace LionFire.Serialization
{
    public abstract class ObservableHandleDictionary<TKey, THandle, T> : ObservableReadHandleDictionary<TKey, THandle, T>
        where THandle : class, IReadHandle<T>, IWriteHandle<T>
    {

        #region Key translation

        // REVIEW - come up with a clearer name for these methods
        public virtual string KeyToHandleKey(string key)
        {
            return key;
        }
        public virtual string HandleKeyToKey(string key) { return key; }

        #endregion

        /// <summary>
        /// Immediately writes directly to underlying data store.  Does not write to internal collection. 
        /// (FUTURE: write to internal collection, so that a local cache is up to date while a long write is in progress.)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public THandle Add(T obj, string key = null)
        {
            //if (!IsWritable) throw new ReadOnlyException("THandle does not implement IWriteHandle<T>");

            THandle handle;
            if (key == null)
            {
                handle = (THandle)Activator.CreateInstance(typeof(THandle));
            }
            else
            {
                handle = (THandle)Activator.CreateInstance(typeof(THandle), KeyToHandleKey(key));
            }

            var writable = (IWriteHandle<T>)handle;
            writable.Object = obj;

            if (handle is ISaveable saveable)
            {
                saveable.Save(PersistenceContext);
            }
            return handle;
        }
        public object PersistenceContext { get; set; }
    }

}
