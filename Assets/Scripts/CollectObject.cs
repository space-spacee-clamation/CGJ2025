    using System;
    using UnityEngine;
    public class CollectObject : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                CollectionManager.Instance.Collection( this);
                DialogManager.Instance.ShowDialog("啊啊啊啊啊啊啊啊", transform.position);
            }
        }
    }
