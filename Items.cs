using Godot;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
public static partial class Items
{
	public enum ItemType { RESOURCE, WEAPON, TOOL, EQUIPPABLE, CONSUMABLE, PATTERN };
	public static List<ItemData> LoadedItems = new();
	private static JsonSerializerOptions jsonConfig = new JsonSerializerOptions() {IncludeFields = true, Converters = {new JsonStringEnumConverter()}};
	public static void LoadItems(string path)
	{
		string jsonString = (FileAccess.Open(path, FileAccess.ModeFlags.Read).GetAsText());
		LoadedItems.AddRange(System.Text.Json.JsonSerializer.Deserialize<List<ItemData>>(jsonString, new JsonSerializerOptions() {IncludeFields = true}));
	}


	public static ItemData GetItem(string itemID) {
		foreach (ItemData item in LoadedItems)
		{
			if (item.Id == itemID) {
				return item;
			}
		}
		return null;
	}
	public class ItemData
	{
		public readonly string Id;
		public readonly string Icon;
		public readonly ItemType Type;
		public readonly string Name;
		public readonly string Desc;
		public readonly string InstancePath;

		[JsonConstructor]
		public ItemData(string id, string icon, ItemType type, string name, string desc) {
			Id = id;
			Icon = icon;
			Type = type;
			Name = name;
			Desc = desc;
		}

		public ItemData(string id) {
			ItemData i = null;
			foreach (var item in LoadedItems)
			{
				if (item.Id == id) {
					i = item;
				}
			}
			Id = i.Id;
			Icon = i.Icon;
			Type = i.Type;
			Name = i.Name;
			Desc = i.Desc;
		}
	}

	public abstract partial class Item: Node2D {
		public abstract string Id {get;}
		public abstract string Icon {get;}
		public abstract string ItemName {get;}
		public abstract string Desc {get;}
		public abstract ItemType ItemType {get;}
	}

	public abstract partial class Resource: Item {
		public ItemType Type = ItemType.RESOURCE;
		public int Amount = 1;
	}

	public abstract partial class Weapon: Item {
		public ItemType Type = ItemType.WEAPON;
		public abstract void PrimaryFire();
		public abstract void SecondaryFire();
	}

	public abstract partial class Tool: Item {
		public ItemType Type = ItemType.TOOL;
		public abstract void PrimaryUse();
	}

	public abstract partial class Equippable: Item {
		public ItemType Type = ItemType.EQUIPPABLE;
		public abstract void OnEquip();
		public abstract void OnUnequip();
	}

	public abstract partial class Consumable: Item {
		public ItemType Type = ItemType.CONSUMABLE;
		public abstract void Consume();
	}

	public abstract partial class Pattern: Item {
		public ItemType Type = ItemType.PATTERN;
		public abstract void Cast();
	}
}
