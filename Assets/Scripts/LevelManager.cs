using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Scriptables")]
    [SerializeField] private LevelData[] levels;

    [Header("Color Prefab Pairs")]
    [SerializeField] private ColorPrefabPairs colorPrefabPairs;

    [Space(10)]
    [SerializeField] private GameObject levelParent;

    private int currentLevelIndex;
    private LevelData currentLevel;

    public int initialPlayerNumber { get; private set; }

    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null)

        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        currentLevelIndex = GameManager.instance.currentLevelIndex;
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

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreatePrefab(levelTexture.GetPixel(x, y), new Vector3(x, 0f, y), offset, levelParent.transform);
            }
        }
    }

    private GameObject CreatePrefab(Color color, Vector3 position, Vector3 offset, Transform parentObject)
    {
        GameObject prefab = GetPrefabFromColor(color);

        Debug.Log("Prefab : " + prefab.name);

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
