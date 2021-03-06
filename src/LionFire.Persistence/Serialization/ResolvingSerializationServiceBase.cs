﻿using System;
using System.Collections.Generic;
using LionFire.IO;
using LionFire.Persistence;

namespace LionFire.Serialization
{
    public abstract class ResolvingSerializationServiceBase : IResolvesSerializationStrategies
    {
        public abstract IEnumerable<SerializationStrategyPreference> SerializationStrategyPreferences { get; }

        public IEnumerable<ISerializationStrategy> Strategies => strategies;
        protected IEnumerable<ISerializationStrategy> strategies;

        public IEnumerable<SerializationSelectionResult> ResolveStrategies(Lazy<PersistenceOperation> operation = null, PersistenceContext context = null, IODirection? direction = null) 
            => SerializeHelpers.ResolveStrategies(SerializationStrategyPreferences, operation, context, direction);
    }
}
