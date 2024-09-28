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
	public OpeningSign openningScreenSign;

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
	/////////////////////////////////////////////////////////////
	float timeBetweenSignSpawn = 5f;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////////        Actions       //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	/////////////////////////////////////////////////////////////     Set Reactions      /////////////////////////////////////////////////////////////
	private void Awake()
	{
		GameStateInformationProvider.GameStarted += HigherUpOrderToStartWork;
		GameStateInformationProvider.OpeningScreenClosed += OnOpeningScreenClosed;
		GameStateInformationProvider.anEchangeEnded += MakeANeedSpecificSignAsAnExchangeWasComplete;
		GameStateInformationProvider.TutorialStarted += TutorialStartedReaction;
		GameStateInformationProvider.TutorialEnded += SignManagerREcievedThatTutorialEnd;
	}


	/////////////////////////////////////////////////////////////       start       /////////////////////////////////////////////////////////////
	 void HigherUpOrderToStartWork()                                                    //         <<<--------------------------------------------------------------
	{
		SubStartInitalize();
		SubStartShowOpeningScreen();
	}
	        void SubStartInitalize()
	{
		ourSignDrawer = this.GetComponentInChildren<SignTechnikalDrawer>();
		ourSignDrawer.ourCanvasToDrawOn = ourCanvasToDrawIN;
		ourSignDrawer.ourheadManager = this;
		ourSignDrawer.HigherUpOrderInitalize();
	}
	        void SubStartShowOpeningScreen()
	{
		ourSignDrawer.MakeOpeningSignInMiddle(openningScreenSign);
	}
	



	//OA///////////////////////////////////////////////////////////     On opening screen closed    /////////////////////////////////////////////////////////////
	void OnOpeningScreenClosed()                                                         //         <<<-------------------------------------------------------------- 
	{
		StartCoroutine(CycleToSpawnGeneralSigns(timeBetweenSignSpawn));
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
		generalSignTimer += 4;
	}




	//OA///////////////////////////////////////////////////////////     Tutorial start reaction      /////////////////////////////////////////////////////////////
	List<ImaginationSign> tutorialSignsMadeAndAreOnScreen = new List<ImaginationSign>();
	void TutorialStartedReaction()                                          //         <<<-------------------------------------------------------------- 
	{
		foreach(ImaginationSign tutorialSign in listOfTutorialSigns)
		{
			ourSignDrawer.MakeTutorialSign(tutorialSign);		
		}
		/////////////////////////////////////////////////////////////
		generalSignTimer += 999;
	}
	public void SubWorkerMessageTutorialSignSuccefullyMade(ImaginationSign signMade)
	{
		tutorialSignsMadeAndAreOnScreen.Add(signMade);
	}

	//OA///////////////////////////////////////////////////////////     Tutorial End reaction      /////////////////////////////////////////////////////////////
	void TutorialEndReaction()
	{
		foreach (ImaginationSign tutorialSign in tutorialSignsMadeAndAreOnScreen)
		{
			Destroy(tutorialSign);
		}
		tutorialSignsMadeAndAreOnScreen.Clear();
		/////////////////////////////////////////////////////////////
		generalSignTimer = timeBetweenSignSpawn;
	}
	void SignManagerREcievedThatTutorialEnd()
	{
		Debug.Log("SignManagerREcievedThatTutorialEnd");
	}



	//S///////////////////////////////////////////////////////////     SubWorker messages       /////////////////////////////////////////////////////////////
	public void SubWorkerMessageSignWasSuccefullyMade(ImaginationSign signThatWasMade)
	{
		signsMadeAndAreOnScreen.Add(signThatWasMade);
		
		foreach(ImaginationSign sing in signsMadeAndAreOnScreen)
		{
			Debug.Log("list is now "+sing.name);
		}
	}
	public void SubWorkerMessageSignIsGoingToLeave(ImaginationSign signThatWillLeave)
	{
		Debug.Log("removed");
		signsMadeAndAreOnScreen.Remove(signThatWillLeave);
		
	}

}
