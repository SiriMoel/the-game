using Godot;
using System;

public enum FaceDirection{ UP, DOWN, LEFT, RIGHT };

public partial class Player : CharacterBody2D
{
	public int Speed = 150;
	public new Vector2 Velocity = new();
	public FaceDirection facing = FaceDirection.DOWN;
	private float Friction = 0.5f;
	public override void _PhysicsProcess(double delta)
	{
		AnimatedSprite2D Animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (Input.IsActionPressed("left")) {
			Velocity.X = -Speed;
			facing = FaceDirection.LEFT;
			Animation.Play("left_move");
		}
		if (Input.IsActionPressed("right")) {
			Velocity.X = Speed;
			facing = FaceDirection.RIGHT;
			Animation.Play("right_move");
		}
		if (Input.IsActionPressed("up")) {
			Velocity.Y = -Speed;
			facing = FaceDirection.UP;
			Animation.Play("up_move");
		}
		if (Input.IsActionPressed("down")) {
			Velocity.Y = Speed;
			facing = FaceDirection.DOWN;
			Animation.Play("down_move");
		}
		if (facing == FaceDirection.LEFT && !Input.IsActionPressed("left")) {
			Animation.Play("left_idle");
		}
		if (facing == FaceDirection.RIGHT && !Input.IsActionPressed("right")) {
			Animation.Play("right_idle");
		}
		if (facing == FaceDirection.UP && !Input.IsActionPressed("up"))  {
			Animation.Play("up_idle");
		}
		if (facing == FaceDirection.DOWN && !Input.IsActionPressed("down")) {
			Animation.Play("down_idle");
		}

		Velocity.X = Mathf.Lerp(Velocity.X, 0, Friction);
		Velocity.Y = Mathf.Lerp(Velocity.Y, 0, Friction);
		if (Mathf.Abs(Velocity.X) < 1)
			Velocity.X = 0f;
		if (Mathf.Abs(Velocity.Y) < 1)
			Velocity.Y = 0f;
		KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null) {
		} else {
		}
	}
}

/*

w up
s down
a left
d right
z cycle equipped item (of the 2 slots)
e quick heal

j attack
k interact (open chest, etc)
i inventory
h special attack? (some weapons have 2nd attack, otherwise use vine attack)

SPACE dash/roll thing

ESC pause menu

*/
