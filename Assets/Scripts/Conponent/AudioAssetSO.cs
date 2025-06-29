using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AudioAssetSO.asset", menuName = "ScriptableObject/Audio", order = 1)]
public class AudioAssetSO : ScriptableObject
{
    public List<AudioResourceDataPAre> clips ;
}

[Serializable]
public class AudioResourceDataPAre
{
    public AudioClip clip;
    public string name;
}