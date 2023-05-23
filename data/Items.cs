using Godot;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public static partial class Items
{
	public enum ItemType { RESOURCE, WEAPON, TOOL, EQUIPABLE, CONSUMABLE, PATTERN };
	public static System.Collections.Generic.List<ItemData> LoadedItems = new();
	private static JsonSerializerOptions jsonConfig = new JsonSerializerOptions() {IncludeFields = true, Converters = {new JsonStringEnumConverter()}};
	public static void LoadItems(string path)
	{
		string jsonString = (FileAccess.Open(path, FileAccess.ModeFlags.Read).GetAsText());
		LoadedItems.AddRange(JsonSerializer.Deserialize<System.Collections.Generic.List<ItemData>>(jsonString, jsonConfig));
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

	public partial class Item: Node2D {
		public ItemData Data;
		public Item(string id) {
			Data = new ItemData(id);
		}
	}
}
