using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace StealthTD.ObjectPool
{
	public interface IPool
	{
	}

	public class ObjectPool<T> : IPool where T : Component
	{
		#region Private Fields

		private readonly bool isFlexible;
		private readonly ConcurrentQueue<GameObject> availableObjects = new();
		private readonly Dictionary<GameObject, T> componentCache = new();
		private readonly GameObject prefab;
		private readonly HashSet<GameObject> allObjects = new();
		private readonly int numberOfPreloadedObjects;
		private readonly int numMaxObjects;
		private readonly List<GameObject> unavailableObjects = new();
		private readonly Transform defaultParent;

		#endregion Private Fields

		#region Public Constructors

		public ObjectPool(GameObject prefab, bool isFlexible, int numMaxObjects, int numberOfPreloadedObjects, Transform defaultParent)
		{
			this.prefab = prefab;
			this.isFlexible = isFlexible;
			this.numMaxObjects = numMaxObjects;
			this.defaultParent = defaultParent;
			this.numberOfPreloadedObjects = numberOfPreloadedObjects;

			if (numberOfPreloadedObjects > 0)
				PrepopulatePool();
		}

		#endregion Public Constructors

		#region Public Methods

		public T Get()
		{
			GameObject instance = GetGameObject();
			instance.transform.SetParent(defaultParent);
			instance.SetActive(true);
			return componentCache[instance];
		}

		public T Get(Vector3 position)
		{
			GameObject instance = GetGameObject();
			instance.transform.position = position;
			instance.transform.SetParent(defaultParent);
			instance.SetActive(true);
			return componentCache[instance];
		}

		public T Get(Vector3 position, Quaternion rotation, Transform parent = null)
		{
			GameObject instance = GetGameObject();
			instance.transform.SetPositionAndRotation(position, rotation);
			instance.transform.SetParent(parent != null ? parent : defaultParent);
			instance.SetActive(true);
			return componentCache[instance];
		}

		public void Return(T component)
		{
			Return(component.gameObject);
		}

		public void Return(GameObject gameObject)
		{
			if (!allObjects.Contains(gameObject))
			{
				throw new InvalidOperationException($"You are trying to return a GameObject '{gameObject.name}' to an ObjectPool<{typeof(T)}>.");
			}

			ResetObject(gameObject);
			unavailableObjects.Remove(gameObject);
			availableObjects.Enqueue(gameObject);
		}

		#endregion Public Methods

		#region Private Methods

		private void ReturnFirstUnavailable()
		{
			GameObject gameObject = unavailableObjects[0];
			ResetObject(gameObject);
			unavailableObjects.RemoveAt(0);
			availableObjects.Enqueue(gameObject);
		}

		private GameObject GetGameObject()
		{
			if (availableObjects.TryDequeue(out GameObject availableObject))
			{
				unavailableObjects.Add(availableObject);
				return availableObject;
			}
			else if (isFlexible)
				InstantiateNew();
			else
				ReturnFirstUnavailable();

			return GetGameObject();
		}

		private void PrepopulatePool()
		{
			int count = isFlexible ? numberOfPreloadedObjects : Mathf.Min(numMaxObjects, numberOfPreloadedObjects);

			for (int i = 0; i < count; i++)
			{
				GameObject instance = InstantiateNew();
				ResetObject(instance);
			}
		}

		private GameObject InstantiateNew()
		{
			GameObject newInstance = GetDiContainer().InstantiatePrefab(prefab, defaultParent);
			newInstance.SetActive(false);
			allObjects.Add(newInstance);

			if (newInstance.TryGetComponent(out T component))
				componentCache[newInstance] = component;

			availableObjects.Enqueue(newInstance);

			return newInstance;
		}

		private void ResetObject(GameObject gameObject)
		{
			gameObject.SetActive(false);
			gameObject.transform.SetParent(defaultParent);
			gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		}

		private DiContainer GetDiContainer()
		{
			return ProjectContext
				.Instance
				.Container
				.Resolve<SceneContextRegistry>()
				.GetContainerForScene(SceneManager.GetActiveScene());
		}

		#endregion Private Methods
	}
}