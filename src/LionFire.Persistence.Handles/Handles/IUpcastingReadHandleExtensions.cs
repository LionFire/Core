﻿#if false // Deferring to ILazilyResolvesFallbackExtensions
using System.Threading.Tasks;
using LionFire.Resolves;
using LionFire.Results;

namespace LionFire.Persistence // Put in LionFire.ExtensionMethods.Persistence namespace?
{
    public static class IUpcastingReadHandleExtensions
    {

        /// <summary>
        /// Fallback to provide ILazilyResolves<T>.Get to RH<T>
        /// Also makes Object return value strongly typed for the covariant ILazilyResolves.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="rh"></param>
        /// <returns></returns>
        public static async Task<IResolveResult<TValue>> GetValue<TValue>(this IReadHandleBase<TValue> rh) // RENAME: TryResolveNonNull
                where TValue : class // REVIEW
        {
            //if (rh == null) return (false, default); OLD
            if (rh == null) return NoopFailResolveResult<TValue>.Instance;

            if (rh is ILazilyResolves<TValue> lr)
            {
                return await lr.GetValue().ConfigureAwait(false);
                //return (result.HasValue, result.Value);
            }
            else
            {
                var value = rh.Value;
                if (rh.HasValue)
                {
                    return new ResolveResult<TValue>(true, value);
                }
                else
                {
                    return await rh.Retrieve().ConfigureAwait(false);
                }
                //return (obj != default, obj);  OLD
            }

            //return (await rh.TryResolveObject().ConfigureAwait(false)) && rh.HasObject;  OLD
            //return await Task.Factory.StartNew(() =>
            //{
            //    var _ = rh.Object;
            //    return _ != null;
            //}).ConfigureAwait(false);
        }
    }
}
#endif