﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionFire.Instantiating
{
    public class InstantiationPipeline : List<IInstantiator>, IInstantiator
    {


        public object Affect(object obj, InstantiationContext context = null)
        {
            foreach (var a in this)
            {
                obj = a.Affect(obj, context);
            }

            (obj as INotifyOnInstantiated)?.OnInstantiated(context);

            return obj;
        }
    }

    //public static class InstantiationPipelineExtensions
    //{
    //    public static object Instantiate(this IEnumerable<IInstantiator> pipeline, InstantiationContext context = null)
    //    {

    //    }
    //}
}
