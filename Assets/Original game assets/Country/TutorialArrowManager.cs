using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrowManager : MonoBehaviour
{


   public static IEnumerator StartShowingTutorialArrowsBasedOnState()
	{
		List<Need> signsToShowTHeirArrows = new List<Need>();
		foreach (Need need in FindObjectsOfType(typeof(Need),true))
		{
			signsToShowTHeirArrows.Add(need);
		}

		if (MouseExchangeManager.currentExchangeState == ExchangeMakingState.LookingToStart)
		{
			HighlightAll();
		}
		else if (MouseExchangeManager.currentExchangeState == ExchangeMakingState.LookingToEnd)
		{


			HighlightSome();
		}

		ExchangeMakingState previousExchangeState = MouseExchangeManager.currentExchangeState;

		while (true)
		{
			
			if (MouseExchangeManager.currentExchangeState != previousExchangeState)
			{
				switch(MouseExchangeManager.currentExchangeState)
				{
					case ExchangeMakingState.LookingToStart:
						HighlightAll();
							break;
					case ExchangeMakingState.LookingToEnd:
						StopAllHighlight();
						HighlightSome();
						break;
				}
				previousExchangeState = MouseExchangeManager.currentExchangeState;
			}


			
			if (GameMaker.numberofExchangesCompleted >= 2)
			{
				StopAllHighlight();
				break;
			}
			yield return null;
		}



		void HighlightAll()
		{
			foreach (Need signToChange in signsToShowTHeirArrows)
			{
				signToChange.gameObject.GetComponent<SignHighLighter>().StartArrowHighlight();
			}
		}
		void HighlightSome()
		{
			Needs needToHiglight = MouseExchangeManager.informationCurrentExchange.typeOfExchange;
			bool signToHighlightIsMoreThan100 = MouseExchangeManager.informationCurrentExchange.otherToCompletExchangeIsMoreTHan100;
			foreach (Need signToChange in signsToShowTHeirArrows)
			{

				if (signToChange.sighnNeedType == needToHiglight && (signToHighlightIsMoreThan100 ? signToChange.currentSignValue > 100 : signToChange.currentSignValue < 100) && signToChange.isMainSign == false)
				{
					signToChange.gameObject.GetComponent<SignHighLighter>().StartArrowHighlight();
					signToChange.ourCountryParent.ourmainSign.gameObject.GetComponent<SignHighLighter>().StartArrowHighlight();
				}

			}
		}
		void StopAllHighlight()
		{
			foreach (Need signToChange in signsToShowTHeirArrows)
			{
				signToChange.gameObject.GetComponent<SignHighLighter>().StopArrowHighlight();
			}
		}
	}
}
