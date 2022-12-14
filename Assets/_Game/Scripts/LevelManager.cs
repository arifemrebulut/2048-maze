using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Linq;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Level Scriptables")]
    [SerializeField] private LevelData[] levels;

    [SerializeField] private ColorNumberPairs colorNumberPairs;

    [Header("Level Prefabs")] [Space(10)]
    [SerializeField] private GameObject levelsParent;
    [SerializeField] private GameObject levelBasePrefab;
    [SerializeField] private Transform nextLevelInstantiatePoint;
    [SerializeField] private Transform levelDestroyPoint;

    [Header("Tile Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject numberCubePrefab;
    [SerializeField] private GameObject wallTilePrefab;
    [SerializeField] private GameObject roadTilePrefab;
    [SerializeField] private GameObject levelBorderCube;

    [Header("Camera Tweens")]
    [SerializeField] private float cameraPositionLerpDuration;
    [SerializeField] private float cameraRotationLerpDuration;

    [Header("Level Success Slide Tween")]
    [SerializeField] private float oldLevelSlideDuration;
    [SerializeField] private float newLevelSlideDuration;

    [SerializeField] private ParticleSystem levelSuccessConfetti;

    private Camera mainCamera;

    private GameObject currentLevel;
    private LevelData currentLevelData;
    private int currentLevelIndex;

    private bool onStart = true;

    public int initialPlayerNumber { get; private set; }
    public int levelCount { get { return levels.Length; } }

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)

        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        EventManager.LevelSuccesEvent += MoveOldLevelToDestroyPoint;
        EventManager.LevelSuccesEvent += LevelSuccessParticles;
        EventManager.RestartLevelEvent += RestartLevel;
    }

    private void OnDisable()
    {
        EventManager.LevelSuccesEvent -= MoveOldLevelToDestroyPoint;
        EventManager.LevelSuccesEvent -= LevelSuccessParticles;
        EventManager.RestartLevelEvent -= RestartLevel;
    }

    private void Start()
    {
        SetupLevels();
        SetCameraTransform(false);
    }

    private void SetupLevels()
    {
        GetLevelInformations();
        CreateCurrentLevel();
    }

    private void GetLevelInformations()
    {
        currentLevelIndex = GameManager.Instance.CurrentLevelIndex;
        currentLevelData = levels[currentLevelIndex];

        initialPlayerNumber = currentLevelData.initialPlayerNumber;
    }

    private void CreateCurrentLevel(bool onPlace = false)
    {
        Texture2D levelTexture = currentLevelData.levelTexture;

        float width = levelTexture.width;
        float height = levelTexture.height;

        // Offset for centering the level
        Vector3 offset = new Vector3(width / 2f, 0f, height / 2f) - new Vector3(0.5f, 0f, 0.5f);

        Vector3 instantiatePosition = onStart || onPlace ? Vector3.zero : nextLevelInstantiatePoint.position;
        onStart = false;

        currentLevel = Instantiate(levelBasePrefab, instantiatePosition, Quaternion.identity, levelsParent.transform);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {   
                Color pixelColor = levelTexture.GetPixel(x, y);

                if (pixelColor.a == 0)
                {
                    GameObject borderCube = Instantiate(levelBorderCube);
                    borderCube.transform.parent = currentLevel.transform;
                    borderCube.transform.localPosition = new Vector3(x, 0f, y) - offset;
                }
                else
                {
                    if (!CompareColors(pixelColor, Color.white))
                    {
                        GameObject roadTile = Instantiate(roadTilePrefab);

                        roadTile.transform.parent = currentLevel.transform;
                        roadTile.transform.localPosition = new Vector3(x, 0f, y) - offset;
                    }

                    GameObject prefab = GetPrefabFromColor(pixelColor);
                    GameObject tile = Instantiate(prefab);

                    tile.transform.parent = currentLevel.transform;
                    tile.transform.localPosition = new Vector3(x, 0f, y) - offset;

                    if (tile.CompareTag("NumberCube"))
                    {
                        tile.GetComponent<NumberCube>().number = int.Parse(GetNumberFromColor(pixelColor));

                        GameObject tileGraphic = tile.transform.GetChild(0).gameObject;
                        tileGraphic.GetComponent<MeshRenderer>().material.color = pixelColor;
                        tileGraphic.GetComponentInChildren<TextMeshPro>().text = GetNumberFromColor(pixelColor);
                    }
                }
            }
        }
    }

    private void MoveOldLevelToDestroyPoint()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(currentLevel.transform.DOMove(levelDestroyPoint.position, oldLevelSlideDuration)
                    .SetDelay(0.5f)
                    .SetEase(Ease.InCirc)
                    .OnComplete(() =>
                    {
                        Destroy(currentLevel);

                        GetLevelInformations();
                        CreateCurrentLevel();
                        SetCameraTransform();
                        MoveNewLevelToCenter();
                    }));
    }

    private GameObject GetPrefabFromColor(Color color)
    {
        GameObject prefab;

        if (color == Color.red)
        {
            prefab = playerPrefab;
        }
        else if (color == Color.white)
        {
            prefab = wallTilePrefab;
        }
        else if (color == Color.black)
        {
            prefab = roadTilePrefab;
        }
        else
        {
            prefab = numberCubePrefab;
        }

        return prefab;
    }

    private string GetNumberFromColor(Color color)
    {
        ColorNumberPair pair = colorNumberPairs.pairs.Find(x => CompareColors(x.color, color));

        return pair.number;
    }

    private void MoveNewLevelToCenter()
    {
        currentLevel.transform.DOMove(Vector3.zero, newLevelSlideDuration)
            .SetEase(Ease.OutCirc);
    }

    private void SetCameraTransform(bool lerp = true)
    {
        if (lerp)
        {
            mainCamera.transform.DOMove(currentLevelData.cameraPosition, cameraPositionLerpDuration).SetEase(Ease.OutSine);
            mainCamera.transform.DORotate(currentLevelData.cameraRotation, cameraRotationLerpDuration).SetEase(Ease.OutSine);
        }
        else
        {
            mainCamera.transform.position = currentLevelData.cameraPosition;
            mainCamera.transform.localEulerAngles = currentLevelData.cameraRotation;
        }
    }

    private bool CompareColors(Color color1, Color color2)
    {
        return Mathf.Abs(color1.r - color2.r) <= 0.01f
            && Mathf.Abs(color1.g - color2.g) <= 0.01f
            && Math.Abs(color1.b - color2.b) <= 0.01f;
    }

    private void LevelSuccessParticles()
    {
        levelSuccessConfetti.Play();
    }

    private void RestartLevel()
    {
        StartCoroutine(RestartLevelCoroutine());
    }

    private IEnumerator RestartLevelCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        Destroy(currentLevel);

        CreateCurrentLevel(true);
    }
}
