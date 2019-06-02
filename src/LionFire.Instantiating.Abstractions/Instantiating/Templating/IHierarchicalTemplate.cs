﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionFire.Instantiating
{
    public interface IHierarchicalTemplate : ITemplate
    {
        List<ITemplate> Children { get; set; }
    }

    public interface IHierarchicalTemplate<T> : ITemplate<T>, IHierarchicalTemplate
        where T : IHierarchicalTemplateInstance, new()
    {
    }
}
