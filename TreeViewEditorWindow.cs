using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class TreeViewEditorWindow : EditorWindow
{
	[SerializeField]
	private TreeViewState _state;
	private TestTreeView _treeView;

	[MenuItem("TreeView Examples/Item Drop")]
	static void ShowWindow()
	{
		var window = GetWindow<TreeViewEditorWindow>("TreeView Item Drop");
		window.Show();
	}

	private void OnEnable()
	{
		_state ??= new TreeViewState();
		_treeView = new TestTreeView(this, _state);
	}

	private void OnGUI()
	{
		using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
		{
			if (GUILayout.Button("Reset", EditorStyles.toolbarButton))
			{
				SampleItemData.Reset();
				_treeView.Reload();
			}
			GUILayout.FlexibleSpace();
		}

		var rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
		_treeView.OnGUI(rect);
	}
}
