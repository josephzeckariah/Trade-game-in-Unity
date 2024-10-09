using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum Needs { Food,Materials,Electronics,Machines };



public  class GameMaker : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[Header("Manual properits")]
	public List<Needs> needsUsedInGame ;
	public List<Country> countriesUsedInMainGame;
	public List<Country> countriesUsedInNewGamePlus;


	[HideInInspector]
	public List<Country> countriesLoadedAccordingToGameMode;



	//auto connection
	NeedValueAssighnerWorker ourNeedValueAssighnerWorker;
	SignShowingManager ourSignManager;
	GeneralInformationProvider ourGeneralInformationMaker;


	public static bool tutorialIsOn = false;

	public static int numberofExchangesCompleted = 0;


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//I///////////////////////////////////////////////////////////     Initalize       /////////////////////////////////////////////////////////////
	void Awake()
	{
		ourNeedValueAssighnerWorker = this.GetComponentInChildren<NeedValueAssighnerWorker>();
		ourSignManager = this.GetComponentInChildren<SignShowingManager>();
		ourGeneralInformationMaker = this.GetComponentInChildren<GeneralInformationProvider>();
		ourGeneralInformationMaker.AssignFacts();


		GameStateInformationProvider.NormalGameStart += UniqueReactionToStartingANormalGame;
		GameStateInformationProvider.NewGamePlusStart += UniqueReactionToStartingANewGamePLus;

		GameStateInformationProvider.NormalGameStart += anyGameStartedCaller;
		GameStateInformationProvider.NewGamePlusStart += anyGameStartedCaller;

		GameStateInformationProvider.AnyGameStart += StartGame;
		GameStateInformationProvider.anEchangeEnded += CheckForEndGame;
		GameStateInformationProvider.GameEnded += OnGameEnd;



		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}



	//S///////////////////////////////////////////////////////////     Start       /////////////////////////////////////////////////////////////
	private void Start()
	{
		GameStateInformationProvider.GameInitalize();                                          //         -------------------------------------------------------->>>
	}




	void anyGameStartedCaller()
	{
		GameStateInformationProvider.AnyGameStart();
	}

	//S///////////////////////////////////////////////////////////     Choose countriesTo be used       /////////////////////////////////////////////////////////////

	void UniqueReactionToStartingANormalGame()
	{
		countriesLoadedAccordingToGameMode = countriesUsedInMainGame;
		GameStateInformationProvider.currentGameType = GameStates.NormalGame;
	}
	void UniqueReactionToStartingANewGamePLus()
	{
		countriesLoadedAccordingToGameMode = countriesUsedInNewGamePlus;
		GameStateInformationProvider.currentGameType = GameStates.NewGamePlus;
	}

	//S///////////////////////////////////////////////////////////     Normal game start     /////////////////////////////////////////////////////////////

	void StartGame()                                                             //         <<<--------------------------------------------------------------
	{
		TellAssighningWorkerToAssighnEachCountryItsNeedVAlue(countriesLoadedAccordingToGameMode);

		SubAwakeTellEachCountryToMakeThierSigns(countriesLoadedAccordingToGameMode);

	}
	                  
	                   void TellAssighningWorkerToAssighnEachCountryItsNeedVAlue(List<Country> countriesUsedInThisGame)
	{
		Debug.Log(countriesUsedInThisGame.Count);
		ourNeedValueAssighnerWorker.AssighnChoosenNeedsValueToChoosenCountries(needsUsedInGame, countriesUsedInThisGame);
	}

	                   void SubAwakeTellEachCountryToMakeThierSigns(List<Country> countriesUsedInThisGame)
	{
		foreach (Country countryToTellToSTart in countriesUsedInThisGame)
		{
			countryToTellToSTart.ForighnOrderByGameMakerToStartMakingSigns();
		}
	}








	//S///////////////////////////////////////////////////////////     on game end       /////////////////////////////////////////////////////////////
	void OnGameEnd()
	{
		IfThereIsAPreviousltMadeGameClearIt();
	}
	                  void IfThereIsAPreviousltMadeGameClearIt()
	{
		foreach (Country country in countriesLoadedAccordingToGameMode)
		{
			country.ourCountriesNeedsAndTheirValue.Clear();

			if (country.ourmainSign != null)
			{
				Destroy(country.ourmainSign.gameObject);
			}


			foreach (Need sign in country.ourCountriesSigns)
			{
				Destroy(sign.gameObject);
			}

			country.ourCountriesSigns.Clear();
		}
	}


	//S///////////////////////////////////////////////////////////     Game end check       /////////////////////////////////////////////////////////////
	void CheckForEndGame(Needs unUsed)
	{
		if (ReturnTrueIfAllSignInGameIs100())
		{
			if(GameStateInformationProvider.AllSigns100 != null)
			{
				GameStateInformationProvider.AllSigns100();
			}
			
		}
	}

	bool ReturnTrueIfAllSignInGameIs100()
	{
		foreach (Country country in countriesLoadedAccordingToGameMode)
		{
			foreach (Need sign in country.ourCountriesSigns)
			{
				if (sign.currentSignValue != 100)
				{
					return false;
				}
			}
		}
		return true;
	}


}

