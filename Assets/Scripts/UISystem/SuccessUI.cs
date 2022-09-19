using UnityEngine;

public class SuccessUI : MonoBehaviour, IUIController
{
    [field: SerializeField] public UIControllerType UIControllerType { get; set; }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}