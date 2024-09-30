using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScreen : MonoBehaviour
{
    [HideInInspector]
    public UiChangeManager ourChangeManager;

	public void TellHeadUiButtonClicked()
	{
		//ourHeadTechnicalDrawer.ourheadManager.ForignOrderOpeningSCreenClosed();
		if (GameStateInformationProvider.NormalGameStart != null)
		{
			GameStateInformationProvider.NormalGameStart();                             //       ------------------------------------------------>>>
		}

		//GameStateInformationProvider.ScreenSizeChanged -= RepoSitionOpeningSign;
		//	GameStateInformationProvider.GameStarted -= OnGameStart;
		//Destroy(gameObject);
	}
}
