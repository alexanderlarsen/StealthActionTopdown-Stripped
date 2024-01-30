using StealthTD.Audio;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Zenject;

namespace StealthTD.UI.MainMenu
{
	public class MenuSceneController : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly TitleScreenView titleScreenView;

		[Inject]
		private readonly MainMenuView mainMenuView;

		[Inject]
		private readonly LevelSelectorView levelSelectorView;

		[Inject]
		private readonly MenuSceneModel model;

		[Inject]
		private readonly MenuAudioManager audioManager;

		[Inject]
		private readonly CursorController cursor;

		private readonly List<LevelSelectorButtonView> levelSelectorButtonViews = new();

		[SerializeField]
		private LevelDataModel[] levelDatas;

		#endregion Private Fields

		#region Private Methods

		private void Awake()
		{
			model.PropertyChanged += Model_PropertyChanged;
			mainMenuView.StartGameButton.onClick.AddListener(MainMenuView_StartGameButton_OnClick);
			mainMenuView.ContinueGameButton.onClick.AddListener(MainMenuView_ContinueGameButton_OnClick);
			mainMenuView.QuitGameButton.onClick.AddListener(MainMenuView_QuitGameButton_OnClick);
			levelSelectorView.BackButton.onClick.AddListener(LevelSelectorView_BackButton_OnClick);
			InitializeLevelSelectorButtons();
		}

		private void OnDestroy()
		{
			model.PropertyChanged -= Model_PropertyChanged;
			mainMenuView.StartGameButton.onClick.RemoveListener(MainMenuView_StartGameButton_OnClick);
			mainMenuView.ContinueGameButton.onClick.RemoveListener(MainMenuView_ContinueGameButton_OnClick);
			mainMenuView.QuitGameButton.onClick.RemoveListener(MainMenuView_QuitGameButton_OnClick);
			levelSelectorView.BackButton.onClick.RemoveListener(LevelSelectorView_BackButton_OnClick);
			levelSelectorButtonViews.ForEach(view => view.Button.onClick.RemoveAllListeners());
		}

		private IEnumerator Start()
		{
			cursor.SetArrowPointer();
			model.CurrentView = MenuView.TitleScreen;
			yield return new WaitForSeconds(0.5f);
			InputSystem.onAnyButtonPress.CallOnce((_) => LeaveTitleScreen());
		}

		private void MainMenuView_StartGameButton_OnClick()
		{
			audioManager.PlayButtonPressAudio();
			model.LoadLevel(1);
		}

		private void MainMenuView_ContinueGameButton_OnClick()
		{
			audioManager.PlayButtonPressAudio();
			model.CurrentView = MenuView.LevelSelector;
		}

		private void MainMenuView_QuitGameButton_OnClick()
		{
			audioManager.PlayButtonPressAudio();
			Application.Quit();
		}

		private void LevelSelectorView_BackButton_OnClick()
		{
			audioManager.PlayButtonPressAudio();
			model.CurrentView = MenuView.MainMenu;
		}

		private void LeaveTitleScreen()
		{
			audioManager.PlayLeaveTitleScreenAudio();
			model.CurrentView = MenuView.MainMenu;
		}

		private void InitializeLevelSelectorButtons()
		{
			foreach (LevelDataModel levelData in levelDatas)
			{
				LevelSelectorButtonView buttonView = levelSelectorView.CreateButton();
				levelSelectorButtonViews.Add(buttonView);
				bool isLocked = model.IsLevelLocked(levelData.LevelIndex);
				buttonView.UpdateView(levelData.LevelName, isLocked, levelData.ThumbnailSprite);

				buttonView.Button.onClick.AddListener(() =>
				{
					if (isLocked)
						audioManager.PlayInvalidButtonPressAudio();
					else
					{
						audioManager.PlayButtonPressAudio();
						model.LoadLevel(levelData.LevelIndex);
					}
				});
			}
		}

		private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(model.CurrentView))
			{
				titleScreenView.UpdateView(model.CurrentView);
				mainMenuView.UpdateView(model.CurrentView, model.CompletedLevelsCount, Application.version);
				levelSelectorView.UpdateView(model.CurrentView);
			}
		}

		#endregion Private Methods
	}
}