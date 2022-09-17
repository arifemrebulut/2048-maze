using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Scriptables")]
    [SerializeField] private LevelData[] levels;

    [Header("Color Prefab Pairs")]
    [SerializeField] private ColorPrefabPairs colorPrefabPairs;

    [Space(10)]
    [SerializeField] private GameObject levelParent;
    [SerializeField] private GameObject levelBasePrefab;    

    private int currentLevelIndex;
    private LevelData currentLevel;

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

    private void Start()
    {
        currentLevelIndex = GameManager.Instance.CurrentLevelIndex;
        currentLevel = levels[currentLevelIndex];
        initialPlayerNumber = currentLevel.initialPlayerNumber;

        SetupLevel();
    }

    private void SetupLevel()
    {
        Texture2D levelTexture = currentLevel.levelTexture;

        float width = levelTexture.width;
        float height = levelTexture.height;

        // Offset for centering the level on the x axis
        Vector3 offset = new Vector3(width / 2f, 0f, height / 2f) - new Vector3(0.5f, 0f, 0.5f);

        // Level Base
        GameObject levelBase = Instantiate(levelBasePrefab, levelParent.transform);
        levelBase.transform.localScale = new Vector3(width, levelBase.transform.localScale.y, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject prefab = CreatePrefab(levelTexture.GetPixel(x, y), new Vector3(x, 0f, y), offset, levelParent.transform);

                if (prefab.CompareTag("Player"))
                {
                    prefab.transform.parent = null;
                }
            }
        }
    }

    private GameObject CreatePrefab(Color color, Vector3 position, Vector3 offset, Transform parentObject)
    {
        GameObject prefab = GetPrefabFromColor(color);

        return Instantiate(prefab, position - offset, Quaternion.identity, parentObject);
    }

    private GameObject GetPrefabFromColor(Color color)
    {
        ColorPrefabPair pair = colorPrefabPairs.pairs.Find(x =>
        {
            return x.color.r == color.r
                && x.color.g == color.g
                && x.color.b == color.b;
        });

        return pair.prefab;
    }
}
