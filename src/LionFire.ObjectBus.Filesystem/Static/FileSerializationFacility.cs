﻿using LionFire.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LionFire.ObjectBus.Filesystem
{
    public static class FileSerializationFacility
    {
        public static async Task<T> DeserializePath<T>(this string path)
        {
            var result = await FSOBase.Instance.TryGet<T>(path);
            if(!result.IsSuccess()) throw new RetrieveException(result);

            return result.Value;
        }
        //public static async Task<T> DeserializeName<T>(this string name)
        //{
        //    var result = await FSOBase.Instance.TryGetName<T>(name);
        //    if (!result.IsSuccess) throw new RetrieveException(result);

        //    return result.Object;
        //}
    }
}
