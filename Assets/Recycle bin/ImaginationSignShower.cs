using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class ImaginationSignShower : MonoBehaviour
{
	//properites
	public Vector2 signDimensions = new Vector2(150, 150);
	//manual connections
	public OpeningSign openningScreenSign;

	public List<ImaginationSign> ListOfGeneralSigns = new List<ImaginationSign>();

	public List<ImaginationSign> ListOfFoodSigns = new List<ImaginationSign>();
	public List<ImaginationSign> ListOfMaterialseSigns = new List<ImaginationSign>();
	public List<ImaginationSign> ListOfElectronicsSigns = new List<ImaginationSign>();
	public List<ImaginationSign> ListOfMechanicsSigns = new List<ImaginationSign>();



	//public facts
	[HideInInspector]
	public RectTransform canvasRectTransform;


	//private memory
	List<ImaginationSign> ListOfSignsOnScreen = new List<ImaginationSign>();
	List<ImaginationSign> ListOfSignsLeftToShow = new List<ImaginationSign>();


	IEnumerator IenumeratorShowingGeneralSigns ;
	private void Awake()
	{
		canvasRectTransform = GetComponent<RectTransform>();
	//	GameMaker.anEchangeStarted += ExchangeWasMade;
	}



	public void ForignOrderShowOpeningSccreen()
	{
		OpeningSign openingSign =  Instantiate(openningScreenSign, this.transform);
		//openingSign.ourHeadSignShower = this;
	}

	public void ForignOrderOpeningSCreenClosed()
	{
		
		IenumeratorShowingGeneralSigns = (TimerShowSigns());
	
		StartCoroutine(IenumeratorShowingGeneralSigns);


	}


	float GeneralSignShowTimer = 0;
	IEnumerator CheckForTutorialStart()
	{
		while (true)
		{
			if (ListOfSignsLeftToShow.Contains(ListOfGeneralSigns[3]) == false) //if tutorial started
			{
				
				StopCoroutine(IenumeratorShowingGeneralSigns);
				while (true)
				{
					if(GameMaker.numberofExchangesCompleted >= 2)
					{
						StartCoroutine(IenumeratorShowingGeneralSigns);
						yield break;
					}
					yield return null;
				}
			}
			yield return null;
		}
	}
	IEnumerator TimerShowSigns()
	{
		foreach (ImaginationSign sign in ListOfGeneralSigns)
		{
			ListOfSignsLeftToShow.Add(sign);
		}
		StartCoroutine(CheckForTutorialStart());

		while (true)
		{
			//Debug.Log("ienum running at "+ GeneralSignShowTimer);
			if (ListOfSignsLeftToShow.Count == 0)
			{
				break;
			}

			if(GeneralSignShowTimer > 4f)
			{
				ShowSignInARandomLocation(ListOfSignsLeftToShow[0]);
				ListOfSignsLeftToShow.RemoveAt(0);
				GeneralSignShowTimer = 0;
			}

			GeneralSignShowTimer+= Time.deltaTime;
			yield return null;
		}
	}
	bool firstExchangeWasMade = false;
	void ExchangeWasMade()
	{
		if (firstExchangeWasMade == false)
		{
			//make exchange tutorial
			if (ListOfSignsLeftToShow.Contains(ListOfGeneralSigns[3]))
			{
				ShowSignInARandomLocation(ListOfGeneralSigns[3]);
				ListOfSignsLeftToShow.Remove(ListOfGeneralSigns[3]);
			}
			if (ListOfSignsLeftToShow.Contains(ListOfGeneralSigns[4]))
			{
				ShowSignInARandomLocation(ListOfGeneralSigns[4]);
				ListOfSignsLeftToShow.Remove(ListOfGeneralSigns[4]);		
			}
			GeneralSignShowTimer = 0;
			firstExchangeWasMade = true;
			return;
		}
		//show specific need content:
		//add to timer of general signs
		switch(GameStateInformationProvider.informationCurrentExchange.typeOfExchange)
		{
			case (Needs.Food):
				ShowSignInARandomLocation(ListOfFoodSigns[ Random.Range(0,ListOfFoodSigns.Count-1) ]);
				break;
			case (Needs.Materials):
				ShowSignInARandomLocation(ListOfMaterialseSigns[Random.Range(0, ListOfMaterialseSigns.Count - 1)]);
				break;
			case (Needs.Electronics):
				ShowSignInARandomLocation(ListOfElectronicsSigns[Random.Range(0, ListOfElectronicsSigns.Count - 1)]);
				break;
			case (Needs.Machines):
				ShowSignInARandomLocation(ListOfMechanicsSigns[Random.Range(0, ListOfMechanicsSigns.Count - 1)]);
				break;
		}
		GeneralSignShowTimer -= 2;

	}





	List<Vector2> listOfMadeSignsDImensionsLeft = new List<Vector2>();
	List<Vector2> listOfMadeSignsDImensionsButton = new List<Vector2>();
	List<Vector2> listOfMadeSignsDImensionsRight = new List<Vector2>();
	enum screenLocation { left, button, right };
	void ShowSignInARandomLocation(ImaginationSign signToShow)
	{

		ImaginationSign spawnedSign = SubFunSignFabricator(signToShow);
		RectTransform transofrmOfNewSign = spawnedSign.gameObject.GetComponent<RectTransform>();

		int numberOfTimesPostionWasReconsidered = 0;
		restartPointToReCpnsiderPosition:
		numberOfTimesPostionWasReconsidered += 1;
		if(numberOfTimesPostionWasReconsidered > 15)
		{			
			Destroy(spawnedSign.gameObject);
			return;
		}    //check reconsiderationOfPosition if toomany

		Vector2 pecentageOfSignOfCanvas =new Vector2((signDimensions.x/2)/canvasRectTransform.sizeDelta.x, (signDimensions.y / 2) / canvasRectTransform.sizeDelta.y);

		screenLocation whereToPLaceSign = (screenLocation)Random.Range(0, 3);
		switch (whereToPLaceSign)
		{
			case screenLocation.left:
				transofrmOfNewSign.anchorMin = new Vector2(0+pecentageOfSignOfCanvas.x, Random.Range(pecentageOfSignOfCanvas.y , 1 - pecentageOfSignOfCanvas.y));
				
				break;
			case screenLocation.button:
				transofrmOfNewSign.anchorMin = new Vector2(Random.Range(pecentageOfSignOfCanvas.x*2, 1 - (pecentageOfSignOfCanvas.x*2)), pecentageOfSignOfCanvas.y);
				break;
			case screenLocation.right:
				transofrmOfNewSign.anchorMin = new Vector2(1-pecentageOfSignOfCanvas.x, Random.Range(pecentageOfSignOfCanvas.y, 1 - pecentageOfSignOfCanvas.y));
				break;
		}
		transofrmOfNewSign.anchorMax = transofrmOfNewSign.anchorMin;

		/////////////////////////////////////////////////////////////
		//check sign if its touching another then rechoose another position
		switch (whereToPLaceSign)
		{
			case screenLocation.left:
				int positionOfSignInVectorLeft = (int)(canvasRectTransform.sizeDelta.y * transofrmOfNewSign.anchorMin.y);          //save dimension occupied
				Vector2 pixelRangeOccupiedBySignLeft = new Vector2(positionOfSignInVectorLeft - (signDimensions.y / 2), positionOfSignInVectorLeft + (signDimensions.y / 2));

				if (!checkLane(pixelRangeOccupiedBySignLeft, listOfMadeSignsDImensionsLeft))
				{
					goto restartPointToReCpnsiderPosition;
				}

				break;

			case screenLocation.button:
				int positionOfSignInVectorButton = (int)(canvasRectTransform.sizeDelta.x * transofrmOfNewSign.anchorMin.x);          //save dimension occupied
				Vector2 pixelRangeOccupiedBySignButton = new Vector2(positionOfSignInVectorButton - (signDimensions.x / 2), positionOfSignInVectorButton + (signDimensions.x / 2));

				//check current sign with privoius sings for collisions
				if (!checkLane(pixelRangeOccupiedBySignButton, listOfMadeSignsDImensionsButton))
				{
					goto restartPointToReCpnsiderPosition;
				}
				/*foreach(Vector2 pixelRangeOfPReviouslyLaidSigns in listOfMadeSignsDImensionsButton)
				{
					if(pixelRangeOccupiedBySign.x < pixelRangeOfPReviouslyLaidSigns.x)
					{
						if(pixelRangeOccupiedBySign.y < pixelRangeOfPReviouslyLaidSigns.x)
						{
							//all is fine continue
							continue;
						}
						else if (pixelRangeOccupiedBySign.y > pixelRangeOfPReviouslyLaidSigns.x)
						{
							
							//redraw
							goto restartPointToReCpnsiderPosition;						
						}
					}
					else if(pixelRangeOccupiedBySign.x > pixelRangeOfPReviouslyLaidSigns.x)
					{
						if (pixelRangeOccupiedBySign.x > pixelRangeOfPReviouslyLaidSigns.y)
						{
							//all is fine continue
							continue;
						}
						else if (pixelRangeOccupiedBySign.x < pixelRangeOfPReviouslyLaidSigns.y)
						{
							
							//redraw
							goto restartPointToReCpnsiderPosition;
						}
					}
					else if (pixelRangeOccupiedBySign.x == pixelRangeOfPReviouslyLaidSigns.x)
					{
						
						//redraw
						goto restartPointToReCpnsiderPosition;
					}
				}


				listOfMadeSignsDImensionsButton.Add(pixelRangeOccupiedBySign);*/
				break;
			case screenLocation.right:
				int positionOfSignInVectorRight = (int)(canvasRectTransform.sizeDelta.y * transofrmOfNewSign.anchorMin.y);          //save dimension occupied
				Vector2 pixelRangeOccupiedBySignRight = new Vector2(positionOfSignInVectorRight - (signDimensions.y / 2), positionOfSignInVectorRight + (signDimensions.y / 2));

				if (!checkLane(pixelRangeOccupiedBySignRight, listOfMadeSignsDImensionsRight))
				{
					goto restartPointToReCpnsiderPosition;
				}
				
				break;
		}




				transofrmOfNewSign.anchoredPosition3D = Vector3.zero;
	}


	bool checkLane(Vector2 rangeOfMainSign,List<Vector2> listOfPReviousSigns)
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
		ImaginationSign spawnedSign = Instantiate(signToFabricate, this.transform);
		//spawnedSign.ourHeadSignShower = this;

		ListOfSignsOnScreen.Add(spawnedSign);
		//KeepOnlyThreeSignsOnScreen();

		return spawnedSign;
	}

	void KeepOnlyThreeSignsOnScreen()
	{
		if(ListOfSignsOnScreen.Count > 3)
		{
			Destroy(ListOfSignsOnScreen[0].gameObject);
			ListOfSignsOnScreen.RemoveAt(0);
		}
	}

}
