﻿#if MOVED
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace LionFire.Collections
{

    #region Collection Interfaces

    public interface INotifyingCollection<T> : ICollection<T>, INotifyCollectionChanged<T>
#if AOT
, INotifyCollectionChanged
#endif
    {
        T[] ToArray();
    }

    #endregion

    #region List Interfaces
    
    public interface INotifyingReadOnlyList<ChildType> :
        IReadOnlyList<ChildType>,
        INotifyCollectionChanged<ChildType>
    {
    }

#endregion

}
#endif