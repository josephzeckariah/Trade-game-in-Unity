using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialArrowManager : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	///////////////////////////////////////////////////////////// Auto connections
	TutorialArowOnScreenEdgeDrawer ourTutorialArowOnScreenEdgeDrawer;


	///////////////////////////////////////////////////////////// Memory
	List<Need> signsToShowTHeirArrows = new List<Need>();
	List<Need> signsThatAreCurrentlyHighlighted = new List<Need>();



	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//I///////////////////////////////////////////////////////////     Initalize       /////////////////////////////////////////////////////////////
	private void Awake()
	{
		ourTutorialArowOnScreenEdgeDrawer = GetComponentInChildren<TutorialArowOnScreenEdgeDrawer>();

		GameStateInformationProvider.AnyGameStart += OnGameStarted;
		GameStateInformationProvider.GameEnded += OnGameEnded;
	}



	//S///////////////////////////////////////////////////////////     OnGameStarted       /////////////////////////////////////////////////////////////
	private void OnGameStarted()
	{
		foreach (Need need in FindObjectsOfType(typeof(Need), true))
		{
			signsToShowTHeirArrows.Add(need);
		}
	}


	//S///////////////////////////////////////////////////////////     OnGameEnded      /////////////////////////////////////////////////////////////

	private void OnGameEnded()
	{
		signsToShowTHeirArrows.Clear();
		signsThatAreCurrentlyHighlighted.Clear();
	}







	//OA///////////////////////////////////////////////////////////     Ocational actions      /////////////////////////////////////////////////////////////
	public void HighlightAll()
	{
		foreach (Need signToChange in signsToShowTHeirArrows)
		{
			if(signToChange.currentSignValue != 100f && signToChange.isMainSign== false)
			{
				StartHighlightSign(signToChange);
				StartHighlightSign(signToChange.ourCountryParent.ourmainSign);
			}
			
		}
	}
	public void HighlightSome()
	{
		Needs needToHiglight = GameStateInformationProvider.informationCurrentExchange.typeOfExchange;
		bool signToHighlightIsMoreThan100 = GameStateInformationProvider.informationCurrentExchange.otherToCompletExchangeIsMoreTHan100;
		foreach (Need signToChange in signsToShowTHeirArrows)
		{

			if (signToChange.sighnNeedType == needToHiglight && (signToHighlightIsMoreThan100 ? signToChange.currentSignValue > 100 : signToChange.currentSignValue < 100) && signToChange.isMainSign == false)
			{
				StartHighlightSign(signToChange);
				StartHighlightSign(signToChange.ourCountryParent.ourmainSign);
			}

		}
	}
	public void StopAllHighlight()
	{
		foreach (Need signToChange in signsToShowTHeirArrows)
		{
			StopHighlightSign(signToChange);
		}
	}



	//S///////////////////////////////////////////////////////////     Sub highlight sign action       /////////////////////////////////////////////////////////////

	void StartHighlightSign(Need NeedToStartHIghlight)
	{
		NeedToStartHIghlight.ArrowForHighlighting.SetActive(true);
		if (!signsThatAreCurrentlyHighlighted.Contains(NeedToStartHIghlight))
		{
			signsThatAreCurrentlyHighlighted.Add(NeedToStartHIghlight);

			ListBeenChanged(signsThatAreCurrentlyHighlighted);
		}
		
	}


	void StopHighlightSign(Need NeedToStopHIghlight)
	{
		NeedToStopHIghlight.ArrowForHighlighting.SetActive(false);
		if (signsThatAreCurrentlyHighlighted.Contains(NeedToStopHIghlight))
		{
			signsThatAreCurrentlyHighlighted.Remove(NeedToStopHIghlight);

			ListBeenChanged(signsThatAreCurrentlyHighlighted);
		}

	}


	//S///////////////////////////////////////////////////////////     SubDown worker       /////////////////////////////////////////////////////////////

	void ListBeenChanged(List<Need> newListOfHighlightSigns)
	{
		ourTutorialArowOnScreenEdgeDrawer.HigherUpMessageListBeenUpdated(newListOfHighlightSigns);
	}

}
