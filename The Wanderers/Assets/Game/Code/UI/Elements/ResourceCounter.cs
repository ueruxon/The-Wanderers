using System;
using Game.Code.Data.Game;
using Game.Code.Infrastructure.Services.Progress;
using Game.Code.Logic.ResourcesLogic;
using TMPro;
using UnityEngine;

namespace Game.Code.UI.Elements
{
    public class ResourceCounter : MonoBehaviour
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private TMP_Text _counter;

        private ResourceStorageData _resourceStorageData;

        public void Init(IGameProgressService progressService)
        {
            _resourceStorageData = progressService.Progress.ResourceStorageData;
            _resourceStorageData.ResourceChanged += OnResourceChanged;

            OnResourceChanged(_resourceType);
        }

        private void OnResourceChanged(ResourceType resourceType)
        {
            if (resourceType == _resourceType)
            {
                int count = _resourceStorageData.StoredAmountResourceByType[resourceType];
                _counter.SetText(count.ToString());
            }
                
        }

        private void OnDestroy() => 
            _resourceStorageData.ResourceChanged -= OnResourceChanged;
    }
}