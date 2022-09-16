using TMPro;
using UnityEngine;

public class NumberCube : MonoBehaviour
{
    [SerializeField] private TextMeshPro numberText;
    [SerializeField] private MeshRenderer meshRenderer;

    public NumberCubeData numberCubeData;

    void Start()
    {
        numberText.text = numberCubeData.cubeNumber.ToString();
        meshRenderer.material.color = numberCubeData.cubeColor;
    }
}
