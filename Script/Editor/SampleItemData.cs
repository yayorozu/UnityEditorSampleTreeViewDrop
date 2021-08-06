using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class SampleItemData : ScriptableSingleton<SampleItemData>
{
	[SerializeField]
	private List<ItemData> _data;

	private static IEnumerable<ItemData> Data
	{
		get
		{
			if (instance._data == null)
				Reset();

			return instance._data;
		}
	}

	/// <summary>
	/// データのリセット
	/// </summary>
	public static void Reset()
	{
		instance._data = new List<ItemData>();
		for (var i = 0; i < 10; i++)
		{
			instance._data.Add(new ItemData(i, i));
		}
	}

	/// <summary>
	/// 親やインデックスの差し替え
	/// </summary>
	public static void ChangeParent(int id, int parentId, int? changeIndex = null)
	{
		instance._data[id].ParentID = parentId;
		if (changeIndex.HasValue)
		{
			var changeIndexData = instance._data
				.Where(d => d.ParentID == parentId)
				.Where(d => d.Index >= changeIndex.Value);

			foreach (var data in changeIndexData)
			{
				data.Index++;
			}
			instance._data[id].Index = changeIndex.Value;
		}
	}

	/// <summary>
	/// 子供をセットしていく
	/// </summary>
	public static void SetChild(TreeViewItem root)
	{
		var rows = new List<TreeViewItem>(200);
		foreach (var data in Data)
		{
			var item = new TreeViewItem(data.ID, 0, data.ID.ToString());
			rows.Add(item);
		}

		// Root の設定
		var rootData = Data
			.Where(d => d.ParentID == -1)
			.OrderBy(d => d.Index);
		foreach (var data in rootData)
			root.AddChild(rows[data.ID]);

		// 子供をセット
		var childData = Data
			.Where(d => d.ParentID != -1)
			.GroupBy(d => d.ParentID);
		foreach (var dic in childData)
		{
			foreach (var data in dic.OrderBy(d => d.Index))
				rows[dic.Key].AddChild(rows[data.ID]);
		}
	}

}
