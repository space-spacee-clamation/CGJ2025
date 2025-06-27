using System;
using System.Collections.Generic;
using UnityEngine;


public class AudioResource
{
    public AudioClip clip;
    public string name;
}
    public class AudioManager :   MonoBehaviour
    {
        public static AudioManager Instance;
        public Dictionary<string, AudioResource> AudioResources=new Dictionary<string, AudioResource>();
        private void Awake()
        {
            if(Instance==null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }
        public void Start()
        {
            //TODO: Init
        }
        public void PlayOnce(string clipName)
        {
            
        }
    }
