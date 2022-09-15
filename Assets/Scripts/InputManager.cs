using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float minSwipeDelta;

    private Vector2 mouseDownPosition;
    private Vector2 mouseUpPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseUpPosition = Input.mousePosition;

            DetectSwipe(mouseDownPosition, mouseUpPosition);
        }
    }

    private void DetectSwipe(Vector2 startPosition, Vector2 endPosition)
    {
        float xDelta = startPosition.x - endPosition.x;
        float yDelta = startPosition.y - endPosition.y;

        float xDeltaAbs = Mathf.Abs(xDelta);
        float yDeltaAbs = Mathf.Abs(yDelta);

        if (xDeltaAbs > yDeltaAbs)
        {
            if (xDeltaAbs > minSwipeDelta && xDelta > 0)
            {
                EventManager.CallMoveEvent(Vector3.left);
            }
            else if (xDeltaAbs > minSwipeDelta && xDelta < 0)
            {
                EventManager.CallMoveEvent(Vector3.right);
            }
        }
        else
        {
            if (yDeltaAbs > minSwipeDelta && yDelta > 0)
            {
                EventManager.CallMoveEvent(Vector3.back);
            }
            else if (yDeltaAbs > minSwipeDelta && yDelta < 0)
            {
                EventManager.CallMoveEvent(Vector3.forward);
            }
        }
    }
}