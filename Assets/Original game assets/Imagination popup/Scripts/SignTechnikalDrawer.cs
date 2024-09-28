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
	Dictionary<ImaginationSign, Vector2> listOfMadeSignsDImensionsLeft = new Dictionary<ImaginationSign, Vector2>();
	Dictionary<ImaginationSign, Vector2> listOfMadeSignsDImensionsButton = new Dictionary<ImaginationSign, Vector2>();
	Dictionary<ImaginationSign, Vector2> listOfMadeSignsDImensionsRight = new Dictionary<ImaginationSign, Vector2>();

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//S///////////////////////////////////////////////////////////     Start       /////////////////////////////////////////////////////////////
	public void HigherUpOrderInitalize()
	{
		ourCanvasRectTransform = ourCanvasToDrawOn.GetComponent<RectTransform>();
	}


	//S///////////////////////////////////////////////////////////     Make tutorial Signs       /////////////////////////////////////////////////////////////
	public void MakeTutorialSign(ImaginationSign SignThatWondDisapear)
	{
		ImaginationSign newSign = MakeSignInARandomLocation(SignThatWondDisapear);
		newSign.NormalSignInitalize();
	}


	/////////////////////////////////////////////////////////////   Make normal sign    /////////////////////////////////////////////////////////////
	public void MakeNormalSignSign(ImaginationSign SignThatWondDisapear)
	{
		ImaginationSign newSign = MakeSignInARandomLocation(SignThatWondDisapear);
		newSign.NormalSignInitalize();
		ourheadManager.SubWorkerMessageSignWasSuccefullyMade(newSign);
	}


	 /////////////////////////////////////////////////////////////////////////////////////////   MakeInRandom    /////////////////////////////////////////////////////////////
	        ImaginationSign MakeSignInARandomLocation(ImaginationSign signToShow)
	{
		///////////////////////////////////////////////////////////// make sign
		ImaginationSign spawnedSign = SubFunSignFabricator(signToShow);                                           //make sign
		RectTransform transofrmOfNewSign = spawnedSign.gameObject.GetComponent<RectTransform>();
		Vector2 pecentageOfSignOfCanvas = new Vector2((transofrmOfNewSign.sizeDelta.x / 2) / ourCanvasRectTransform.sizeDelta.x, (transofrmOfNewSign.sizeDelta.y / 2) / ourCanvasRectTransform.sizeDelta.y);

		/////////////////////////////////////////////////////////////
		/*	int numberOfTimesPostionWasReconsidered = 0;                            //check position reconsideration itiration
		restartPointToReCpnsiderPosition:  
			numberOfTimesPostionWasReconsidered += 1;
			if (numberOfTimesPostionWasReconsidered > 15)
			{
				Debug.Log("counted too many times");
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

					listOfMadeSignsDImensionsLeft.Add(spawnedSign, pixelRangeOccupiedBySignLeft);
					break;


				case screenLocation.button:
					int positionOfSignInVectorButton = (int)(ourCanvasRectTransform.sizeDelta.x * transofrmOfNewSign.anchorMin.x);          //save dimension occupied
					Vector2 pixelRangeOccupiedBySignButton = new Vector2(positionOfSignInVectorButton - (transofrmOfNewSign.sizeDelta.x / 2), positionOfSignInVectorButton + (transofrmOfNewSign.sizeDelta.x / 2));

					//check current sign with privoius sings for collisions
					if (!checkLane(pixelRangeOccupiedBySignButton, listOfMadeSignsDImensionsButton))
					{
						goto restartPointToReCpnsiderPosition;
					}
					listOfMadeSignsDImensionsButton.Add(spawnedSign, pixelRangeOccupiedBySignButton);
					break;


				case screenLocation.right:
					int positionOfSignInVectorRight = (int)(ourCanvasRectTransform.sizeDelta.y * transofrmOfNewSign.anchorMin.y);          //save dimension occupied
					Vector2 pixelRangeOccupiedBySignRight = new Vector2(positionOfSignInVectorRight - (transofrmOfNewSign.sizeDelta.y / 2), positionOfSignInVectorRight + (transofrmOfNewSign.sizeDelta.y / 2));

					if (!checkLane(pixelRangeOccupiedBySignRight, listOfMadeSignsDImensionsRight))
					{
						goto restartPointToReCpnsiderPosition;
					}

					listOfMadeSignsDImensionsRight.Add(spawnedSign, pixelRangeOccupiedBySignRight);
					break;
			}*/

		TryToPlaceSignAndDestrouItIfYouTryTooMantTimes( spawnedSign, transofrmOfNewSign, pecentageOfSignOfCanvas, 15);
		return spawnedSign;
	}                                         //         <<<--------------------------------------------------------------

	           void TryToPlaceSignAndDestrouItIfYouTryTooMantTimes( ImaginationSign singToPositon, RectTransform transformOfSign, Vector2 PecrentageOfSignToCanvas,int numberOfTimesToTry)
	{
		int numberOfTimesWeTriedToPlaceSign = 0;

		bool SignHasBeenPlaceWell = false;
		while (SignHasBeenPlaceWell == false)
		{
			SignHasBeenPlaceWell = TryToPlaceThisSignRandomlyAndIfItsWrondReturnFalse( singToPositon, transformOfSign, PecrentageOfSignToCanvas);
			if (SignHasBeenPlaceWell)
			{
				transformOfSign.anchoredPosition3D = Vector3.zero;
				break;
			}
			else if (!SignHasBeenPlaceWell)
			{
				numberOfTimesWeTriedToPlaceSign += 1;
			}

			if (numberOfTimesWeTriedToPlaceSign > numberOfTimesToTry)
			{
				Destroy(singToPositon.gameObject);				
				break;
			}
		}
	}

	                 bool TryToPlaceThisSignRandomlyAndIfItsWrondReturnFalse(ImaginationSign singToPositon,RectTransform transformOfSign,Vector2 PecrentageOfSignToCanvas)
	{
		screenLocation whereToPLaceSign = (screenLocation)Random.Range(0, 3);             //randomly pick a (right,button,lift)then randomly place sign in it
		switch (whereToPLaceSign)
		{
			case screenLocation.left:
				transformOfSign.anchorMin = new Vector2(0 + PecrentageOfSignToCanvas.x, Random.Range(PecrentageOfSignToCanvas.y, 1 - PecrentageOfSignToCanvas.y));

				break;
			case screenLocation.button:
				transformOfSign.anchorMin = new Vector2(Random.Range(PecrentageOfSignToCanvas.x * 3, 1 - (PecrentageOfSignToCanvas.x * 3)), PecrentageOfSignToCanvas.y);
				break;
			case screenLocation.right:
				transformOfSign.anchorMin = new Vector2(1 - PecrentageOfSignToCanvas.x, Random.Range(PecrentageOfSignToCanvas.y, 1 - PecrentageOfSignToCanvas.y));
				break;
		}
		transformOfSign.anchorMax = transformOfSign.anchorMin;

		
		/////////////////////////////////////////////////////////////
		//check sign if its touching another then rechoose another position
		switch (whereToPLaceSign)
		{
			case screenLocation.left:
				int positionOfSignInVectorLeft = (int)(ourCanvasRectTransform.sizeDelta.y * transformOfSign.anchorMin.y);          //save dimension occupied
				Vector2 pixelRangeOccupiedBySignLeft = new Vector2(positionOfSignInVectorLeft - (transformOfSign.sizeDelta.y / 2), positionOfSignInVectorLeft + (transformOfSign.sizeDelta.y / 2));

				if (!checkLane(pixelRangeOccupiedBySignLeft, listOfMadeSignsDImensionsLeft))
				{
					return false;
				}

				listOfMadeSignsDImensionsLeft.Add(singToPositon, pixelRangeOccupiedBySignLeft);
				return true;


			case screenLocation.button:
				int positionOfSignInVectorButton = (int)(ourCanvasRectTransform.sizeDelta.x * transformOfSign.anchorMin.x);          //save dimension occupied
				Vector2 pixelRangeOccupiedBySignButton = new Vector2(positionOfSignInVectorButton - (transformOfSign.sizeDelta.x / 2), positionOfSignInVectorButton + (transformOfSign.sizeDelta.x / 2));

				//check current sign with privoius sings for collisions
				if (!checkLane(pixelRangeOccupiedBySignButton, listOfMadeSignsDImensionsButton))
				{
					return false;
				}
				listOfMadeSignsDImensionsButton.Add(singToPositon, pixelRangeOccupiedBySignButton);
				return true;


			case screenLocation.right:
				int positionOfSignInVectorRight = (int)(ourCanvasRectTransform.sizeDelta.y * transformOfSign.anchorMin.y);          //save dimension occupied
				Vector2 pixelRangeOccupiedBySignRight = new Vector2(positionOfSignInVectorRight - (transformOfSign.sizeDelta.y / 2), positionOfSignInVectorRight + (transformOfSign.sizeDelta.y / 2));

				if (!checkLane(pixelRangeOccupiedBySignRight, listOfMadeSignsDImensionsRight))
				{
					return false;
				}

				listOfMadeSignsDImensionsRight.Add(singToPositon, pixelRangeOccupiedBySignRight);
				return true;
		}
		return false;
	}
	                          bool checkLane(Vector2 rangeOfMainSign, Dictionary<ImaginationSign,Vector2> listOfPReviousSigns)
	{
		foreach (Vector2 pixelRangeOfPReviouslyLaidSigns in listOfPReviousSigns.Values)
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
		
		return true;
	}


	         ImaginationSign SubFunSignFabricator(ImaginationSign signToFabricate)
	{
		ImaginationSign spawnedSign = Instantiate(signToFabricate, ourCanvasToDrawOn.transform);
		spawnedSign.ourHeadTeEchnicalDrawer = this;
		

		//InformOurHeadManagerThatWeHaveMadeThisSignSuccesfully(signToFabricate);

		return spawnedSign;
	}
	               void InformOurHeadManagerThatWeHaveMadeThisSignSuccesfully(ImaginationSign signMade)
	{
		ourheadManager.SubWorkerMessageSignWasSuccefullyMade(signMade);
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
		openingSign.ourCanvasWeAreDrawOn = ourCanvasToDrawOn;
		
		
		
	}                                              //         <<<--------------------------------------------------------------


	/////////////////////////////////////////////////////////////    Sign Making and destoying information      /////////////////////////////////////////////////////////////


	
	//I///////////////////////////////////////////////////////////     On signdestoyed      /////////////////////////////////////////////////////////////
	public void SubWorkerMessageSignIsGoingToLeave(ImaginationSign signThatWillBeLeaving)
	{
		CheckIfSignRemovedIsHThisLane(signThatWillBeLeaving, listOfMadeSignsDImensionsLeft);
		CheckIfSignRemovedIsHThisLane(signThatWillBeLeaving, listOfMadeSignsDImensionsButton);
		CheckIfSignRemovedIsHThisLane(signThatWillBeLeaving, listOfMadeSignsDImensionsRight);
		Debug.Log("Drawerrecieved that "+ signThatWillBeLeaving+" is leaving.");
		ourheadManager.SubWorkerMessageSignIsGoingToLeave(signThatWillBeLeaving);
	}
	           void CheckIfSignRemovedIsHThisLane(ImaginationSign signThatWillBeLeaving, Dictionary<ImaginationSign, Vector2> laneList)
	{
		foreach (ImaginationSign sign in laneList.Keys)
		{
			if (signThatWillBeLeaving == sign)
			{
				laneList.Remove(sign);
				break;
			}
		}
	}
}
