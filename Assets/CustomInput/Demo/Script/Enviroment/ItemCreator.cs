using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputDemo
{
    public class ItemCreator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _Prefab;
        [SerializeField]
        private Vector2Int _Size;
        [SerializeField]
        private Vector2Int _Capacity;
        [SerializeField]
        private List<ItemPair> _Items;

        public int Count => this._Items.Count(c => c.Prefab != null);

        private void Awake()
        {
            this._Items = new List<ItemPair>();
            this._Capacity = this._Size * 2 + new Vector2Int(1, 1);
        }

        public GameObject Create(Vector2Int locate)
        {
            var locateLength = LocateConvert(locate.x, locate.y);
            var capacityLength = LocateConvert(this._Capacity.x - 1, this._Capacity.y - 1);
            
            if (locateLength > capacityLength) { return null; }
            
            var pair = this._Items.Find(f => f.Locate == locate);

            var x = locate.x - this._Size.x;
            var z = locate.y - this._Size.y;

            var position = new Vector3(x, this._Prefab.transform.position.y, z);

            if (pair == null)
            {
                var prefab = Instantiate(this._Prefab, position, Quaternion.identity, this.transform);

                this._Items.Add(new ItemPair(locate, prefab));

                return prefab;
            }

            if (pair.Prefab == null) 
            {
                pair.Prefab = Instantiate(this._Prefab, position, Quaternion.identity, this.transform);

                return pair.Prefab;
            }

            return null;
        }

        public void RandomCreate(int count) 
        {
            for(var i = 1; i <= count ;) 
            {
                var x = Mathf.RoundToInt(Random.Range(0, this._Capacity.x - 1));
                var y = Mathf.RoundToInt(Random.Range(0, this._Capacity.y - 1));
                var locate = new Vector2Int(x, y);

                if (this.Create(locate)) { i++; }
            }
        }

        public GameObject RandomCreate()
        {
            var x = Mathf.RoundToInt(Random.Range(0, this._Capacity.x - 1));
            var y = Mathf.RoundToInt(Random.Range(0, this._Capacity.y - 1));
            var locate = new Vector2Int(x, y);

            return this.Create(locate);
        }

        public bool Removed(GameObject prefab)
        {
            var pair = this._Items.Find(f => f.Prefab == prefab);

            if (pair != null)
            {
                pair.Prefab = null;

                return true;
            }

            return false;
        }

        // locate 0 ~ _Capacity x + (y - 1) * x
        private int LocateConvert(int x, int y) 
        {
            return (y - 1) * this._Capacity.x + x;
        }
    }

    [System.Serializable]
    public class ItemPair
    {
        [SerializeField]
        private Vector2 _Locate;
        [SerializeField]
        private GameObject _Prefab;

        public Vector2 Locate => this._Locate;

        public GameObject Prefab 
        {
            get => this._Prefab;

            set => this._Prefab = value;
        }

        public ItemPair(Vector2 locate, GameObject prefab) 
        {
            this._Locate = locate;
            this._Prefab = prefab;
        }
    }
}