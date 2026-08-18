using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "ScriptableObjects/WaveConfig")]
public class WaveConfigSO : ScriptableObject
{
    [Serializable]
    public struct BotWaveValue
    {
        [Required]
        [AssetsOnly]
        public GameObject BotPrefab;
        
        [MinValue(0)]
        public int MinInWave;
        
        [MinValue("@MinInWave")]
        public int MaxInWave;
    }

    [TableList(AlwaysExpanded = true)]
    public List<BotWaveValue> BotWaves = new List<BotWaveValue>();
}