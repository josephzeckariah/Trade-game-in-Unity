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
	public List<Country> countriesUsedInGame;

	[Header("Manual connections")]
	public  GameObject needSighnTemplateSHowInspector;
	public  Sprite FoodSprite;
	public  Sprite MaterialsSprite;                                                            //set by inspector General game data
	public  Sprite ElectronicsSprite;
	public Sprite MachinesSprite;

	public Sprite SmileSprite;
	public Sprite MoneySprite;

	//public memory
	public static Sprite Smile;
	public static Sprite Money;
	public static Dictionary<Needs,Sprite> needsAssets = new Dictionary<Needs, Sprite>();
	public static GameObject needSighnTemplate;
	//auto connection
	NeedValueAssighnerWorker ourNeedValueAssighnerWorker;
	SignShowingManager ourSignManager;


	public static bool tutorialIsOn = false;

	public static int numberofExchangesCompleted = 0;


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//I///////////////////////////////////////////////////////////     Initalize       /////////////////////////////////////////////////////////////
	void Awake()
	{
		ourNeedValueAssighnerWorker = this.GetComponentInChildren<NeedValueAssighnerWorker>();
		ourSignManager = this.GetComponentInChildren<SignShowingManager>();

		needSighnTemplate = needSighnTemplateSHowInspector;
		
		needsAssets.Add(Needs.Food, FoodSprite);
		needsAssets.Add(Needs.Materials, MaterialsSprite);
		needsAssets.Add(Needs.Electronics, ElectronicsSprite);
		needsAssets.Add(Needs.Machines, MachinesSprite);

		Money = MoneySprite;
		Smile = SmileSprite;

		GameStateInformationProvider.anEchangeEnded += CheckForEndGame;
		GameStateInformationProvider.NormalGameStart += StartNormalGame;
		GameStateInformationProvider.GameEnded += OnGameEnd;
	}



	//S///////////////////////////////////////////////////////////     Start       /////////////////////////////////////////////////////////////
	private void Start()
	{
		GameStateInformationProvider.GameInitalize();                                          //         -------------------------------------------------------->>>
	}





	//S///////////////////////////////////////////////////////////     Normal game start     /////////////////////////////////////////////////////////////

	void StartNormalGame()                                                             //         <<<--------------------------------------------------------------
	{	

		TellAssighningWorkerToAssighnEachCountryItsNeedVAlue();

		SubAwakeTellEachCountryToMakeThierSigns();

	}
	                  
	                   void TellAssighningWorkerToAssighnEachCountryItsNeedVAlue()
	{

		ourNeedValueAssighnerWorker.AssighnChoosenNeedsValueToChoosenCountries(needsUsedInGame, countriesUsedInGame);
	}

	                   void SubAwakeTellEachCountryToMakeThierSigns()
	{
		foreach (Country countryToTellToSTart in countriesUsedInGame)
		{
			countryToTellToSTart.ForighnOrderByGameMakerToStartMakingSigns();
		}
	}




	void OnGameEnd()
	{
		IfThereIsAPreviousltMadeGameClearIt();
	}
	                  void IfThereIsAPreviousltMadeGameClearIt()
	{
		foreach (Country country in countriesUsedInGame)
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
			if(GameStateInformationProvider.GameEnded != null)
			{
				GameStateInformationProvider.GameEnded();
			}
			
		}
	}
	               bool ReturnTrueIfAllSignInGameIs100()
	{
		foreach (Country country in countriesUsedInGame)
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

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (GameStateInformationProvider.GameEnded != null)
			{
				GameStateInformationProvider.GameEnded();
			}
		}
	}
}

