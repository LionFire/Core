﻿using LionFire.Serialization;

namespace LionFire.Persistence.Filesystem
{
    /// <summary>
    /// For filesystem-like Persistence
    /// </summary>
    public class FilesystemPersistenceOptionsBase : PersistenceOptions
    {
        //public FilesystemPersistenceOptionsBase(ISerializationProvider serializationProvider) : base(serializationProvider) { }

        public string EndOfNameMarker { get; set; } = "'"; // REVIEW

        // REVIEW - is there a more pluggable way of getting these?  Make this type MultiTypable and have named Decorators for AutoRetry for Retrieve/Put/etc.?
        public int MaxGetRetries { get; set; } = 0;
        public int MaxDeleteRetries { get; set; } = 0;
        public int MillisecondsBetweenGetRetries { get; set; } = 500;
        public int MillisecondsBetweenDeleteRetries { get; set; } = 500;

        public AutoAppendExtension AutoAppendExtension { get; set; } = AutoAppendExtension.Disabled;
    }
}
