using Godot;
using System;

public partial class Inventory : Node2D
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

}
