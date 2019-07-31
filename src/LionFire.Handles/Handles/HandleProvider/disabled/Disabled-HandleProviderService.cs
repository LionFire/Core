﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using LionFire.DependencyInjection;

//namespace LionFire.Referencing
//{
//    public class HandleProviderService : IHandleProviderService
//    {
//        public IEnumerable<Type> HandleTypes => DependencyContext.Current.GetService<IEnumerable<IHandleProvider>>().SelectMany(hp => hp.HandleTypes);

//        public H<T> ToHandleOrThrow<T>(IReference reference) where T : class
//        {
//            var result = ToHandleOrThrow<T>(reference);
//            if (result != null) return result;

//            throw new Exception($"Failed to provide handle for reference with scheme {reference.Scheme}.  Have you registered the relevant IHandleProvider service?");
//        }

//        public H<T> GetHandle<T>(IReference reference, T handleObject = default(T))
//        {
//            foreach(var hp in DependencyContext.Current.GetService<IEnumerable<IHandleProvider>>())
//            {
//                var h = hp.GetHandle<T>(reference, handleObject);
//                if (h != null) return h;
//            }

//            return null;
//        }
//    }
//}

