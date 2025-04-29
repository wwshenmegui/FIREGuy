using Godot;
using System;

public partial class Interactable : Area2D
{
	[Export]
	public string DialogText { get; set; } = "This is an interactable object.";
	
	[Export]
	public float InteractionRadius { get; set; } = 80.0f; // Distance for interaction
	
	private DialogManager _dialogManager;
	private Label _interactHintLabel;
	private bool _playerInRange = false;
	
	public override void _Ready()
	{
		// Connect the body entered signal
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
		
		// Get reference to the dialog manager (autoloaded)
		_dialogManager = GetNode<DialogManager>("/root/DialogManager");
		
		// Get reference to the interact hint label
		_interactHintLabel = GetNode<Label>("InteractHint");
		
		// Hide the hint initially
		if (_interactHintLabel != null)
		{
			_interactHintLabel.Visible = false;
		}
		
		// Set the collision shape to match the interaction radius
		CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		if (collisionShape != null && collisionShape.Shape is CircleShape2D circleShape)
		{
			circleShape.Radius = InteractionRadius;
		}
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			_playerInRange = true;
			
			// Show the interaction hint
			if (_interactHintLabel != null)
			{
				_interactHintLabel.Visible = true;
			}
		}
	}
	
	private void OnBodyExited(Node2D body)
	{
		if (body is Player)
		{
			_playerInRange = false;
			
			// Hide the interaction hint
			if (_interactHintLabel != null)
			{
				_interactHintLabel.Visible = false;
			}
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		// Check if the interact button is pressed while player is in range
		if (_playerInRange && @event.IsActionPressed("interact") && !_dialogManager.IsDialogActive())
		{
			_dialogManager.ShowDialog(DialogText);
			GetViewport().SetInputAsHandled(); // Prevent the input from being processed by other nodes
		}
	}
}
