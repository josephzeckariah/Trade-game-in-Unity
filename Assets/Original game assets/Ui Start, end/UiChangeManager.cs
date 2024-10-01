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
	public UiScreen openingUi;
	public UiScreen endingUi;
	public UiScreen creditsUi;

	[Header("Manual connection")]
	public UiScreen transparentBackground;

	/////////////////////////////////////////////////////////////           Memory
	UiScreen openingUiCreated;
	UiScreen endingUiCreated;
	UiScreen creditsUiCreated;

	UiScreen transparentBackgroundCreated;






	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//I///////////////////////////////////////////////////////////     Initalize       /////////////////////////////////////////////////////////////

	private void Awake()
	{
		GameStateInformationProvider.GameInitalize += GameStartReaction;
		GameStateInformationProvider.GameEnded += GameEndReaction;
	}


	//S///////////////////////////////////////////////////////////     Main Sub make screen mithod      /////////////////////////////////////////////////////////////
	void MakeScreen(UiScreen screenToMake, ref UiScreen refrenceToMadeScreen)
	{
		/////////////////////////////////////////////////////////////        Make sign
		refrenceToMadeScreen = Instantiate(screenToMake, canvasToDrawON.transform);
		refrenceToMadeScreen.ourUiChangeManager = this;

		RectTransform transofrmOfNewSign = refrenceToMadeScreen.gameObject.GetComponent<RectTransform>();


		/////////////////////////////////////////////////////////////       Position Sign and achor
		transofrmOfNewSign.anchorMax = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchorMin = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchoredPosition3D = Vector3.zero;

	}


	//I///////////////////////////////////////////////////////////     Game Start       /////////////////////////////////////////////////////////////

	void GameStartReaction()                                                                            //         <<<--------------------------------------------------------------
	{
		MakeScreen(transparentBackground, ref transparentBackgroundCreated);
		MakeScreen(openingUi,ref openingUiCreated);

		/*MakeTransparentBackgoundFirstTime();
		MakeOpeningSign();*/
	}

	/*               void MakeTransparentBackgoundFirstTime()
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

	}*/

	

	//S///////////////////////////////////////////////////////////     In game Start        /////////////////////////////////////////////////////////////

	public void NormalStartButtonClicked()
	{
		Destroy(openingUiCreated.gameObject);
		transparentBackgroundCreated.gameObject.SetActive(false);

		if (GameStateInformationProvider.NormalGameStart != null)
		{
			GameStateInformationProvider.NormalGameStart();                             //       ------------------------------------------------>>>
		}	
	}






	//I///////////////////////////////////////////////////////////     Game End       /////////////////////////////////////////////////////////////

	void GameEndReaction()                                                     //         <<<--------------------------------------------------------------
	{
		transparentBackgroundCreated.gameObject.SetActive(true);
		MakeScreen(endingUi,ref endingUiCreated);

		//MakeEndingSign();
	}
	



	//I///////////////////////////////////////////////////////////     Move From End Screen To CreditScreen       /////////////////////////////////////////////////////////////
	public void MoveFromEndScreenToCreditScreen()
	{
		Destroy(endingUiCreated.gameObject);
		MakeScreen(creditsUi,ref creditsUiCreated);
	}



	//S///////////////////////////////////////////////////////////     Start       /////////////////////////////////////////////////////////////

	public void MoveFromCreditScreenBackToStartScreenWithExplainer()
	{
		Destroy(creditsUiCreated.gameObject);
		//gameRefresh
		MakeScreen(openingUi, ref openingUiCreated);
		//makescreen explainer
	}

}
