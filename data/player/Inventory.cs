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
