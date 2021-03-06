﻿#nullable enable
using LionFire.Ontology;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LionFire.DependencyMachines
{
    public static class IParticipantExtensions
    {

        //public static bool IsNoop(this IParticipant participant) // FUTURE?
        //{
        //    return participant.StartTask == null && participant.StopTask == null;
        //}

        public static IEnumerable<IParticipant> GetParticipants(this object obj)
        {
            if (obj is IHas<IParticipant> hasParticipant)
            {
                yield return hasParticipant.Object;
            }
            if (obj is IHasMany<IParticipant> hasParticipants)
            {
                foreach (var p in hasParticipants.Objects) { yield return p; }
            }
        }

        public static IParticipant Key(this IParticipant participant, string key)
        {
            participant.Key = key;
            return participant;
        }

        #region Contributes

        public static IParticipant Contributes(this IParticipant participant, params string[] stage)
        {
            participant.Contributes ??= new List<object>();
            participant.Contributes.AddRange(stage);
            return participant;
        }
        public static IParticipant Contributes(this IParticipant participant, params Enum[] stages)
               => participant.Contributes(stages.Select(s => s.ToString()).ToArray());

        #endregion

        #region Provides

        public static IParticipant Provide(this IParticipant participant, params string[] keys)
        {
            participant.Provides ??= new List<object>();
            participant.Provides.AddRange(keys);
            return participant;
        }
        public static IParticipant Provide(this IParticipant participant, params Enum[] keys)
               => participant.Provide(keys.Select(s => s.ToString()).ToArray());

        public static IEnumerable<object> EffectiveProvides(this IParticipant participant)
        {
            IEnumerable<object> result = new object[] { participant.Key };
            if (participant.Provides != null)
            {
                result = result.Concat(participant.Provides).Distinct();
            }
            return result;
        }
        #endregion

        #region DependsOn

        public static IParticipant RootDependency(this IParticipant participant)
            => participant.DependsOn(new string?[] { null });

        public static IParticipant DependsOn(this IParticipant participant, params string?[] stages)
        {
            participant.Dependencies ??= new List<object>();
            participant.Dependencies.AddRange(stages.Cast<object>());
            return participant;
        }
        public static IParticipant DependsOn<T>(this IParticipant participant)
        {
            participant.Dependencies ??= new List<object>();
            participant.Dependencies.Add("type:" + typeof(T).FullName);
            return participant;
        }

        public static IParticipant DependsOn(this IParticipant participant, params Enum[] stages)
            => participant.DependsOn(stages.Select(s => s.ToString()).ToArray());

        #endregion

        #region HostedService

        public static IParticipant DependsOnHostedService<T>(this IParticipant participant)
            where T : IHostedService
            => participant.DependsOn(HostedServiceParticipant<T>.KeyForHostedServiceType);
        public static IParticipant DependsOnHostedService<T>(this IParticipant participant, string machineName)
                   where T : IHostedService
                   => participant.DependsOn(HostedServiceParticipant<T>.KeyForHostedServiceType);

        #endregion

        #region Before

        public static IParticipant Before(this IParticipant participant, params string?[] stages)
        {
            participant.PrerequisiteFor ??= new List<object>();
            participant.PrerequisiteFor.AddRange(stages.Cast<object>());
            return participant;
        }

        public static IParticipant Before(this IParticipant participant, params Enum[] stages)
            => participant.Before(stages.Select(s => s.ToString()).ToArray());

        #endregion

        #region After

        public static IParticipant After(this IParticipant participant, params string?[] stages)
        {
            participant.After ??= new List<object>();
            participant.After.AddRange(stages.Cast<object>());
            return participant;
        }

        public static IParticipant After(this IParticipant participant, params Enum[] stages)
            => participant.After(stages.Select(s => s.ToString()).ToArray());

        public static IParticipant After<T>(this IParticipant participant) // UNTESTED
            => participant.After(typeof(T).FullName);

        #endregion

        #region RequirementFor

        //public static IParticipant RequirementFor(this IParticipant participant, params string[] stage)
        //{

        //    participant.PrerequisiteFor ??= new List<object>();
        //    participant.PrerequisiteFor.AddRange(stage);
        //    return participant;
        //}
        //public static IParticipant RequirementFor(this IParticipant participant, params Enum[] stages)
        //            => participant.RequirementFor(stages.Select(s => s.ToString()).ToArray());

        //public static IDependencyStateMachine StageIsRequirementForStage(this IDependencyStateMachine dependencyStateMachine, string prerequisiteStage, string dependantStage)
        //      => dependencyStateMachine.Register(new Placeholder(prerequisiteStage)
        //          .PrerequisiteFor(dependantStage));

        #endregion
    }

}
