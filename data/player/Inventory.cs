using Godot;
using System;

public partial class Inventory : Control
{
	public Items.Item[] InvItems = new Items.Item[20];
	public void AddItem(string itemId, int? slot) {
		int insertInto = 0;
		for (int i = 0; i < InvItems.Length; i++)
		{
			if (InvItems[i] == null) {
				insertInto = i;
				break;
			}
		}
		insertInto = slot ?? insertInto;
		Items.ItemData it = Items.GetItem(itemId);
		PackedScene scene = (PackedScene)ResourceLoader.Load(it.InstancePath);
		Items.Item newItem = (Items.Item)scene.Instantiate();
		if (InvItems[insertInto] != null) {
			throw new IndexOutOfRangeException($"Slot {insertInto} already has an item in it.");
		}
		InvItems[insertInto] = newItem;
	}

	public void RemoveItem(int slot) {
		InvItems[slot].Free();
		InvItems[slot] = null;
	}

	public void SwitchSlots(int slot1, int slot2) {
		Items.Item i1 = InvItems[slot1];
		Items.Item i2 = InvItems[slot2];
		InvItems[slot1] = i2;
		InvItems[slot2] = i1;
	}

	public void EquipItem(int slot) {
		Items.Item i = InvItems[slot];
		GetParent().AddChild(i);
	}

	public bool InventoryShown = false;
	public int ActiveSlot = 1;

	public override void _Ready()
	{
		Visible = false;
		
	}

	public override void _Process(double delta)
	{
		TextureRect SelectedSlot = GetNode<TextureRect>("SelectedSlot");

		if (Input.IsActionJustPressed("inventory")) {	
			InventoryShown = !InventoryShown;
			Visible = !Visible;
			GetTree().Paused = !GetTree().Paused;
		}

		if (InventoryShown == true) {
			if (Input.IsActionJustPressed("up")) {
				ActiveSlot -= 5;
			}
			if (Input.IsActionJustPressed("down")) {
				ActiveSlot += 5;
			}
			if (Input.IsActionJustPressed("left")) {
				ActiveSlot -= 1;
			}
			if (Input.IsActionJustPressed("right")) {
				ActiveSlot += 1;
			}
			if (ActiveSlot < 1) {
				ActiveSlot = 1;
			}
			if (ActiveSlot > 20) {
				ActiveSlot = 20;
			}

			SelectedSlot.Position = GetNode<TextureRect>("Slot{ActiveSlot}").Position;
		}
	
	}

}

/*

X X X X X
X X X X X
X X X X X
X X X X X

01 02 03 04 05
06 07 08 09 10
11 12 13 14 15
16 17 18 19 20

*/
