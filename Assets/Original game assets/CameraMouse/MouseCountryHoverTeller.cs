using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCountryHoverTeller : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	GameMaker ourGameMaker;

	List<Country> countriesToCheckIfEntered = new List<Country>();
	List<Country> countriesToCheckIfExited = new List<Country>();

	Coroutine ourHoverTellingCycle;
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//I///////////////////////////////////////////////////////////     Initalize       /////////////////////////////////////////////////////////////
	private void Awake()
	{
		GameStateInformationProvider.AnyGameStart += StartTellingHover;
		GameStateInformationProvider.AllSigns100 += StopTellingHover;
	}

	//S///////////////////////////////////////////////////////////     Start       /////////////////////////////////////////////////////////////
	void StartTellingHover()
	{
		SubAwakeCopyCountryListFromBigBossGameMaker();                        //         <<<--------------------------------------------------------------


		ourHoverTellingCycle = StartCoroutine(ContinousCheckForMouseHover());
	}
	/////////////////////////////////////////////////////////////
	
	void StopTellingHover()
	{
		StopCoroutine(ourHoverTellingCycle);
	}

	//S///////////////////////////////////////////////////////////     Know countries to work with       /////////////////////////////////////////////////////////////
	void SubAwakeCopyCountryListFromBigBossGameMaker()
	{
		ourGameMaker = GameObject.FindAnyObjectByType<GameMaker>();

		countriesToCheckIfEntered.Clear();
		countriesToCheckIfExited.Clear();
		foreach (Country countryToADd in ourGameMaker.countriesLoadedAccordingToGameMode) //////////////////////////////
		{
			countriesToCheckIfEntered.Add(countryToADd);
		}
	}


	//S///////////////////////////////////////////////////////////     Continous Tell if hovered      /////////////////////////////////////////////////////////////
	                IEnumerator ContinousCheckForMouseHover()
	{
		yield return null;  //a wait to avoid glitches.


		while (true)
		{
			Vector3 mousePositionInWOrldSPace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 mousePositionInWOrldSPaceCorrected = new Vector3(mousePositionInWOrldSPace.x, mousePositionInWOrldSPace.y, 0);


			UpdateContinousCheckCountriesIfMouseEntered(mousePositionInWOrldSPaceCorrected);
			UpdateContinousCheckCountriesIfMouseExited(mousePositionInWOrldSPaceCorrected);

			yield return null;
		}
	
	}


	                            void UpdateContinousCheckCountriesIfMouseEntered(Vector3 mousePositionOnMap)
	{
		List<Country> countriesEntered = new List<Country>();
		foreach (Country countryToCheck in countriesToCheckIfEntered)
		{
			if (Vector3.Distance(countryToCheck.transform.position, mousePositionOnMap) < 2)
			{
				countryToCheck.ForignOrderMouseEnterHoverOverCountry();
				countriesEntered.Add(countryToCheck);

			}
		}
		foreach (Country countryEnterd in countriesEntered)
		{
			countriesToCheckIfEntered.Remove(countryEnterd);
			countriesToCheckIfExited.Add(countryEnterd);
		}
	}	
	                            void UpdateContinousCheckCountriesIfMouseExited(Vector3 mousePositionOnMap)
	{
		List<Country> countriesExited = new List<Country>();
		foreach (Country countryToCheck in countriesToCheckIfExited)
		{
			if (Vector3.Distance(countryToCheck.transform.position, mousePositionOnMap) > 6)
			{
				countryToCheck.ForignOrderMouseExitedHoverOverCountry();
				countriesExited.Add(countryToCheck);

			}
		}
		foreach (Country countryEnterd in countriesExited)
		{
			countriesToCheckIfExited.Remove(countryEnterd);
			countriesToCheckIfEntered.Add(countryEnterd);
		}
	}
}
