using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArowOnScreenEdgeDrawer : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


	///////////////////////////////////////////////////////////// manual properties
	public GameObject ArrowPrefab;

	



	///////////////////////////////////////////////////////////// Auto connections
	Camera ourCameraToDrawArrowOnItsEdge;
	TutorialArrowManager ourHeadArrowManagerToGiveUsListOfHighlightedSigns;


	///////////////////////////////////////////////////////////// Memory
	///
	Vector2 worldUnitsSizeOfScreen;
	float distanceOfArrowFromEdge;
	float ScaleOfArrow;

	List<Need> listOfHighlightedSigns = new List<Need>();
	List<Need> listOfHighlightedSignsOutOfScreen = new List<Need>();


	Dictionary<Need,GameObject> dictionaryOfSignsAndTheirArrows = new Dictionary<Need,GameObject>();


	Coroutine ourArrowUpdatingCycle;
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


	//I///////////////////////////////////////////////////////////     Initalize       /////////////////////////////////////////////////////////////
	private void Awake()
	{
		GameStateInformationProvider.AnyGameStart += StartArrowMaking;
		GameStateInformationProvider.AllSigns100 += StopAndClearArrow;

		GameStateInformationProvider.ScreenSizeChanged += ScreenCHangedRecalculateScreenSizeAndArrowScaleDistanceAndScale;
		ourCameraToDrawArrowOnItsEdge = Camera.main;
	}


	//S///////////////////////////////////////////////////////////     Start  and end      /////////////////////////////////////////////////////////////
	void StartArrowMaking()
	{
		ScreenCHangedRecalculateScreenSizeAndArrowScaleDistanceAndScale(Vector2.left);

		ourArrowUpdatingCycle = StartCoroutine(UpdateArrowsOnEdge());
	}
	                  void ScreenCHangedRecalculateScreenSizeAndArrowScaleDistanceAndScale(Vector2 unused)
	{
		worldUnitsSizeOfScreen = new Vector2(ourCameraToDrawArrowOnItsEdge.orthographicSize * ourCameraToDrawArrowOnItsEdge.aspect, ourCameraToDrawArrowOnItsEdge.orthographicSize);

		float ScreenSideToUseForCalculation = MathF.Min(worldUnitsSizeOfScreen.x, worldUnitsSizeOfScreen.y);

		float percentOfDistanceAwayFromEdge = 0.8f;
		float DistanceAwayFromScreenEndge = ScreenSideToUseForCalculation - (ScreenSideToUseForCalculation * percentOfDistanceAwayFromEdge);
		distanceOfArrowFromEdge = DistanceAwayFromScreenEndge;

		float PercentageOfScale = 0.3f;
		Sprite Arrowimage = ArrowPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
		float SizeOfArrowInWorldUnit = Arrowimage.pixelsPerUnit / Arrowimage.rect.height;
		float percentageOfArrowFromSide = SizeOfArrowInWorldUnit / ScreenSideToUseForCalculation;
		float scaleOfArrow = PercentageOfScale / percentageOfArrowFromSide;
		ScaleOfArrow = scaleOfArrow;

		foreach (GameObject Arrow in dictionaryOfSignsAndTheirArrows.Values)
		{
			//Vector3 CameraPosition = ourCameraToDrawArrowOnItsEdge.transform.position;
			//Arrow.transform.position = Vector3.MoveTowards(Arrow.transform.position, new Vector3(CameraPosition.x, CameraPosition.y, 0), DistanceAwayFromScreenEndge);
			Arrow.transform.localScale = Vector3.one * scaleOfArrow;
		}
	}



	void StopAndClearArrow()
	{
		StopCoroutine(ourArrowUpdatingCycle);

		/////////////////////////////////////////////////////////////          make a mirror list to itirate through
		Dictionary<Need, GameObject> mirroeDictionaryOfSignsAndTheirArrows = new Dictionary<Need, GameObject>();
		foreach (KeyValuePair<Need, GameObject> value in dictionaryOfSignsAndTheirArrows)
		{
			mirroeDictionaryOfSignsAndTheirArrows.Add(value.Key, value.Value);
		}


		/////////////////////////////////////////////////////////////clean up
		foreach (Need sign in mirroeDictionaryOfSignsAndTheirArrows.Keys)
		{
			Destroy(dictionaryOfSignsAndTheirArrows[sign]);
			dictionaryOfSignsAndTheirArrows.Remove(sign);
		}
	}


	//I///////////////////////////////////////////////////////////     Get list of highlights       /////////////////////////////////////////////////////////////

	public void HigherUpMessageListBeenUpdated(List<Need> newListOfHighlightSigns)
	{
		listOfHighlightedSigns = newListOfHighlightSigns;

	
	}


	//I///////////////////////////////////////////////////////////     continously get and update list of highlighted signs out of the screen       /////////////////////////////////////////////////////////////

	IEnumerator UpdateArrowsOnEdge()
	{
		
		while(true)
		{
			if(CameraPositionCHanged__Or__ListOfSignToHighlightChanged())
			{
				GetListOfHighlightedSignsOUtOfScreen();
				UpdateArrowsCreatedAndCheckIfToCreateANew();
				CleanUpArrowOfSignNoLongerHighlightedOUtOfScreen();
			}

			yield return null;//new WaitForSecondsRealtime(TimeBetweenUpdates);
		}
	}



	/////////////////////////////////////////////////////////////
	     bool CameraPositionCHanged__Or__ListOfSignToHighlightChanged()
	{
		bool camerachanged = CameraPositionChanged();
		bool ListChanged = SignsToHighlightChanged();

		if (camerachanged || ListChanged)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	                Vector3 previousCameraPosition = Vector3.zero;
	                bool CameraPositionChanged()
	{
		if(ourCameraToDrawArrowOnItsEdge.transform.position != previousCameraPosition)
		{
			return true;
		}
		else
		{ 
			return false; 
		}
	}




	                List<Need> previousListOfSignsToHighlight = new List<Need>();
	                bool SignsToHighlightChanged()
	{
		if (listOfHighlightedSigns != previousListOfSignsToHighlight)
		{
			return true;
		}
		else
		{
			return false;
		}
	}



	/////////////////////////////////////////////////////////////

	                  void GetListOfHighlightedSignsOUtOfScreen() 
	{
		listOfHighlightedSignsOutOfScreen.Clear();
		foreach (Need sign in listOfHighlightedSigns)
		{
			if (sign.isActiveAndEnabled == true)
			{
				if (CheckIfSignIsOutOfCameraView(sign))
				{
					listOfHighlightedSignsOutOfScreen.Add(sign);
				}
			}

		}
	}
	                                          bool CheckIfSignIsOutOfCameraView(Need signToCheck)
	{
		Vector3 SignPosition = signToCheck.transform.position;
		Vector3 CameraPosition = Camera.main.transform.position;
		Vector2 DifferenceFromCameraPosition = new Vector2(MathF.Abs(SignPosition.x - CameraPosition.x), MathF.Abs(SignPosition.y - CameraPosition.y));

		if (DifferenceFromCameraPosition.x > worldUnitsSizeOfScreen.x || DifferenceFromCameraPosition.y > worldUnitsSizeOfScreen.y)
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	/////////////////////////////////////////////////////////////
	                          void UpdateArrowsCreatedAndCheckIfToCreateANew()
	{
		foreach (Need h in listOfHighlightedSignsOutOfScreen)
		{
			///////////////////////////////////////////////////////////// CheckIfToCreateANew
			if (dictionaryOfSignsAndTheirArrows.ContainsKey(h) == false)
			{
				GameObject Arrow = Instantiate(ArrowPrefab);
				Arrow.transform.localScale = Vector3.one * ScaleOfArrow;
				dictionaryOfSignsAndTheirArrows.Add(h ,Arrow);

				positionArrowRelativeToCamera(h);

				RotateArrowToPointTSign(h);


			}


			///////////////////////////////////////////////////////////// update arrow already created
			else if (dictionaryOfSignsAndTheirArrows.ContainsKey(h))
			{
				

				positionArrowRelativeToCamera(h);

				RotateArrowToPointTSign(h);



			}

			//Debug.Log(h.ourCountryParent.name +" has "+h.sighnNeedType.ToString()+" highlighted");
		}
	}

	                                       void positionArrowRelativeToCamera(Need signTopositionItsArrow)
	{
		/////////////////////////////////////////////////////////////            prepare calculation
		Vector3 SignPosition = signTopositionItsArrow.transform.position;
		Vector3 CameraPosition = Camera.main.transform.position;

		Vector2 DifferenceFromCameraPosition = new Vector2(SignPosition.x - CameraPosition.x, SignPosition.y - CameraPosition.y);
		Vector2 DifferenceFromCameraPositionAbsoluted = new Vector2(MathF.Abs(SignPosition.x - CameraPosition.x), MathF.Abs(SignPosition.y - CameraPosition.y));


		/////////////////////////////////////////////////////////////       calculate position based on these 3 ifs
		if (DifferenceFromCameraPositionAbsoluted.x > worldUnitsSizeOfScreen.x && DifferenceFromCameraPositionAbsoluted.y > worldUnitsSizeOfScreen.y)
		{
			Vector3 edgePosition = Camera.main.ViewportToWorldPoint(new Vector3(Mathf.InverseLerp(-1, 1, DifferenceFromCameraPosition.x / Mathf.Abs(DifferenceFromCameraPosition.x)),
				Mathf.InverseLerp(-1, 1, DifferenceFromCameraPosition.y / Mathf.Abs(DifferenceFromCameraPosition.y)), 0));
			//Vector3 vectorFromEdgeToSign = SignPosition - edgePosition;

			Vector3 offsetToCentre = (-DifferenceFromCameraPosition.normalized) * distanceOfArrowFromEdge *1.5f;
			dictionaryOfSignsAndTheirArrows[signTopositionItsArrow].transform.position = new Vector3(edgePosition.x,edgePosition.y,0) + offsetToCentre;

			/*if (vectorFromEdgeToSign.x > vectorFromEdgeToSign.y)
			{
				float ratio = vectorFromEdgeToSign.x / vectorFromEdgeToSign.y;
				float ratioAbs = Mathf.Abs(ratio);

				Vector3 offsetOfSign = new Vector3(0, -vectorFromEdgeToSign.y*ratioAbs, 0);

				dictionaryOfSignsAndTheirArrows[signTopositionItsArrow].transform.position =  edgePosition + offsetOfSign;
			}
			else if(vectorFromEdgeToSign.y > vectorFromEdgeToSign.x)
				{
				float ratio = vectorFromEdgeToSign.y / vectorFromEdgeToSign.x;
				float ratioAbs = Mathf.Abs(ratio);

				Vector3 offsetOfSign = new Vector3(-vectorFromEdgeToSign.x * ratioAbs, 0 , 0);

				dictionaryOfSignsAndTheirArrows[signTopositionItsArrow].transform.position = edgePosition + offsetOfSign;
			}
			else
			{
				dictionaryOfSignsAndTheirArrows[signTopositionItsArrow].transform.position = edgePosition;
			}
				//dictionaryOfSignsAndTheirArrows[signTopositionItsArrow].transform.position = Camera.main.ViewportToWorldPoint(new Vector3(Mathf.InverseLerp(-1, 1, DifferenceFromCameraPosition.x / Mathf.Abs(DifferenceFromCameraPosition.x)),
				//Mathf.InverseLerp(-1, 1, DifferenceFromCameraPosition.y / Mathf.Abs(DifferenceFromCameraPosition.y)), 0));*/
		}
		else if(DifferenceFromCameraPositionAbsoluted.x > worldUnitsSizeOfScreen.x)
		{
			float ratio = DifferenceFromCameraPosition.x/worldUnitsSizeOfScreen.x;
			float ratioAbs = Mathf.Abs(ratio);

			Vector3 positionToPlaceArrowOnScreenEdge = new Vector3(DifferenceFromCameraPosition.x / ratioAbs, DifferenceFromCameraPosition.y / ratioAbs, 0);
			Vector3 offsetToCentre = (-DifferenceFromCameraPosition.normalized) * distanceOfArrowFromEdge;
			dictionaryOfSignsAndTheirArrows[signTopositionItsArrow].transform.position = new Vector3(CameraPosition.x, CameraPosition.y, 0) + positionToPlaceArrowOnScreenEdge + offsetToCentre;
		}
		else if (DifferenceFromCameraPositionAbsoluted.y > worldUnitsSizeOfScreen.y)
		{
			float ratio = DifferenceFromCameraPosition.y / worldUnitsSizeOfScreen.y;
			float ratioAbs = Mathf.Abs(ratio);

			Vector3 positionToPlaceArrowOnScreenEdge = new Vector3(DifferenceFromCameraPosition.x / ratioAbs, DifferenceFromCameraPosition.y / ratioAbs, 0);
			Vector3 offsetToCentre = (-DifferenceFromCameraPosition.normalized) * distanceOfArrowFromEdge;
			dictionaryOfSignsAndTheirArrows[signTopositionItsArrow].transform.position = new Vector3 (CameraPosition.x, CameraPosition.y,0) + positionToPlaceArrowOnScreenEdge + offsetToCentre;
		}

	}

	                                       void RotateArrowToPointTSign(Need signToRotateItsArrow)
											  {
		Vector3 vectorFromSignToCamera = signToRotateItsArrow.transform.position - ourCameraToDrawArrowOnItsEdge.transform.position;
		float angleBetweenSignAndCamera = Mathf.Atan2(vectorFromSignToCamera.y,vectorFromSignToCamera.x) * Mathf.Rad2Deg;
		dictionaryOfSignsAndTheirArrows[signToRotateItsArrow].transform.eulerAngles = new Vector3(0,0, angleBetweenSignAndCamera-90);

	}


	/////////////////////////////////////////////////////////////
	                             void CleanUpArrowOfSignNoLongerHighlightedOUtOfScreen()
	{
		/////////////////////////////////////////////////////////////          make a mirror list to itirate through
		Dictionary<Need, GameObject> mirroeDictionaryOfSignsAndTheirArrows = new Dictionary<Need, GameObject>();
		foreach (KeyValuePair<Need, GameObject> value in dictionaryOfSignsAndTheirArrows)
		{
			mirroeDictionaryOfSignsAndTheirArrows.Add(value.Key, value.Value);
		}


		/////////////////////////////////////////////////////////////clean up
		foreach (Need sign in mirroeDictionaryOfSignsAndTheirArrows.Keys)
		{
			if (listOfHighlightedSignsOutOfScreen.Contains(sign))
			{
				continue;
			}
			else if (!listOfHighlightedSignsOutOfScreen.Contains(sign))
			{
				Destroy(dictionaryOfSignsAndTheirArrows[sign]);
				dictionaryOfSignsAndTheirArrows.Remove(sign);
			}
		}
	}



	/////////////////////////////////////////////////////////////

}
