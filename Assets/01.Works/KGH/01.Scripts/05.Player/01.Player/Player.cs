using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Boat Setting")] [SerializeField]
    private Vector2 boatSize;

    [SerializeField] private Vector2 boatCenter;
    [Header("Player Setting")] public float playerSpeed;
    [SerializeField] private Transform shipTransform;
    public UnityEvent OnMiniGameStartEvent;
    public UnityEvent OnMiniGameEndEvent;
    [field: SerializeField] public InputReader InputReader { get; private set; }
    private Camera _mainCamera;

    public Rigidbody2D RigidbodyComponent { get; private set; }
    public Animator AnimatorComponent { get; private set; }

    private PlayerStateMachine _stateMachine;

    private Vector2 _targetPosition;

    public Vector2 TargetPosition
    {
        get => _targetPosition;
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
        _mainCamera = Camera.main;

        _stateMachine = new PlayerStateMachine();

        _stateMachine.AddState(new IdleState(this, _stateMachine));
        _stateMachine.AddState(new MoveState(this, _stateMachine));
        _stateMachine.AddState(new FishState(this, _stateMachine));
        _stateMachine.AddState(new CookState(this, _stateMachine));

        _stateMachine.Initialize(typeof(IdleState), this);

        InputReader.OnMoveDownEvent += (pos) =>
        {
            var worldPos = _mainCamera.ScreenToWorldPoint(pos);
            if (worldPos.x < -boatSize.x + boatCenter.x - 1 || worldPos.x > boatSize.x + boatCenter.x + 1 ||
                worldPos.y < -boatSize.y + boatCenter.y - 1 || worldPos.y > boatSize.y + boatCenter.y + 1) return;
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boatCenter + (Vector2)shipTransform.position, boatSize * 2);
    }
#endif
}