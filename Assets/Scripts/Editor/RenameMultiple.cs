using UnityEditor;
using UnityEngine;

namespace StealthTD.Editor
{
	public class RenameMultiple : EditorWindow
	{
		#region Private Fields

		private static RenameMultiple instance;
		private string baseName = "GameObject";

		#endregion Private Fields

		#region Private Methods

		// Add menu item named "Rename Multiple" to the GameObject menu
		[MenuItem("GameObject/Rename Multiple", false, -1)]
		private static void Init()
		{
			// If the window is already open, just focus on it
			if (instance != null)
			{
				instance.Focus();
			}
			else
			{
				// Create a new window
				instance = CreateInstance<RenameMultiple>();
				instance.baseName = "GameObject"; // Default name
				instance.ShowUtility(); // Show as a utility window
			}
		}

		// Validate the menu item
		[MenuItem("GameObject/Rename Multiple", true)]
		private static bool ValidateInit()
		{
			// The menu item will be disabled if less than two GameObjects are selected
			return Selection.gameObjects != null && Selection.gameObjects.Length > 1;
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Enter new base name for selected GameObjects:", EditorStyles.wordWrappedLabel);
			baseName = EditorGUILayout.TextField("Base Name", baseName);

			if (GUILayout.Button("Rename"))
			{
				RenameSelectedGameObjects();
			}
		}

		private void RenameSelectedGameObjects()
		{
			if (Selection.gameObjects.Length > 0)
			{
				foreach (GameObject obj in Selection.gameObjects)
				{
					Undo.RecordObject(obj, "Rename Selected GameObjects");
					obj.name = $"{baseName} ({obj.transform.GetSiblingIndex() + 1})";
					EditorUtility.SetDirty(obj);
				}
			}
			else
			{
				EditorUtility.DisplayDialog("No GameObject selected", "Please select at least one GameObject in the Hierarchy to rename.", "OK");
			}
		}

		private void OnLostFocus()
		{
			Close();
		}

		private void OnDestroy()
		{
			instance = null;
		}

		#endregion Private Methods
	}
}