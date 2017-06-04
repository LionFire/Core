﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LionFire.Instantiating
{
    public class TemplateReference
    {
        public TemplateReference() { }
        public TemplateReference(ITemplateInstance instance)
        {
            this.Template = instance.GetTemplate();
            this.TypeName = Template.GetType().FullName;
            this.Assembly = Template.GetType().AssemblyQualifiedName;
        }
        public ITemplate Template { get; set; }

        [JsonIgnore] // TODO - use custom attribute from LionFire.Runtime.Extensions
        public Type Type { get; set; }
        public string TypeName { get; set; }
        public string Assembly { get; set; }
    }

    //public class TemplateReference<TemplateType, I>
    //    where TemplateType : class, ITemplate<I>
    //    where I : class, ITemplateInstance<TemplateType>, new()
    //{
    //    public TemplateReference() { }
    //    public TemplateReference(I instance)
    //    {
    //        this.Template = instance.Template;
    //        this.Type = Template.GetType().FullName;
    //        this.Assembly = Template.GetType().AssemblyQualifiedName ;
    //    }

    //    public string Type { get; set; }
    //    public string Assembly { get; set; }

    //    public ITemplate Template { get; set; }
    //}

}
