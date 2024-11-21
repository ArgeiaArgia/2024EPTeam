using UnityEngine;

public class MoveState : PlayerState
{
    private bool _isInside = false;

    public MoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.OnMouseDownEvent += HandleMouseDownEvent;
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
    }

    private void HandleMouseDownEvent(Vector2 obj)
    {
        Player.TargetPosition = obj;
    }

    private void Move()
    {
        var isTargetTypeCook = StateMachine.TargetState.GetType() == typeof(CookState);
        if (isTargetTypeCook && !_isInside || !isTargetTypeCook && _isInside)
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