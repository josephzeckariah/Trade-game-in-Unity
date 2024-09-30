using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiChangeManager : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public Canvas canvasToDrawON;
	
	[Header("Asset refrence")]
	public GameObject openingUi;
	public GameObject endingUi;
	public GameObject creditsUi;

	[Header("Manual connection")]
	public GameObject transparentBackground;

	/////////////////////////////////////////////////////////////           Memory
	GameObject openingUiCreated;
	GameObject endingUiCreated;
	GameObject creditsUiCreated;

	GameObject transparentBackgroundCreated;






	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//I///////////////////////////////////////////////////////////     Initalize       /////////////////////////////////////////////////////////////

	private void Awake()
	{
		GameStateInformationProvider.GameStarted += GameStartReaction;
		GameStateInformationProvider.GameEnded += GameEndReaction;
		GameStateInformationProvider.NormalGameStart += InGameStartReaction;
	}




	//I///////////////////////////////////////////////////////////     Game Start       /////////////////////////////////////////////////////////////

	void GameStartReaction()                                                                            //         <<<--------------------------------------------------------------
	{
		MakeTransparentBackgoundFirstTime();
		MakeOpeningSign();
	}

	               void MakeTransparentBackgoundFirstTime()
	{
		transparentBackgroundCreated = Instantiate(transparentBackground, canvasToDrawON.transform);
		//transparentBackgroundCreated.GetComponent<TransparentBcgroundBehaviour>().ourCanvasToFitIn = canvasToDrawON;
	}
	               void MakeOpeningSign()
	{
		/////////////////////////////////////////////////////////////        Make sign
		openingUiCreated = Instantiate(openingUi, canvasToDrawON.transform);

		
		RectTransform transofrmOfNewSign = openingUiCreated.gameObject.GetComponent<RectTransform>();


		/////////////////////////////////////////////////////////////       Position Sign and achor
		transofrmOfNewSign.anchorMax = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchorMin = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchoredPosition3D = Vector3.zero;

		/////////////////////////////////////////////////////////////      initalize sign
	//	spawnedSign.ourHeadTechnicalDrawer = this;
		//spawnedSign.ourCanvasWeAreDrawOn = canvasToDrawON;

	}





	//S///////////////////////////////////////////////////////////     In game Start        /////////////////////////////////////////////////////////////

	void InGameStartReaction()                                                               //         <<<--------------------------------------------------------------
	{
		Destroy(openingUiCreated.gameObject);
	    transparentBackgroundCreated.gameObject.SetActive(false);
	}





	//I///////////////////////////////////////////////////////////     Game End       /////////////////////////////////////////////////////////////

	void GameEndReaction()
	{
		transparentBackgroundCreated.SetActive(true);
		MoveFromEndScreenToCreditScreen();//MakeEndingSign();
	}
	           void MakeEndingSign()
	{
		/////////////////////////////////////////////////////////////        Make sign
		endingUiCreated = Instantiate(endingUi, canvasToDrawON.transform);
		RectTransform transofrmOfNewSign = endingUiCreated.gameObject.GetComponent<RectTransform>();


		/////////////////////////////////////////////////////////////       Position Sign and achor
		transofrmOfNewSign.anchorMax = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchorMin = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchoredPosition3D = Vector3.zero;

		/////////////////////////////////////////////////////////////      initalize sign
		//	spawnedSign.ourHeadTechnicalDrawer = this;
		//spawnedSign.ourCanvasWeAreDrawOn = canvasToDrawON;

	}



	//I///////////////////////////////////////////////////////////     Game End       /////////////////////////////////////////////////////////////
	void MoveFromEndScreenToCreditScreen()
	{
		//Destroy(endingUiCreated.gameObject);
		MakeCreditSigns();
	}
	                void MakeCreditSigns()
	{
		/////////////////////////////////////////////////////////////        Make sign
		creditsUiCreated = Instantiate(creditsUi, canvasToDrawON.transform);
		RectTransform transofrmOfNewSign = creditsUiCreated.gameObject.GetComponent<RectTransform>();


		/////////////////////////////////////////////////////////////       Position Sign and achor
		transofrmOfNewSign.anchorMax = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchorMin = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchoredPosition3D = Vector3.zero;

	}
}
