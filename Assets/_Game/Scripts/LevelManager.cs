using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    [Header("Level Scriptables")]
    [SerializeField] private LevelData[] levels;

    [Header("Color Prefab Pairs")]
    [SerializeField] private ColorPrefabPairs colorPrefabPairs;

    [Space(10)]
    [SerializeField] private GameObject levelsParent;
    [SerializeField] private GameObject levelBasePrefab;
    [SerializeField] private GameObject roadTilePrefab;
    [SerializeField] private Transform nextLevelInstantiatePoint;
    [SerializeField] private Transform levelDestroyPoint;

    [Header("Level Success Slide Tween")]
    [SerializeField] private float oldLevelSlideDuration;
    [SerializeField] private float newLevelSlideDuration;

    [SerializeField] private ParticleSystem levelSuccessConfetti;

    private GameObject currentLevel;
    private LevelData currentLevelData;
    private int currentLevelIndex;

    private bool onStart = true;

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
        EventManager.LevelSuccesEvent += MoveOldLevelToDestroyPoint;
        EventManager.LevelSuccesEvent += LevelSuccessParticles;
    }

    private void OnDisable()
    {
        EventManager.LevelSuccesEvent -= MoveOldLevelToDestroyPoint;
        EventManager.LevelSuccesEvent -= LevelSuccessParticles;
    }

    private void Start()
    {
        SetupLevels();
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

    private void CreateCurrentLevel()
    {
        Texture2D levelTexture = currentLevelData.levelTexture;

        float width = levelTexture.width;
        float height = levelTexture.height;

        // Offset for centering the level on the x axis
        Vector3 offset = new Vector3(width / 2f, 0f, height / 2f) - new Vector3(0.5f, 0f, 0.5f);

        Vector3 instantiatePosition = onStart ? Vector3.zero : nextLevelInstantiatePoint.position;
        onStart = false;

        currentLevel = Instantiate(levelBasePrefab, instantiatePosition, Quaternion.identity, levelsParent.transform);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixelColor = levelTexture.GetPixel(x, y);

                if (!CompareColors(pixelColor, Color.white))
                {
                    GameObject roadTile = Instantiate(roadTilePrefab);

                    roadTile.transform.parent = currentLevel.transform;
                    roadTile.transform.localPosition = new Vector3(x, 0f, y) - offset;
                }

                if (CompareColors(pixelColor, Color.black)) continue;

                GameObject prefab = GetPrefabFromColor(pixelColor);
                GameObject tile = Instantiate(prefab);

                tile.transform.parent = currentLevel.transform;
                tile.transform.localPosition = new Vector3(x, 0f, y) - offset;
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
                        MoveNewLevelToCenter();
                    }));
    }

    private void MoveNewLevelToCenter()
    {
        currentLevel.transform.DOMove(Vector3.zero, newLevelSlideDuration)
            .SetEase(Ease.OutCirc);
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

    private void LevelSuccessParticles()
    {
        levelSuccessConfetti.Play();
    }
}
