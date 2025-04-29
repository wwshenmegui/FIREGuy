using Godot;
using System;

public partial class DialogManager : CanvasLayer
{
	private Label _dialogLabel;
	private Panel _dialogPanel;
	private bool _isActive = false;
	
	[Signal]
	public delegate void DialogOpenedEventHandler();
	
	[Signal]
	public delegate void DialogClosedEventHandler();
	
	public override void _Ready()
	{
		_dialogPanel = GetNode<Panel>("DialogPanel");
		_dialogLabel = GetNode<Label>("DialogPanel/DialogLabel");
		
		// Hide the dialog panel initially
		_dialogPanel.Visible = false;
	}
	
	public void ShowDialog(string text)
	{
		_dialogLabel.Text = text;
		_dialogPanel.Visible = true;
		_isActive = true;
		EmitSignal(SignalName.DialogOpened);
	}
	
	public void CloseDialog()
	{
		_dialogPanel.Visible = false;
		_isActive = false;
		EmitSignal(SignalName.DialogClosed);
	}
	
	public bool IsDialogActive()
	{
		return _isActive;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (_isActive && @event.IsActionPressed("interact"))
		{
			CloseDialog();
			GetViewport().SetInputAsHandled(); // Prevent the input from being processed by other nodes
		}
	}
}
