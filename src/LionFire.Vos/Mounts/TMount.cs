﻿using LionFire.Referencing;
using LionFire.Instantiating;

namespace LionFire.Vos.Mounts
{
    // REVIEW - Consider renaming this to Mount, and Mount to MountState

    public class TMount : ITemplate<Mount>, ITMount
    {
        public TMount() { }
        public TMount(IVobReference vobReference, IReference reference, IMountOptions options = null)
        {
            MountPoint = vobReference;
            Reference = reference;
            Options = options;
        }

        public IVobReference MountPoint { get; set; }

        public IReference Reference { get; set; }

        public IMountOptions Options { get; set; }

        public override string ToString() => $"{{TMount {MountPoint} --> {Reference}}}";
    }
}
