using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Code.Infrastructure.Services.AssetManagement
{
    public class ResourceLoader
    {
        public static T Load<T>(string filepath) where T : Object
        {
            var data = Resources.Load<T>(filepath);
            if(data is null)
                throw new NullReferenceException($"Asset of type {typeof(T)} from {filepath} can't be loaded");

            return data;
        }
        
        public static T[] LoadAll<T>(string path) where T : Object
        {
            var data = Resources.LoadAll<T>(path);
            if(data is null)
                throw new NullReferenceException($"Assets of type {typeof(T)} from {path} can't be loaded");

            return data;
        }

        // public static async Task<T> LoadAsync<T>(string filepath) where T : Object
        // {
        //     var asyncHandle = Resources.LoadAsync<T>(filepath);
        //     await AsyncWaitersFactory.WaitUntil(() => asyncHandle.isDone);
        //     if(asyncHandle.asset == null)
        //         throw new NullReferenceException($"Asset of type {typeof(T)} from {filepath} can't be loaded");
        //     if(asyncHandle.asset is not T castedAsset)
        //         throw new InvalidCastException($"Asset of type {typeof(T)} from {filepath} can't be casted");
        //     return castedAsset;
        // }
    }
}