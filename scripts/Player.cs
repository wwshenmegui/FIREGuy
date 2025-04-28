using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 400.0f;
	
	private AnimatedSprite2D _animatedSprite;
	private string _lastDirection = "down"; // Track last direction for idle animations
	
	public override void _Ready()
	{
		// Get reference to the AnimatedSprite2D node
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Vector2.Zero;
		
		// Get input direction
		if (Input.IsActionPressed("move_right"))
			velocity.X += 1;
		if (Input.IsActionPressed("move_left"))
			velocity.X -= 1;
		if (Input.IsActionPressed("move_down"))
			velocity.Y += 1;
		if (Input.IsActionPressed("move_up"))
			velocity.Y -= 1;
		
		// Handle movement and animations
		if (velocity.Length() > 0)
		{
			// Normalize velocity for consistent speed
			velocity = velocity.Normalized() * Speed;
			
			// Set animation based on direction
			if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
			{
				// Horizontal movement is dominant
				if (velocity.X > 0)
				{
					_animatedSprite.Play("walk_right");
					_animatedSprite.FlipH = false;
					_lastDirection = "right";
				}
				else
				{
					// Use right animation but flipped horizontally
					_animatedSprite.Play("walk_right");
					_animatedSprite.FlipH = true;
					_lastDirection = "left";
				}
			}
			else
			{
				// Vertical movement is dominant
				if (velocity.Y > 0)
				{
					_animatedSprite.Play("walk_down");
					_lastDirection = "down";
				}
				else
				{
					_animatedSprite.Play("walk_up");
					_lastDirection = "up";
				}
				// Reset horizontal flip when moving vertically
				_animatedSprite.FlipH = false;
			}
		}
		else
		{
			// No movement, play idle animation based on last direction
			switch (_lastDirection)
			{
				case "up":
					_animatedSprite.Play("idle_up");
					_animatedSprite.FlipH = false;
					break;
				case "down":
					_animatedSprite.Play("idle_down");
					_animatedSprite.FlipH = false;
					break;
				case "right":
					_animatedSprite.Play("idle_right");
					_animatedSprite.FlipH = false;
					break;
				case "left":
					// Use right idle animation but flipped horizontally
					_animatedSprite.Play("idle_right");
					_animatedSprite.FlipH = true;
					break;
			}
		}
		
		// Move the character
		Velocity = velocity;
		MoveAndSlide();
	}
}
