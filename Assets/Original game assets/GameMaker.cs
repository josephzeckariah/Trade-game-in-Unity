using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum Needs { Food,Materials,Electronics,Mechanics };
public delegate void ResolutionDelegate(Vector2 resolution);
//public delegate void ExchangeDelegate();


public  class GameMaker : MonoBehaviour
{
	[Header("Manual properits")]
	public List<Needs> needsUsedInGame ;
	public List<Country> countriesUsedInGame;

	[Header("Manual connections")]
	public  GameObject needSighnTemplateSHowInspector;
	public  Sprite FoodSprite;
	public  Sprite MaterialsSprite;                                                            //set by inspector General game data
	public  Sprite ElectronicsSprite;
	public Sprite MechanicsSprite;

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

	//events	
	public static ResolutionDelegate gameScreenSizeChanged;
	//public static ExchangeDelegate anEchangeStarted;
	//public static ExchangeDelegate anEchangeEnded;

	public static bool tutorialIsOn = false;

	public static int numberofExchangesCompleted = 0;
	void Awake()
	{
		ourNeedValueAssighnerWorker = this.GetComponentInChildren<NeedValueAssighnerWorker>();
		ourSignManager = this.GetComponentInChildren<SignShowingManager>();

		needSighnTemplate = needSighnTemplateSHowInspector;
		
		needsAssets.Add(Needs.Food, FoodSprite);
		needsAssets.Add(Needs.Materials, MaterialsSprite);
		needsAssets.Add(Needs.Electronics, ElectronicsSprite);
		needsAssets.Add(Needs.Mechanics, MechanicsSprite);

		Money = MoneySprite;
		Smile = SmileSprite;

		
	}
	private void Start()
	{
		//StartCoroutine(AlwaysCeckForSCreenSizeChange());

		TellAssighningWorkerToAssighnEachCountryItsNeedVAlue();
		SubAwakeTellEachCountryToMakeThierSigns();
				
		if (GameStateInformationProvider.GameStarted != null)
		{
			GameStateInformationProvider.GameStarted();                        //         -------------------------------------------------------->>>
		}
	}
	void TellAssighningWorkerToAssighnEachCountryItsNeedVAlue()
	{

		ourNeedValueAssighnerWorker.AssighnChoosenNeedsValueToChoosenCountries(needsUsedInGame,countriesUsedInGame );
	}
	
	void SubAwakeTellEachCountryToMakeThierSigns()
	{
		foreach (Country countryToTellToSTart in countriesUsedInGame)
		{
			countryToTellToSTart.ForighnOrderByGameMakerToStartMakingSigns();
		}
	}

	


	//still unfinished
	/*IEnumerator AlwaysCeckForSCreenSizeChange()
	{
		Vector2 previousScreenSize = new Vector2( Camera.main.pixelWidth, Camera.main.pixelHeight );
		while (true)
		{
			if(Camera.main.pixelWidth != previousScreenSize.x || Camera.main.pixelHeight!= previousScreenSize.y)
			{
				
				if(gameScreenSizeChanged != null)
				{
					gameScreenSizeChanged(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));
				}
				previousScreenSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
			}
			yield return new WaitForSecondsRealtime(0.2f);
		}
	}*/
}

