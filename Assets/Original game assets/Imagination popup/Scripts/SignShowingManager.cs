using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignShowingManager : MonoBehaviour  //the script job is to decide when to spawn signs then it sends the sign to its subwroker the (SignTecknicalDrawer) to draw it on screen
												 //this takes the signs given in inspector and continously spawns genral signs through //////General sign spawner///// or when exchagne completes 
												 //spawns a need specific sing through the  //////Make a need sign when an exhcange is completed //////
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	///////////////////////////////////////////////////////////////manual connection
	[Header("Signs to place")]

	public List<ImaginationSign> ListOfGeneralSigns = new List<ImaginationSign>();

	public List<ImaginationSign> listOfTutorialSigns = new List<ImaginationSign>();

	public List<ImaginationSign> ListOfFoodSigns = new List<ImaginationSign>();
	public List<ImaginationSign> ListOfMaterialseSigns = new List<ImaginationSign>();
	public List<ImaginationSign> ListOfElectronicsSigns = new List<ImaginationSign>();
	public List<ImaginationSign> ListOfMechanicsSigns = new List<ImaginationSign>();

    [SerializeField]             //canvas to draw in
    Canvas ourCanvasToDrawIN;

	/////////////////////////////////////////////////////////////	//auto connection //our subworker
	SignTechnikalDrawer ourSignDrawer;

	/////////////////////////////////////////////////////////////    //memory
	List<ImaginationSign> signsMadeAndAreOnScreen = new List<ImaginationSign>();
	List<ImaginationSign> tutorialSignsMadeAndAreOnScreen = new List<ImaginationSign>();
	/////////////////////////////////////////////////////////////
	float timeBetweenSignSpawn = 9f;

	/////////////////////////////////////////////////////////////
	Coroutine ourGeneralSignCycle;



	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////////        Actions       //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	/////////////////////////////////////////////////////////////     Set Reactions      /////////////////////////////////////////////////////////////
	private void Awake()
	{
		GameStateInformationProvider.GameInitalize += HigherUpOrderToStartWork;
		GameStateInformationProvider.NormalGameStart += OnGameInitalize;
		GameStateInformationProvider.anEchangeEnded += MakeANeedSpecificSignAsAnExchangeWasComplete;
		GameStateInformationProvider.TutorialStarted += TutorialStartTrigger;
		GameStateInformationProvider.TutorialEnded += TutorialEndReaction;
		GameStateInformationProvider.GameEnded += OnGameEnded;
;
	}


	/////////////////////////////////////////////////////////////       Initalize      /////////////////////////////////////////////////////////////
	 void HigherUpOrderToStartWork()                                                    //         <<<--------------------------------------------------------------
	{
		SubStartInitalize();
	}
	        void SubStartInitalize()
	{
		ourSignDrawer = this.GetComponentInChildren<SignTechnikalDrawer>();
		ourSignDrawer.ourCanvasToDrawOn = ourCanvasToDrawIN;
		ourSignDrawer.ourheadManager = this;
		ourSignDrawer.HigherUpOrderInitalize();
	}

	



	//OA///////////////////////////////////////////////////////////     On Game Start and End    /////////////////////////////////////////////////////////////
	void OnGameInitalize()                                                         //         <<<-------------------------------------------------------------- 
	{
		ourGeneralSignCycle =StartCoroutine(CycleToSpawnGeneralSigns(timeBetweenSignSpawn));
	}

	void OnGameEnded()
	{
		StopCoroutine(ourGeneralSignCycle);

		foreach(ImaginationSign SignToDestoy in signsMadeAndAreOnScreen)
		{
			Destroy(SignToDestoy.gameObject);
		}
	}


	//OA///////////////////////////////////////////////////////////     General sign spawner      /////////////////////////////////////////////////////////////
	float generalSignTimer = 0;
	IEnumerator CycleToSpawnGeneralSigns(float timeBetweenSignSpawns)
	{
		///////////////////////////////////////////////////////////// Keep a memory of a mirror list and the spawn repeatTimer 
		////////////////////////////
		/////////////
		List<ImaginationSign> listOfSignsWeAreYetToMake = SubIenumMakeAMirrorList(ListOfGeneralSigns);

		float signTimer = timeBetweenSignSpawns;

		/////////////////////////////////////////////////////////////        Cycle
		//////////////////////////
		///////////////
		while (true)
		{
			AdvanceTimerANdIfItsTimeSpawnAGenralSign(ref listOfSignsWeAreYetToMake,timeBetweenSignSpawns);
			yield return null;
		}

	}
	         void AdvanceTimerANdIfItsTimeSpawnAGenralSign(ref List<ImaginationSign> listOfSignsWeAreYetToMake,float timeBetweenSpawns)
	{
		///////////////////////////////////////////////////////////// repeat list if finished
		if (listOfSignsWeAreYetToMake.Count == 0)
		{
			listOfSignsWeAreYetToMake = SubIenumMakeAMirrorList(ListOfGeneralSigns);
			
		}
		
		///////////////////////////////////////////////////////////// decrease time
		generalSignTimer -= Time.deltaTime;

		///////////////////////////////////////////////////////////// iff timer is done thsi cycle
		if (generalSignTimer < 0)
		{
			ourSignDrawer.MakeNormalSignSign(listOfSignsWeAreYetToMake[0]);
			listOfSignsWeAreYetToMake.RemoveAt(0);
			generalSignTimer = timeBetweenSpawns;
		}
	}
	              List<ImaginationSign> SubIenumMakeAMirrorList(List<ImaginationSign> listOfSigns)
	{
		List<ImaginationSign> listOfSignsWeAreYetToMake = new List<ImaginationSign>();
		foreach (ImaginationSign Sign in listOfSigns)
		{
			listOfSignsWeAreYetToMake.Add(Sign);
		}
	
		return listOfSignsWeAreYetToMake;
	}





	//S///////////////////////////////////////////////////////////     Make a need sign when an exhcange is completed       ///////////////////////////////////////////////////////////// 
	List<ImaginationSign> ListOfFoodSignsYetToBeMade = new List<ImaginationSign>();
	List<ImaginationSign> ListOfMaterialseSignsYetToBeMade = new List<ImaginationSign>();
	List<ImaginationSign> ListOfElectronicsSignsYetToBeMade = new List<ImaginationSign>();
	List<ImaginationSign> ListOfMechanicsSignsYetToBeMade = new List<ImaginationSign>();
	///////////////////////////
	void MakeANeedSpecificSignAsAnExchangeWasComplete(Needs needOfExchangeCompleted)                                       //         <<<--------------------------------------------------------------
	{
		switch (needOfExchangeCompleted)
		{
			case (Needs.Food):

				MakeASignOfThisNeedType(Needs.Food,ref ListOfFoodSignsYetToBeMade, ListOfFoodSigns);

				/////////////////////////////////////////////////////////////
				break;

			case (Needs.Electronics):

				MakeASignOfThisNeedType(Needs.Electronics, ref ListOfElectronicsSignsYetToBeMade, ListOfElectronicsSigns);

				/////////////////////////////////////////////////////////////
				break;

			case (Needs.Machines):

				MakeASignOfThisNeedType(Needs.Machines, ref ListOfMechanicsSignsYetToBeMade, ListOfMechanicsSigns);

				/////////////////////////////////////////////////////////////
				break;

			case (Needs.Materials):

				MakeASignOfThisNeedType(Needs.Materials, ref ListOfMaterialseSignsYetToBeMade, ListOfMaterialseSigns);

				/////////////////////////////////////////////////////////////
				break;

		}
	}
	                  void MakeASignOfThisNeedType(Needs needOfExchangeCompleted,ref List<ImaginationSign> mirrorListOfSigns, List<ImaginationSign> OriginalListOfSigns) 
	{
		
		if (mirrorListOfSigns.Count == 0)
		{
			mirrorListOfSigns = SubIenumMakeAMirrorList(OriginalListOfSigns);
		}
		/////////////////////////////////////////////////////////////

		ourSignDrawer.MakeNormalSignSign(mirrorListOfSigns[0]);
		mirrorListOfSigns.RemoveAt(0);
		generalSignTimer = (timeBetweenSignSpawn/2) + ((generalSignTimer / timeBetweenSignSpawn) * (timeBetweenSignSpawn / 2));//(generalSignTimer/timeBetweenSignSpawn) * timeBetweenSignSpawn ;
	}




	//OA///////////////////////////////////////////////////////////     Tutorial start reaction      /////////////////////////////////////////////////////////////
	void TutorialStartTrigger()
	{
		StartCoroutine(TutorialStartedReaction());
	}
	IEnumerator TutorialStartedReaction()                                          //         <<<-------------------------------------------------------------- 
	{
		generalSignTimer += 999;
		/////////////////////////////////////////////////////////////
		foreach (ImaginationSign tutorialSign in listOfTutorialSigns)
		{
			ourSignDrawer.MakeTutorialSign(tutorialSign);	
			yield return new WaitForSecondsRealtime(4f);
		}
	
		
	}
	public void SubWorkerMessageTutorialSignSuccefullyMade(ImaginationSign signMade)
	{
		tutorialSignsMadeAndAreOnScreen.Add(signMade);
	}

	//OA///////////////////////////////////////////////////////////     Tutorial End reaction      /////////////////////////////////////////////////////////////
	void TutorialEndReaction()
	{
		List<ImaginationSign> signToDestroy = new List<ImaginationSign>();
		foreach (ImaginationSign tutorialSign in tutorialSignsMadeAndAreOnScreen)
		{
			signToDestroy.Add(tutorialSign);
			
		}
		foreach (ImaginationSign tutorialSign in signToDestroy)
		{

			tutorialSign.OnXButtonClick();
		}	
		/////////////////////////////////////////////////////////////
		generalSignTimer = timeBetweenSignSpawn;
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			TutorialEndReaction();
		}
	}



	//S///////////////////////////////////////////////////////////     SubWorker messages       /////////////////////////////////////////////////////////////
	public void SubWorkerMessageSignWasSuccefullyMade(ImaginationSign signThatWasMade)
	{
		switch (signThatWasMade.ourSignType)
		{
			case (SignType.Normal):
				signsMadeAndAreOnScreen.Add(signThatWasMade);
				break;
			case SignType.Tutorial:
				tutorialSignsMadeAndAreOnScreen.Add(signThatWasMade);
				break;
		}
			
			foreach (ImaginationSign sing in signsMadeAndAreOnScreen)
		{
			Debug.Log("list is now "+sing.name);
		}
	}
	public void SubWorkerMessageSignIsGoingToLeave(ImaginationSign signThatWillLeave)
	{
		switch (signThatWillLeave.ourSignType)
		{
			case (SignType.Normal):
				signsMadeAndAreOnScreen.Remove(signThatWillLeave);
				break;
			case SignType.Tutorial:
				tutorialSignsMadeAndAreOnScreen.Remove(signThatWillLeave);
				break;
		}
		//signsMadeAndAreOnScreen.Remove(signThatWillLeave);
		
	}

}
