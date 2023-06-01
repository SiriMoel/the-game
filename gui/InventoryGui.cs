using Godot;
using System;

public partial class InventoryGui : Control
{
	public bool InventoryShown = false;
	public int ActiveSlot = 1;
	public Texture2D SelectedSlotTexture;
	public Texture2D SlotTexture;
	public override void _Ready()
	{
		Visible = false;
		SelectedSlotTexture = GetNode<TextureRect>($"Slots/Slot1").Texture;
		SlotTexture = GetNode<TextureRect>($"Slots/Slot2").Texture;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("inventory")) {	
			InventoryShown = !InventoryShown;
			Visible = !Visible;
			GetTree().Paused = !GetTree().Paused;
		}
		if (InventoryShown == true) {
			int NewSlot = ActiveSlot;
			if (Input.IsActionJustPressed("up")) {
				NewSlot -= 5;
			}
			if (Input.IsActionJustPressed("down")) {
				NewSlot += 5;
			}
			if (Input.IsActionJustPressed("left")) {
				NewSlot -= 1;
			}
			if (Input.IsActionJustPressed("right")) {
				NewSlot += 1;
			}
			if (NewSlot < 1) {
				NewSlot = ActiveSlot;
			}
			if (NewSlot > 20) {
				NewSlot = ActiveSlot;
			}

			if (NewSlot != ActiveSlot) {
				GetNode<TextureRect>($"Slots/Slot{NewSlot}").Texture = SelectedSlotTexture;
				GetNode<TextureRect>($"Slots/Slot{ActiveSlot}").Texture = SlotTexture;
				ActiveSlot = NewSlot;
			}
		}
	
	}}
