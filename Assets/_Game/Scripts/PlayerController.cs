using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    [SerializeField] private GameObject playerGraphic;

    [Header("Punch Scale Tween On Stop")]
    [SerializeField] private Vector3 punchScale;
    [SerializeField] private float punchScaleDuration;
    [SerializeField] private int punchScaleVibrato;
    [SerializeField] private float punchScaleElasticity;

    [Header("Particles")]
    [SerializeField] private ParticleSystem mergeParticle;

    [SerializeField] private LayerMask tilesLayer;
    [SerializeField] private TextMeshPro numberText;

    [HideInInspector] public int currentPlayerNumber;

    private float initialYValue;

    public static bool canMove { get; private set; } = true;
    private const int MAX_RAY_DISTANCE = 50;

    private void OnEnable()
    {
        EventManager.MoveEvent += Move;
        EventManager.MergeNumbersEvent += MergeNumbers;
    }

    private void OnDisable()
    {
        EventManager.MoveEvent -= Move;
        EventManager.MergeNumbersEvent -= MergeNumbers;
    }

    private void Start()
    {
        currentPlayerNumber = LevelManager.Instance.initialPlayerNumber;
        numberText.text = currentPlayerNumber.ToString();

        initialYValue = playerGraphic.transform.localPosition.y;

        JumpTween();
    }

    private void Move(Vector3 direction)
    {
        if (canMove)
        {
            canMove = false;
            Vector3 targetPosition = transform.position;

            if (Physics.Raycast(transform.position + new Vector3(0f, 0.7f, 0f), direction, out RaycastHit hit, MAX_RAY_DISTANCE, tilesLayer))
            {   
                float duration;
                NumberCube hitCube = null;
                bool merge = false;

                if (hit.transform.CompareTag("WallTile"))
                {
                    targetPosition = CalculateTargetPosisition(hit, direction);
                }
                else if (hit.transform.CompareTag("NumberCube"))
                {
                    hitCube = hit.transform.GetComponent<NumberCube>();

                    if (hitCube.number == currentPlayerNumber)
                    {
                        targetPosition = hitCube.transform.position;
                        merge = true;
                    }
                    else
                    {
                        targetPosition = CalculateTargetPosisition(hit, direction);
                    }
                }
                
                duration = CalculateMovementDuration(transform.position, targetPosition);
                transform.DOMove(targetPosition, duration).SetEase(Ease.Linear)
                    .OnComplete(() =>
                {
                    PunchScaleOnStop(direction);

                    if (merge)
                    {
                        EventManager.CallMergeNumbersEvent();

                        Destroy(hit.transform.gameObject);

                        mergeParticle.Play();
                    }
                    canMove = true;
                });
            }
        }
    }

    private float CalculateMovementDuration(Vector3 currentPosition, Vector3 targetPosition)
    {
        return Vector3.Distance(currentPosition, targetPosition) * movementSpeed;
    }

    private Vector3 CalculateTargetPosisition(RaycastHit hit, Vector3 direction)
    {
        Vector3 targetPosition;

        if (direction == Vector3.left)
        {
            targetPosition = hit.transform.position - Vector3.left;
        }
        else if (direction == Vector3.right)
        {
            targetPosition = hit.transform.position - Vector3.right;
        }
        else if (direction == Vector3.forward)
        {
            targetPosition = hit.transform.position - Vector3.forward;
        }
        else
        {
            targetPosition = hit.transform.position - Vector3.back;
        }

        return targetPosition;
    }

    private void MergeNumbers()
    {
        currentPlayerNumber *= 2;

        if (currentPlayerNumber == 2048)
        {
            canMove = false;

            EventManager.CallLevelSuccessEvent();
        }

        numberText.text = currentPlayerNumber.ToString();
    }

    private void PunchScaleOnStop(Vector3 direction)
    {
        DOTween.CompleteAll();

        if (direction == Vector3.left || direction == Vector3.right)
        {
            transform.DOPunchScale(new Vector3(-punchScale.x, punchScale.y, punchScale.z), punchScaleDuration, punchScaleVibrato, punchScaleElasticity);
        }
        else if (direction == Vector3.forward || direction == Vector3.back)
        {
            transform.DOPunchScale(new Vector3(punchScale.x, punchScale.y, -punchScale.z), punchScaleDuration, punchScaleVibrato, punchScaleElasticity);
        }
    }

    private void JumpTween()
    {
        playerGraphic.transform.DOMoveY(initialYValue + 0.8f, 0.2f)
            .SetLoops(-1, LoopType.Yoyo);
    }
}