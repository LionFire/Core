﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Composables
{
    //public static class IComposableExtensions
    //{
    //    //public static TComposable Add<TComposable, TComponent>(this TComposable composable)
    //    //    where TComposable : IComposable<TComposable>
    //    //    where TComponent : class, new()
    //    //{
    //    //    var component = new TComponent();
    //    //    composable.Add(component);
    //    //    return composable;
    //    //}
    //}

    public static class IComposableExtensions
    {
        public static bool Contains<T>(IComposition c)
        {
            return c.Children.Contains(typeof(T));
        }
        public static bool Contains(IComposition c, object obj)
        {
            return c.Children.Contains(obj);
        }
    }
}
