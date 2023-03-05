using System.Collections.Generic;
using UnityEngine;

namespace BSTW.Utils
{
    [System.Serializable]
    public struct LootItem
    {
        public ObjectPooling ItemPool;
        public float Probability;
    }

    public class LootGenerator : MonoBehaviour
    {
        private List<GameObject> _currentLootItems = new List<GameObject>();

        [SerializeField] private LootItem[] _lootItems;

        [SerializeField] private int _itemsLimit = 2;

        public void ClearLootItems()
        {
            if (_currentLootItems.Count > 0)
            {
                _currentLootItems.ForEach(i => i.SetActive(false));
                _currentLootItems.Clear();
            }
        }

        public void GenerateLoot()
        {
            ClearLootItems();

            foreach (var lootItem in _lootItems)
            {
                if (Random.value <= lootItem.Probability)
                {
                    if (_currentLootItems.Count >= _itemsLimit) break;

                    var item = lootItem.ItemPool.GetObject();
                    item.transform.position = transform.position;

                    _currentLootItems.Add(item);
                }
            }
        }
    }
}
