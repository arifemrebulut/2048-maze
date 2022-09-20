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
        //uuulevelBase.transform.localScale = new Vector3(width, levelBase.transform.localScale.y, height);

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
                    prefab.transform.parent = null;
                }
                else if (prefab.CompareTag("WallTile"))
                {
                    prefab.transform.parent = levelParent.transform.Find("Walls");
                }
                else if (prefab.CompareTag("NumberCube"))
                {
                    prefab.transform.parent = levelParent.transform.Find("NumberCubes");
                }
            }
        }
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
