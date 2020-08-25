﻿using LionFire.Assets;
using LionFire.Persistence;
using LionFire.Persistence.Handles;
using LionFire.Referencing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LionFire.Assets
{
    public interface IAssetReadHandle : IReferencable<IAssetReference>, IReadHandle
    {
    }
    public interface IHRAsset : IAssetReadHandle // PORTINGGUIDE IHRAsset > RAsset
    {
        // PORTINGGUIDE - Type > TreatAsType
    } // TEMP TOPORT

    public class RAsset<TValue> : ReadHandlePassthrough<TValue, IAssetReference>, IAssetReadHandle
        where TValue : IAsset<TValue>
    {

        #region Construction and Implicit Operators

        public static implicit operator RAsset<TValue>(string assetPath) => assetPath == default ? default :  new RAsset<TValue> { Reference = new AssetReference<TValue>(assetPath) };
        public static implicit operator RAsset<TValue>(TValue asset) => Object.Equals(asset, default(TValue)) ? default : new RAsset<TValue> { Reference = (AssetReference<TValue>)asset.Reference, Value = asset };
        public static implicit operator RAsset<TValue>(RWAsset<TValue> asset) => asset == null ? null : new RAsset<TValue>(asset.ReadWriteHandle); // TOFLYWEIGHT
        public static implicit operator AssetReference<TValue>(RAsset<TValue> asset) => asset == null ? null : asset.Reference; 
        public static implicit operator TValue(RAsset<TValue> rAsset) => rAsset == null ? default : rAsset.Value;

        public RAsset() { }
        public RAsset(IReadHandle<TValue> handle) : base(handle) { }

        #endregion
        
        public string AssetPath => Reference.Path;
        public new AssetReference<TValue> Reference { get => (AssetReference<TValue>)base.Reference; set => base.Reference = value; }

        public static RAsset<TValue> Get(string assetPath)
            => assetPath;

        public override string ToString() => Reference.ToString();
    }

    public static class RAsset
    {
        public static RAsset<TValue> Get<TValue>(string assetPath)
            where TValue : IAsset<TValue> => assetPath;
    }


    public static class RAssetExtensions
    {
        // UNUSED UNNEEDED?
        public static RAsset<TValue> ToRAsset<TValue>(this IReadHandle<TValue> readHandle)
            where TValue : IAsset<TValue>
            => new RAsset<TValue>(readHandle);

        public static RAsset<TValue> ToRAsset<TValue>(this string assetPath)
         where TValue : IAsset<TValue>
         => assetPath;
    }
}