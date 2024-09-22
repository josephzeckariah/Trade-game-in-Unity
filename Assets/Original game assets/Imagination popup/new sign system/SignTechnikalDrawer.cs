using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum screenLocation { left, button, right };

public class SignTechnikalDrawer : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	///////////////////////////////////////////////////////////////////////////////////   auto connections
	[HideInInspector]
	public SignShowingManager ourheadManager;
	[HideInInspector]
    public Canvas ourCanvasToDrawOn;
	[HideInInspector]
	public RectTransform ourCanvasRectTransform;
	
	/////////////////////////////////////////////////////////////////////////////////////     private memories
	List<Vector2> listOfMadeSignsDImensionsLeft = new List<Vector2>();
	List<Vector2> listOfMadeSignsDImensionsButton = new List<Vector2>();
	List<Vector2> listOfMadeSignsDImensionsRight = new List<Vector2>();

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//S///////////////////////////////////////////////////////////     Start       /////////////////////////////////////////////////////////////
	public void HigherUpOrderStartWork()
	{
		ourCanvasRectTransform = ourCanvasToDrawOn.GetComponent<RectTransform>();
	}
	/////////////////////////////////////////////////////////////   MakeInRandom    /////////////////////////////////////////////////////////////
	public void MakeSignInARandomLocation(ImaginationSign signToShow)
	{
		///////////////////////////////////////////////////////////// make sign
		ImaginationSign spawnedSign = SubFunSignFabricator(signToShow);                                           //make sign
		RectTransform transofrmOfNewSign = spawnedSign.gameObject.GetComponent<RectTransform>();
		Vector2 pecentageOfSignOfCanvas = new Vector2((transofrmOfNewSign.sizeDelta.x / 2) / ourCanvasRectTransform.sizeDelta.x, (transofrmOfNewSign.sizeDelta.y / 2) / ourCanvasRectTransform.sizeDelta.y);

		/////////////////////////////////////////////////////////////
		int numberOfTimesPostionWasReconsidered = 0;                            //check position reconsideration itiration
	restartPointToReCpnsiderPosition:  
		numberOfTimesPostionWasReconsidered += 1;
		if (numberOfTimesPostionWasReconsidered > 15)
		{
			Destroy(spawnedSign.gameObject);
			return;
		}    //check reconsiderationOfPosition if toomany
			 /////////////////////////////////////////////////////////////
	

		screenLocation whereToPLaceSign = (screenLocation)Random.Range(0, 3);             //randomly pick a (right,button,lift)then randomly place sign in it
		switch (whereToPLaceSign)
		{
			case screenLocation.left:
				transofrmOfNewSign.anchorMin = new Vector2(0 + pecentageOfSignOfCanvas.x, Random.Range(pecentageOfSignOfCanvas.y, 1 - pecentageOfSignOfCanvas.y));

				break;
			case screenLocation.button:
				transofrmOfNewSign.anchorMin = new Vector2(Random.Range(pecentageOfSignOfCanvas.x * 3, 1 - (pecentageOfSignOfCanvas.x * 3)), pecentageOfSignOfCanvas.y);
				break;
			case screenLocation.right:
				transofrmOfNewSign.anchorMin = new Vector2(1 - pecentageOfSignOfCanvas.x, Random.Range(pecentageOfSignOfCanvas.y, 1 - pecentageOfSignOfCanvas.y));
				break;
		}
		transofrmOfNewSign.anchorMax = transofrmOfNewSign.anchorMin;
		

		/////////////////////////////////////////////////////////////
		//check sign if its touching another then rechoose another position
		switch (whereToPLaceSign)
		{
			case screenLocation.left:
				int positionOfSignInVectorLeft = (int)(ourCanvasRectTransform.sizeDelta.y * transofrmOfNewSign.anchorMin.y);          //save dimension occupied
				Vector2 pixelRangeOccupiedBySignLeft = new Vector2(positionOfSignInVectorLeft - (transofrmOfNewSign.sizeDelta.y / 2), positionOfSignInVectorLeft + (transofrmOfNewSign.sizeDelta.y / 2));

				if (!checkLane(pixelRangeOccupiedBySignLeft, listOfMadeSignsDImensionsLeft))
				{
					goto restartPointToReCpnsiderPosition;
				}

				break;


			case screenLocation.button:
				int positionOfSignInVectorButton = (int)(ourCanvasRectTransform.sizeDelta.x * transofrmOfNewSign.anchorMin.x);          //save dimension occupied
				Vector2 pixelRangeOccupiedBySignButton = new Vector2(positionOfSignInVectorButton - (transofrmOfNewSign.sizeDelta.x / 2), positionOfSignInVectorButton + (transofrmOfNewSign.sizeDelta.x / 2));

				//check current sign with privoius sings for collisions
				if (!checkLane(pixelRangeOccupiedBySignButton, listOfMadeSignsDImensionsButton))
				{
					goto restartPointToReCpnsiderPosition;
				}			
				break;


			case screenLocation.right:
				int positionOfSignInVectorRight = (int)(ourCanvasRectTransform.sizeDelta.y * transofrmOfNewSign.anchorMin.y);          //save dimension occupied
				Vector2 pixelRangeOccupiedBySignRight = new Vector2(positionOfSignInVectorRight - (transofrmOfNewSign.sizeDelta.y / 2), positionOfSignInVectorRight + (transofrmOfNewSign.sizeDelta.y / 2));

				if (!checkLane(pixelRangeOccupiedBySignRight, listOfMadeSignsDImensionsRight))
				{
					goto restartPointToReCpnsiderPosition;
				}

				break;
		}


		transofrmOfNewSign.anchoredPosition3D = Vector3.zero;
	}                                         //         <<<--------------------------------------------------------------


	                 bool checkLane(Vector2 rangeOfMainSign, List<Vector2> listOfPReviousSigns)
	{
		foreach (Vector2 pixelRangeOfPReviouslyLaidSigns in listOfPReviousSigns)
		{
			if (rangeOfMainSign.x < pixelRangeOfPReviouslyLaidSigns.x)
			{
				if (rangeOfMainSign.y < pixelRangeOfPReviouslyLaidSigns.x)
				{
					//all is fine continue
					continue;
				}
				else if (rangeOfMainSign.y > pixelRangeOfPReviouslyLaidSigns.x)
				{

					//redraw
					return false;
				}
			}
			else if (rangeOfMainSign.x > pixelRangeOfPReviouslyLaidSigns.x)
			{
				if (rangeOfMainSign.x > pixelRangeOfPReviouslyLaidSigns.y)
				{
					//all is fine continue
					continue;
				}
				else if (rangeOfMainSign.x < pixelRangeOfPReviouslyLaidSigns.y)
				{

					//redraw
					return false;
				}
			}
			else if (rangeOfMainSign.x == pixelRangeOfPReviouslyLaidSigns.x)
			{

				//redraw
				return false;
			}
		}
		//if all is fine then:
		listOfPReviousSigns.Add(rangeOfMainSign);
		return true;
	}


	                 ImaginationSign SubFunSignFabricator(ImaginationSign signToFabricate)
	{
		ImaginationSign spawnedSign = Instantiate(signToFabricate, ourCanvasToDrawOn.transform);


		spawnedSign.ourHeadTeEchnicalDrawer = this;
		


		return spawnedSign;
	}


	/////////////////////////////////////////////////////////////  Make opening in middle      /////////////////////////////////////////////////////////////

	public void MakeOpeningSignInMiddle(OpeningSign openingSign)
	{
		/////////////////////////////////////////////////////////////        Make sign
		OpeningSign spawnedSign  = Instantiate(openingSign, ourCanvasToDrawOn.transform);
		RectTransform transofrmOfNewSign = spawnedSign.gameObject.GetComponent<RectTransform>();


		/////////////////////////////////////////////////////////////       Position Sign and achor
		transofrmOfNewSign.anchorMax = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchorMin = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchoredPosition3D = Vector3.zero;

		/////////////////////////////////////////////////////////////      initalize sign
		openingSign.ourHeadTechnicalDrawer = this;
		openingSign.ourCanvasRectTransform = ourCanvasRectTransform;
	}                                              //         <<<--------------------------------------------------------------

	/////////////////////////////////////////////////////////////    Raw make      /////////////////////////////////////////////////////////////

	void InformOurHeadManagerThatWeHaveMadeThisSignSuccesfully(ImaginationSign signMade)
	{

	}
}
