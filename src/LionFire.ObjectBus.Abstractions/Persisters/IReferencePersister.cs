﻿using LionFire.Referencing;
using System.Threading.Tasks;

namespace LionFire.Persistence
{
    public interface IReferencePersister<in TReference>
            where TReference : IReference
    {
        Task<IPersistenceResult> Create<TValue>(TReference reference, TValue value);

        Task<IExistsResult> Exists(TReference reference);

        Task<IRetrieveResult<TValue>> Retrieve<TValue>(TReference reference);

        Task<IPersistenceResult> Update<TValue>(TReference reference, TValue value);
        Task<IPersistenceResult> Upsert<TValue>(TReference reference, TValue value);

        Task<IPersistenceResult> Delete(TReference reference);
    }
}