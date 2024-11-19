using UnityEngine;

public class MoveState : PlayerState
{
    public MoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }
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

    private void HandleMouseDownEvent(Vector2 obj) { Player.TargetPosition = obj; }
    
    private void Move()
    {
        if (Player.TargetPosition.x - Player.transform.position.x < 0.5f)
        {
            Player.RigidbodyComponent.velocity = Vector2.zero;
            StateMachine.ChangeToTargetState();
            return;
        }
        Player.RigidbodyComponent.velocity = (Player.TargetPosition - (Vector2)Player.transform.position).normalized * Player.playerSpeed;
    }
}
