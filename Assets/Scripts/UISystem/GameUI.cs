using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour, IUIController
{
    [field: SerializeField] public UIControllerType UIControllerType { get; set; }

    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        levelText.text = (GameManager.Instance.CurrentLevelIndex + 1).ToString();    
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}
