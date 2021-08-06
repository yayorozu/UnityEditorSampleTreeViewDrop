using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

public class TestTreeView : TreeView
{
	private TreeViewEditorWindow _window;
	private static readonly string DataKey = "TestTreeView";

	public TestTreeView(TreeViewEditorWindow window, TreeViewState state) : base(state)
	{
		_window = window;
		showBorder = true;
		showAlternatingRowBackgrounds = true;
		Reload();
	}

	protected override TreeViewItem BuildRoot()
	{
		var root = new TreeViewItem {id = -1, depth = -1};

		SampleItemData.SetChild(root);

		SetupDepthsFromParentsAndChildren(root);

		return root;
	}

	protected override bool CanMultiSelect(TreeViewItem item) => false;

	/// <summary>
	/// 入れ替え時親となれるか
	/// </summary>
	protected override bool CanBeParent(TreeViewItem item) => true;

	/// <summary>
	/// 要素を移動できるか
	/// </summary>
	protected override bool CanStartDrag(CanStartDragArgs args) => true;

	/// <summary>
	/// ドラッグ開始時の処理
	/// </summary>
	protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
	{
		var selections = args.draggedItemIDs;
		if (selections.Count <= 0)
			return;

		var dragObjects = GetRows()
			.Where(i => selections.Contains(i.id))
			.ToArray()
			;

		if (dragObjects.Length <= 0)
			return;

		DragAndDrop.PrepareStartDrag();
		DragAndDrop.SetGenericData(DataKey, dragObjects);
		DragAndDrop.StartDrag(dragObjects.Length > 1 ? "<Multiple>" : dragObjects[0].displayName);
	}

	/// <summary>
	/// ドラッグ＆ドロップ時の処理
	/// </summary>
	protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
	{
		// ドロップ時の処理
		if (args.performDrop)
		{
			var data =  DragAndDrop.GetGenericData(DataKey);
			var items = data as TreeViewItem[];

			if (items == null || items.Length <= 0)
				return DragAndDropVisualMode.None;

			var item = items.First();
			switch (args.dragAndDropPosition)
			{
				case DragAndDropPosition.UponItem:
					SampleItemData.ChangeParent(item.id, args.parentItem.id);
					SetSelection(new List<int>{item.id});
					Reload();
					break;
				case DragAndDropPosition.BetweenItems:
					SampleItemData.ChangeParent(item.id, args.parentItem.id, args.insertAtIndex);
					SetSelection(new List<int>{item.id});
					Reload();
					break;

				case DragAndDropPosition.OutsideItems:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		else if (isDragging)
		{
			// ドラッグ中の処理
		}

		return DragAndDropVisualMode.Move;
	}
}
