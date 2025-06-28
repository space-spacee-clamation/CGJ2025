    using System;
    using UnityEngine;
    public class CollectObject : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.tag == "Player");
        }
    }
