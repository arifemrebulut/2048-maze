using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public int initialPlayerNumber;

    [Header("Camera Settings for Level")]
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;

    public Texture2D levelTexture;
}