using UnityEngine;

public class MoveState : PlayerState
{
    private bool _isInside = false;

    public MoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        defaultAnimationHash = Animator.StringToHash("Walk");
    }

    public override void Enter()
    {
        base.Enter();
        Player.OnMouseDownEvent += HandleMouseDownEvent;
        Player.InputReader.OnEscapeEvent += StateMachine.ResetToIdleState;
    }


    public override void Update()
    {
        base.Update();
        Move();
    }

    public override void Exit()
    {
        base.Exit();
        Player.OnMouseDownEvent -= HandleMouseDownEvent;
        Player.InputReader.OnEscapeEvent -= StateMachine.ResetToIdleState;
    }

    private void HandleMouseDownEvent(Vector2 obj)
    {
        Player.TargetPosition = obj;
    }

    private void Move()
    {
        var isTargetTypeInside = StateMachine.TargetState.GetType() == typeof(CookState) || StateMachine.TargetState.GetType() == typeof(SleepState);
        if (isTargetTypeInside && !_isInside || !isTargetTypeInside && _isInside)
        {
            if (Mathf.Abs(Player.DoorPos.x - Player.transform.position.x) < 0.25f)
            {
                Player.RigidbodyComponent.velocity = Vector2.zero;
                _isInside = !_isInside;
                Player.SpriteRendererComponent.sortingOrder = _isInside ? 0 : 10;
                return;
            }
            Player.RigidbodyComponent.velocity = (Player.DoorPos - (Vector2)Player.transform.position).normalized
                                                 * Player.playerSpeed;
        }
        else
        {
            if (Mathf.Abs(Player.TargetPosition.x - Player.transform.position.x) < 0.25f)
            {
                Player.RigidbodyComponent.velocity = Vector2.zero;
                StateMachine.ChangeToTargetState();
                return;
            }

            Player.RigidbodyComponent.velocity = (Player.TargetPosition - (Vector2)Player.transform.position).normalized
                                                 * Player.playerSpeed;
        }
    }
}