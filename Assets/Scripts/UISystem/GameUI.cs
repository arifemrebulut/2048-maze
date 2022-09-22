using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour, IUIController
{
    [field: SerializeField] public UIControllerType UIControllerType { get; set; }

    [SerializeField] private TextMeshProUGUI levelText;

    private int levelIndex;

    private void Start()
    {
        levelIndex = (GameManager.Instance.CurrentLevelIndex + 1);
        levelText.text = "LEVEL " + levelIndex.ToString();
    }

    private void OnEnable()
    {
        EventManager.LevelSuccesEvent += IncreaseLevelNumber;
    }

    private void OnDisable()
    {
        EventManager.LevelSuccesEvent -= IncreaseLevelNumber;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    private void IncreaseLevelNumber()
    {
        levelIndex++;
        levelText.text = "LEVEL " + levelIndex.ToString();
    }
}
