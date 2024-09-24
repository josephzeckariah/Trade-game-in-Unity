using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrowManager : MonoBehaviour
{
	List<Need> signsToShowTHeirArrows = new List<Need>();
	private void Start()
	{
		
		foreach (Need need in FindObjectsOfType(typeof(Need), true))
		{
			signsToShowTHeirArrows.Add(need);
		}
	}


	public void HighlightAll()
	{
		foreach (Need signToChange in signsToShowTHeirArrows)
		{
			signToChange.gameObject.GetComponent<SignHighLighter>().StartArrowHighlight();
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
				signToChange.gameObject.GetComponent<SignHighLighter>().StartArrowHighlight();
				signToChange.ourCountryParent.ourmainSign.gameObject.GetComponent<SignHighLighter>().StartArrowHighlight();
			}

		}
	}
	public void StopAllHighlight()
	{
		foreach (Need signToChange in signsToShowTHeirArrows)
		{
			signToChange.gameObject.GetComponent<SignHighLighter>().StopArrowHighlight();
		}
	}
}
