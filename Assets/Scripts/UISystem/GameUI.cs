using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour, IUIController
{
    [field: SerializeField] public UIControllerType UIControllerType { get; set; }

    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        EventManager.LevelSuccesEvent += UpdateLevelText;
    }

    private void OnDisable()
    {
        EventManager.LevelSuccesEvent -= UpdateLevelText;
    }

    private void Start()
    {
        UpdateLevelText();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    private void UpdateLevelText()
    {
        levelText.text = (GameManager.Instance.CurrentLevelIndex + 1).ToString();
    }
}
