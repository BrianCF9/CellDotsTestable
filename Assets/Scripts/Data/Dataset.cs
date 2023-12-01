using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dataset
{
    [SerializeField] private List<Entry> growth_distribution;

    public List<Entry> Entries => growth_distribution;

    public override string ToString()
    {
        string result = "";
        if (growth_distribution == null)
        {
            return result;
        }
        foreach (var entry in growth_distribution)
        {
            result += $"GrowthDurationInSeconds: {entry.GrowthDurationInSeconds}, MaxSize: {entry.MaxSize}\n";
        }
        return result;
    }

}
[System.Serializable]
public class Entry
{
    [SerializeField] private int ID;
    [SerializeField] private float varSecond;
    [SerializeField] private float varSize;

    #region Properties
    public int GetID => ID;
    public float GrowthDurationInSeconds => varSecond;
    public float MaxSize => varSize;
    #endregion

}