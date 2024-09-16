using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum Needs { Vegs, Fruit, Cloth, Wood };
public delegate void ResolutionDelegate(Vector2 resolution);
public delegate void ExchangeDelegate(Needs typeOfExchange);

public  class GameMaker : MonoBehaviour
{
	[Header("Manual properits")]
	public List<Needs> needsUsedInGame ;
	public List<Country> countriesUsedInGame;

	[Header("Manual connections")]
	public  GameObject needSighnTemplateSHowInspector;
	public  Sprite vegsSprite;
	public  Sprite fruitSprite;                                                            //set by inspector General game data
	public  Sprite woodSprite;
	public Sprite clothSprite;

	public Sprite SmileSprite;
	public Sprite MoneySprite;

	//public memory
	public static Sprite Smile;
	public static Sprite Money;
	public static Dictionary<Needs,Sprite> needsAssets = new Dictionary<Needs, Sprite>();
	public static GameObject needSighnTemplate;
	//auto connection
	NeedValueAssighnerWorker ourNeedValueAssighnerWorker;
	ImaginationSignShower ourImaginationSignShower;

	//events	
	public static ResolutionDelegate gameScreenSizeChanged;
	public static ExchangeDelegate anEchangeStarted;
	public static ExchangeDelegate anEchangeEnded;
	void Awake()
	{
		ourNeedValueAssighnerWorker = this.GetComponentInChildren<NeedValueAssighnerWorker>();
		ourImaginationSignShower = this.GetComponentInChildren<ImaginationSignShower>();

		needSighnTemplate = needSighnTemplateSHowInspector;
		
		needsAssets.Add(Needs.Vegs, vegsSprite);
		needsAssets.Add(Needs.Fruit, fruitSprite);
		needsAssets.Add(Needs.Wood, woodSprite);
		needsAssets.Add(Needs.Cloth, clothSprite);

		Money = MoneySprite;
		Smile = SmileSprite;

		
	}
	private void Start()
	{
		StartCoroutine(AlwaysCeckForSCreenSizeChange());

		TellAssighningWorkerToAssighnEachCountryItsNeedVAlue();
		SubAwakeTellEachCountryToMakeThierSigns();
		SubAwakeTellTheImaginationSignsToStart();
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

	void SubAwakeTellTheImaginationSignsToStart()
	{
		ourImaginationSignShower.ForignOrderShowOpeningSccreen();
	}


	//still unfinished
	IEnumerator AlwaysCeckForSCreenSizeChange()
	{
		Vector2 previousScreenSize = new Vector2( Camera.main.pixelWidth, Camera.main.pixelHeight );
		while (true)
		{
			if(Camera.main.pixelWidth != previousScreenSize.x || Camera.main.pixelHeight!= previousScreenSize.y)
			{
				Debug.Log("we need to recalculate");
				if(gameScreenSizeChanged != null)
				{
					gameScreenSizeChanged(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));
				}
				previousScreenSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
			}
			yield return new WaitForSecondsRealtime(0.2f);
		}
	}
}

