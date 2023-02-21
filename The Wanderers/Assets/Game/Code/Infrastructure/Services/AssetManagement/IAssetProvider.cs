using UnityEngine;

namespace Game.Code.Infrastructure.Services.AssetManagement
{
    public interface IAssetProvider : IService
    {
        public T Instantiate<T>(string path, Vector3 at);
        public T Instantiate<T>(string path);
        public T Load<T>(string path);
        public T[] LoadAll<T>(string path);
    }
}