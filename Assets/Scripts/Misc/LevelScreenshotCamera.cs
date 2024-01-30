#if UNITY_EDITOR
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScreenshotCamera : MonoBehaviour
{
	#region Private Fields

	private const string folder = "Assets/Art/Sprites/LevelThumbnails";

	[SerializeField]
	private float delay = 0.1f;

	[SerializeField]
	private int height = 1080;

	[SerializeField]
	private int width = 1920;

	private Camera captureCamera;

	#endregion Private Fields

	#region Private Properties

	private string ImageName => SceneManager.GetActiveScene().name;
	private string FilePath => $"{folder}/{ImageName}.png";

	#endregion Private Properties

	#region Private Methods

	private IEnumerator Start()
	{
		if (!Application.isEditor)
		{
			Destroy(gameObject);
			yield break;
		}

		captureCamera = transform.GetComponent<Camera>();
		yield return new WaitForSeconds(delay);
		yield return CaptureAndSave();
	}

	private IEnumerator CaptureAndSave()
	{
		yield return new WaitForEndOfFrame();
		RenderTexture renderTexture = new(width, height, 24);
		RenderTexture currentRT = RenderTexture.active;
		RenderTexture.active = captureCamera.targetTexture = renderTexture;
		captureCamera.Render();
		Texture2D image = new(width, height, TextureFormat.RGB24, false);
		image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		image.Apply();
		captureCamera.targetTexture = null;
		RenderTexture.active = currentRT;
		byte[] bytes = image.EncodeToPNG();
		Destroy(image);
		Directory.CreateDirectory(folder);
		File.WriteAllBytes(FilePath, bytes);
		Debug.Log("Saved Image to: " + FilePath);
		AssetDatabase.Refresh();
		AssetDatabase.ImportAsset(FilePath);
		ModifyImportSettings(FilePath);
		EditorApplication.isPlaying = false;
	}

	private void ModifyImportSettings(string assetPath)
	{
		TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
		importer.textureType = TextureImporterType.Sprite;
		importer.maxTextureSize = 512;
		importer.SaveAndReimport();
	}

	#endregion Private Methods
} 
#endif