using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [Header("Level Scriptables")]
    [SerializeField] private LevelData[] levels;

    [Header("Color Prefab Pairs")]
    [SerializeField] private ColorPrefabPairs colorPrefabPairs;

    [Space(10)]
    [SerializeField] private GameObject levelsParent;
    [SerializeField] private GameObject levelBasePrefab;
    [SerializeField] private Transform nextLevelInstantiatePoint;
    [SerializeField] private Transform levelDestroyPoint;

    [Header("Move Level Tween")]
    [SerializeField] private float moveDuration;

    private GameObject currentLevel;
    private GameObject nextLevel;
    private GameObject oldLevel;

    private LevelData currentLevelData;
    private LevelData nextLevelData;
    private int currentLevelIndex;
    private int nextLevelIndex;

    public int initialPlayerNumber { get; private set; }

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
    }

    private void OnEnable()
    {
        EventManager.LevelSuccesEvent += MoveLevels;
    }

    private void OnDisable()
    {
        EventManager.LevelSuccesEvent -= MoveLevels;
    }

    private void Start()
    {
        SetupLevels();
    }

    private void SetupLevels()
    {
        GetLevelInformations();
        CreateCurrentLevel();
        CreateNextLevel();
    }

    private void GetLevelInformations()
    {
        currentLevelIndex = GameManager.Instance.CurrentLevelIndex;
        nextLevelIndex = currentLevelIndex + 1;

        currentLevelData = levels[currentLevelIndex];
        nextLevelData = levels[nextLevelIndex];

        initialPlayerNumber = currentLevelData.initialPlayerNumber;
    }

    private void CreateCurrentLevel()
    {
        Texture2D levelTexture = currentLevelData.levelTexture;

        float width = levelTexture.width;
        float height = levelTexture.height;

        // Offset for centering the level on the x axis
        Vector3 offset = new Vector3(width / 2f, 0f, height / 2f) - new Vector3(0.5f, 0f, 0.5f);

        currentLevel = Instantiate(levelBasePrefab, levelsParent.transform);
        currentLevel.name = "CurrentLevel";

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixelColor = levelTexture.GetPixel(x, y);

                if (CompareColors(pixelColor, Color.black))
                {
                    continue;
                }

                GameObject prefab = CreatePrefab(pixelColor, new Vector3(x, 0f, y), offset);

                if (prefab.CompareTag("Player"))
                {
                    prefab.transform.parent = currentLevel.transform;
                }
                else if (prefab.CompareTag("WallTile"))
                {
                    prefab.transform.parent = currentLevel.transform.Find("Walls");
                }
                else if (prefab.CompareTag("NumberCube"))
                {
                    prefab.transform.parent = currentLevel.transform.Find("NumberCubes");
                }
            }
        }
    }

    private void CreateNextLevel()
    {
        Texture2D levelTexture = nextLevelData.levelTexture;

        float width = levelTexture.width;
        float height = levelTexture.height;

        // Offset for centering the level on the x axis
        Vector3 offset = new Vector3(width / 2f, 0f, height / 2f) - new Vector3(0.5f, 0f, 0.5f);

        nextLevel = Instantiate(levelBasePrefab, nextLevelInstantiatePoint.position, Quaternion.identity, levelsParent.transform);
        nextLevel.name = "NextLevel";

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixelColor = levelTexture.GetPixel(x, y);

                if (CompareColors(pixelColor, Color.black))
                {
                    continue;
                }

                GameObject prefab = CreatePrefab(pixelColor, nextLevelInstantiatePoint.position + new Vector3(x, 0f, y), offset);

                if (prefab.CompareTag("Player"))
                {
                    prefab.transform.parent = nextLevel.transform;
                }
                else if (prefab.CompareTag("WallTile"))
                {
                    prefab.transform.parent = nextLevel.transform.Find("Walls");
                }
                else if (prefab.CompareTag("NumberCube"))
                {
                    prefab.transform.parent = nextLevel.transform.Find("NumberCubes");
                }
            }
        }
    }

    private void MoveLevels()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(currentLevel.transform.DOMove(levelDestroyPoint.position, moveDuration)
                    .SetEase(Ease.Linear))
                .Join(nextLevel.transform.DOMove(Vector3.zero, moveDuration)
                    .SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    oldLevel = currentLevel;
                    currentLevel = nextLevel;

                    GetLevelInformations();

                    CreateNextLevel();

                    Destroy(oldLevel);
                });
    }

    private GameObject CreatePrefab(Color color, Vector3 position, Vector3 offset)
    {
        GameObject prefab = GetPrefabFromColor(color);

        return Instantiate(prefab, position - offset, Quaternion.identity);
    }

    private GameObject GetPrefabFromColor(Color color)
    {
        ColorPrefabPair pair = colorPrefabPairs.pairs.Find(x => CompareColors(x.color, color));

        return pair.prefab;
    }

    private bool CompareColors(Color color1, Color color2)
    {
        return color1.r == color2.r
            && color1.g == color2.g
            && color1.b == color2.b;
    }
}
