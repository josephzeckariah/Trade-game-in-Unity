using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignShowingManager : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	///////////////////////////////////////////////////////////////manual connection
	public OpeningSign openningScreenSign;

	public List<ImaginationSign> ListOfGeneralSigns = new List<ImaginationSign>();

	public List<ImaginationSign> ListOfFoodSigns = new List<ImaginationSign>();
	public List<ImaginationSign> ListOfMaterialseSigns = new List<ImaginationSign>();
	public List<ImaginationSign> ListOfElectronicsSigns = new List<ImaginationSign>();
	public List<ImaginationSign> ListOfMechanicsSigns = new List<ImaginationSign>();

    [SerializeField]             //canvas to draw in
    Canvas ourCanvasToDrawIN;

	/////////////////////////////////////////////////////////////	//auto connection //our subworker
	SignTechnikalDrawer ourSignDrawer;

	/////////////////////////////////////////////////////////////    //memory
	 //List<ImaginationSign> ListOfMadeSigns = new List<ImaginationSign>();

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////////        Actions       //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	/////////////////////////////////////////////////////////////     Set Reactions      /////////////////////////////////////////////////////////////
	private void Awake()
	{
		GameStateInformationProvider.GameStarted += HigherUpOrderToStartWork;
		GameStateInformationProvider.OpeningScreenClosed += OnOpeningScreenClosed;
		GameStateInformationProvider.anEchangeEnded += MakeANeedSpecificSignAsAnExchangeWasComplete;
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
		ourSignDrawer.HigherUpOrderStartWork();
	}
	        void SubStartShowOpeningScreen()
	{
		ourSignDrawer.MakeOpeningSignInMiddle(openningScreenSign);
	}


	//OA///////////////////////////////////////////////////////////     Ocational actions      /////////////////////////////////////////////////////////////
	void OnOpeningScreenClosed()                                                         //         <<<-------------------------------------------------------------- 
	{
		StartCoroutine(CycleToSpawnGeneralSigns(5f));
	}

	//OA///////////////////////////////////////////////////////////     Ocational Cycle      /////////////////////////////////////////////////////////////
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
			///////////////////////////////////////////////////////////// repeat list if finished
			if (listOfSignsWeAreYetToMake.Count == 0) 
			{
				listOfSignsWeAreYetToMake = SubIenumMakeAMirrorList(ListOfGeneralSigns);
			}
			///////////////////////////////////////////////////////////// decrease time
			signTimer -= Time.deltaTime;

			///////////////////////////////////////////////////////////// iff timer is done thsi cycle
			if (signTimer < 0)
			{
				ourSignDrawer.MakeSignInARandomLocation(listOfSignsWeAreYetToMake[0]);
				listOfSignsWeAreYetToMake.RemoveAt(0);
				signTimer = timeBetweenSignSpawns;
			}

			///////////////////////////////////////////////////////////// repeat
			yield return null;
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

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Make a need sign when an exhcange is completed
	////////////////////////////////////////////////////////////////       private mithod memory
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

		ourSignDrawer.MakeSignInARandomLocation(mirrorListOfSigns[0]);
		mirrorListOfSigns.RemoveAt(0);
	}
}
