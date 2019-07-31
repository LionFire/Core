﻿namespace LionFire.Referencing // RENAME LionFire.Referencing.Handles
{
    public interface IReadHandleProvider
    {
        RH<T> GetReadHandle<T>(IReference reference);
    }

    //[AutoRegister]
    public interface IReadHandleProvider<TReference>
        where TReference : IReference
    {
        RH<T> GetReadHandle<T>(TReference reference);
    }

}
