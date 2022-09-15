using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPrefabPairs", menuName = "ScriptableObjects/ColorPrefabPairs")]
public class ColorPrefabPairs : ScriptableObject
{
    public List<ColorPrefabPair> pairs;
}

[System.Serializable]
public struct ColorPrefabPair
{
    public Color color;
    public GameObject prefab;
}
