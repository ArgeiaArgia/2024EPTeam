using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Boat Setting")] [SerializeField]
    private Vector2 boatSize;

    [field: SerializeField] public Vector2 DoorPos { get; set; }
    [field: SerializeField] public Vector2 KitchenPos { get; set; }

    [SerializeField] private Vector2 boatCenter;
    [Header("Player Setting")] public float playerSpeed;
    [SerializeField] private Transform shipTransform;
    [field: SerializeField] public float CatchPercentage { get; set; }
    [field: SerializeField] public float FishWaitingTime {get; set;}
    [field: SerializeField] public float SleepTerm {get; set;}
    [field: SerializeField] public int SleepGetOver {get; set;}
    
    public UnityEvent OnMiniGameStartEvent;
    public UnityEvent OnMiniGameEndEvent;
    public UnityEvent FishStartEvent;
    public UnityEvent<ItemSO> CookStartEvent;
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public InGameUI InGameUI { get; set; }
    [field: SerializeField] public StatManager StatManager { get; set; }
    [field: SerializeField] public FishMiniGameUI FishMiniGameUI { get; set; }
    private Camera _mainCamera;

    public Rigidbody2D RigidbodyComponent { get; private set; }
    public Animator AnimatorComponent { get; private set; }
    public SpriteRenderer SpriteRendererComponent { get; private set; }

    private PlayerStateMachine _stateMachine;

    private Vector2 _targetPosition;

    public Vector2 TargetPosition
    {
        get
        {
            if (_stateMachine.TargetState.GetType() == typeof(CookState))
            {
                return KitchenPos + boatCenter + (Vector2)shipTransform.position;
            }

            return _targetPosition;
        }
        set
        {
            _targetPosition =
                new Vector2(
                    Mathf.Clamp(value.x, -boatSize.x + boatCenter.x + shipTransform.position.x,
                        boatSize.x + boatCenter.x + shipTransform.position.x), -0.2f);
        }
    }

    public event Action<Vector2> OnMouseDownEvent;

    private void Awake()
    {
        RigidbodyComponent = GetComponent<Rigidbody2D>();
        AnimatorComponent = GetComponent<Animator>();
        SpriteRendererComponent = GetComponent<SpriteRenderer>();

        _mainCamera = Camera.main;

        _stateMachine = new PlayerStateMachine();

        _stateMachine.AddState(new IdleState(this, _stateMachine));
        _stateMachine.AddState(new MoveState(this, _stateMachine));
        _stateMachine.AddState(new FishState(this, _stateMachine));
        _stateMachine.AddState(new CookState(this, _stateMachine));
        _stateMachine.AddState(new CraftState(this, _stateMachine));
        _stateMachine.AddState(new SleepState(this, _stateMachine));

        _stateMachine.Initialize(typeof(IdleState), this);

        InputReader.OnMoveDownEvent += (pos) =>
        {
            var worldPos = _mainCamera.ScreenToWorldPoint(pos);
            if (worldPos.x < -boatSize.x + boatCenter.x + shipTransform.position.x || worldPos.x > boatSize
                    .x + boatCenter.x + shipTransform.position.x || worldPos.y < -boatSize.y + boatCenter.y - 1 ||
                worldPos.y > boatSize.y + boatCenter.y + 1) return;
            OnMouseDownEvent?.Invoke(worldPos);
        };
    }

    private void Update()
    {
        _stateMachine.CurrentState.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.CurrentState.FixedUpdate();
        if (RigidbodyComponent.velocity.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (RigidbodyComponent.velocity.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void SetTargetStateToFishState()
    {
        _stateMachine.SetTargetState<FishState>();
    }

    public void SetTargetStateToCookState()
    {
        _stateMachine.SetTargetState<CookState>();
    }

    public void SetTargetStateToCraftState()
    {
        _stateMachine.SetTargetState<CraftState>();
    }
    
    public void SetTargetStateToSleepState()
    {
        _stateMachine.SetTargetState<SleepState>();
    }
    
    public void CoroutineStarter(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
    public void CoroutineStopper(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
    }
    public void EndActionState()
    {
        if(_stateMachine.CurrentState.GetType() == typeof(FishState) || _stateMachine.CurrentState.GetType() == typeof(CookState))
            _stateMachine.ResetToIdleState();
    }

    public bool CheckIfPlayerNearPosition()
    {
        var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(mousePos, TargetPosition) < 0.5f)
        {
            TargetPosition = mousePos;
            return true;
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boatCenter + (Vector2)shipTransform.position, boatSize * 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(DoorPos.x + +boatCenter.x + shipTransform.position.x, DoorPos.y), 0.5f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(new Vector3(KitchenPos.x + +boatCenter.x + shipTransform.position.x, KitchenPos.y), 0.5f);
    }
#endif
}