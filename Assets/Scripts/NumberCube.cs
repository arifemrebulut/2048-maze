using TMPro;
using UnityEngine;

public class NumberCube : MonoBehaviour
{
    [SerializeField] private TextMeshPro numberText;

    public NumberCubeData numberCubeData;

    void Start()
    {
        numberText.text = numberCubeData.cubeNumber.ToString();
    }
}
