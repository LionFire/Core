﻿using LionFire.Referencing;

namespace LionFire.Persistence.Handles
{
    public abstract class PersisterReadWriteHandleBase<TReference, TValue> : ReadWriteHandleBase<TReference, TValue>, IPersisterHandle<TReference>
        where TReference : IReference
    {
        public IPersister<TReference> Persister { get; protected set; }

        public PersisterReadWriteHandleBase(IPersister<TReference> persister) => Persister = persister;


        //public override event Action<PersistenceEvent<TValue>> PersistenceStateChanged;

        //public override ILazyResolveResult<TValue> QueryValue() => throw new NotImplementedException();
        //public override void RaisePersistenceEvent(PersistenceEvent<TValue> ev) => throw new NotImplementedException();
        //protected override Task<IResolveResult<TValue>> ResolveImpl() => throw new NotImplementedException();

        //protected override async Task<IPersistenceResult> UpsertImpl() =>
        //    await Persister.Upsert<IReference, TValue>(this, ProtectedValue);
    }

    

}