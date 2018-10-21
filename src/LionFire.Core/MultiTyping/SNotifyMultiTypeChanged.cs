﻿using System;

namespace LionFire.MultiTyping
{

    public interface SNotifyMultiTypeChanged
    {

        void AddTypeHandler(Type type, Action<SReadOnlyMultiTypedEx, Type> callback);
        void RemoveTypeHandler(Type type, Action<SReadOnlyMultiTypedEx, Type> callback);
        //void AddTypeHandler<T>(Type type, MulticastDelegate callback) where T:class;
        //void RemoveTypeHandler<T>(Type type, MulticastDelegate callback) where T:class;
    }
}
