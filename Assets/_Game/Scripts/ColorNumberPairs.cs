using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorNumberPairs", menuName = "ScriptableObjects/ColorNumberPairs")]
public class ColorNumberPairs : ScriptableObject
{
    public List<ColorNumberPair> pairs;
}

[System.Serializable]
public struct ColorNumberPair
{
    public Color color;
    public string number;
}