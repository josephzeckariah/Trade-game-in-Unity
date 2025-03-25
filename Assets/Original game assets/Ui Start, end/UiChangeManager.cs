using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UiChangeManager : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public Canvas canvasToDrawON;
	
	[Header("Asset refrence")]
	public UiScreen openingUi;
	public UiScreen endingUi;
	public CreditScreen creditsUi;
	public UiScreen endingAnimationUi;
	public IntroUiScreen IntroScreennUi;
	public LanguageChoosingScreen languageChoosingUi;
	public UiScreen newGamePLusIntroduction;

	[Header("Manual connection")]
	public UiScreen transparentBackground;

	/////////////////////////////////////////////////////////////           Memory
	UiScreen openingUiCreated;
	UiScreen endingUiCreated;
	CreditScreen creditsUiCreated;
	UiScreen endingAnimationUiCreated;
	IntroUiScreen IntroScreennUiCreated;
	LanguageChoosingScreen languageChoosingUiCreated;
	UiScreen newGamePLusIntroductionCreated;


	UiScreen transparentBackgroundCreated;






	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//I///////////////////////////////////////////////////////////     Initalize       /////////////////////////////////////////////////////////////

	private void Awake()
	{
		GameStateInformationProvider.GameInitalize += GameStartReaction;
		GameStateInformationProvider.GameEnded += GameEndReaction;
		GameStateInformationProvider.ZoomStarted += ShowEndingAnimationScreen;
	}


	               //S///////////////////////////////////////////////////////////     Main Sub make screen mithod      /////////////////////////////////////////////////////////////
	                   void MakeScreen(UiScreen screenToMake,ref  UiScreen refrenceToMadeScreen)
	{
		/////////////////////////////////////////////////////////////        Make sign
		refrenceToMadeScreen = Instantiate(screenToMake, canvasToDrawON.transform);
		refrenceToMadeScreen.ourUiChangeManager = this;

		RectTransform transofrmOfNewSign = refrenceToMadeScreen.gameObject.GetComponent<RectTransform>();


		/////////////////////////////////////////////////////////////       Position Sign and achor
		/*transofrmOfNewSign.anchorMax = new Vector2(0.5f, 0.5f);
		transofrmOfNewSign.anchorMin = new Vector2(0.5f, 0.5f);*/
		transofrmOfNewSign.localPosition = Vector3.zero;


	}
	                   void MakeScreen(CreditScreen screenToMake, ref CreditScreen refrenceToMadeScreen)
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
	                   void MakeScreen(LanguageChoosingScreen screenToMake, ref LanguageChoosingScreen refrenceToMadeScreen)
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

	                   void MakeScreen(IntroUiScreen screenToMake, ref IntroUiScreen refrenceToMadeScreen)
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


	                 //S///////////////////////////////////////////////////////////     SubFun bring ui to top       /////////////////////////////////////////////////////////////

	                  void SubFunBringUiToTop(UiScreen uiToBringToTop)
	{
		uiToBringToTop.transform.parent = null;
		uiToBringToTop.transform.SetParent(canvasToDrawON.transform);
		uiToBringToTop.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
	}


	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Game Start       ///////////////////////////

	//I///////////////////////////////////////////////////////////     Game Start       /////////////////////////////////////////////////////////////

	void GameStartReaction()                                                                            //         <<<--------------------------------------------------------------
	{
		MakeScreen(transparentBackground, ref transparentBackgroundCreated);
		MakeScreen(languageChoosingUi, ref languageChoosingUiCreated);
		

		/*MakeScreen(transparentBackground, ref transparentBackgroundCreated);
		MakeScreen(openingUi, ref openingUiCreated);*/
	}

	///////////////////////////////////////////////////////////// language buttons
	public void AnyLanguageBUttonWasClicked()
	{
		Destroy(languageChoosingUiCreated.gameObject);

		MakeScreen(openingUi, ref openingUiCreated);
	}



	//S///////////////////////////////////////////////////////////     opening screen buttons reactions        /////////////////////////////////////////////////////////////

	public void NormalStartButtonClicked()
	{
		Destroy(openingUiCreated.gameObject);
		MakeScreen(IntroScreennUi, ref IntroScreennUiCreated);
		IntroScreennUiCreated.willIntroduceMinGmae = true;
	}

	public void NewGamePlusButtonClicked ()
	{
		Destroy(openingUiCreated.gameObject);
		MakeScreen(IntroScreennUi, ref IntroScreennUiCreated);
		IntroScreennUiCreated.willIntroduceMinGmae = false;
	}

	public void OpeningScreenLanguageButtonClicked()
	{
		
		Destroy(openingUiCreated.gameObject);
		MakeScreen(languageChoosingUi, ref languageChoosingUiCreated);
	}





	//S///////////////////////////////////////////////////////////     intro start       /////////////////////////////////////////////////////////////

	public void IntroEnded()
	{
		Destroy(IntroScreennUiCreated.gameObject);
		transparentBackgroundCreated.gameObject.SetActive(false);

	
	}






	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Game end       ///////////////////////////


	//S///////////////////////////////////////////////////////////     Show end animation       /////////////////////////////////////////////////////////////

	void ShowEndingAnimationScreen()                                                                   //         <<<--------------------------------------------------------------
	{
		 MakeScreen(endingAnimationUi, ref endingAnimationUiCreated);
	}


	public void MoveFromStartScreenToCreditScreen()
	{
		Destroy(openingUiCreated.gameObject);
		MakeScreen(creditsUi, ref creditsUiCreated);
		creditsUiCreated.creditScreenType = CreditScreenType.openingCredit;
	}



	//ng= button

	//I///////////////////////////////////////////////////////////     Game End       /////////////////////////////////////////////////////////////

	void GameEndReaction()                                                     //         <<<--------------------------------------------------------------
	{
		Destroy(endingAnimationUiCreated.gameObject);
		transparentBackgroundCreated.gameObject.SetActive(true);
		MakeScreen(endingUi, ref endingUiCreated);

		//MakeEndingSign();
	}

	


	//I///////////////////////////////////////////////////////////    Open Credits       /////////////////////////////////////////////////////////////
	public void MoveFromEndScreenToCreditScreen()
	{
		Destroy(endingUiCreated.gameObject);
		MakeScreen(creditsUi,ref creditsUiCreated);
		creditsUiCreated.creditScreenType = CreditScreenType.EndCredit;
	}

	

	//S///////////////////////////////////////////////////////////     exit credit       /////////////////////////////////////////////////////////////

	public void MoveFromCreditScreenBackToStartScreenWithExplainer()
	{
		if(GameStateInformationProvider.currentGameType == GameStates.NormalGame)
		{
			Destroy(creditsUiCreated.gameObject);
			MakeScreen(openingUi, ref openingUiCreated);

			SubFunBringUiToTop(transparentBackgroundCreated);

			MakeScreen(newGamePLusIntroduction, ref newGamePLusIntroductionCreated);

		}
		else
		{
			Destroy(creditsUiCreated.gameObject);
			MakeScreen(openingUi, ref openingUiCreated);

		}



	}
	public void MoveFromCreditScreenBackToStartScreenWithoutAnyExplaner()
	{
		Destroy(creditsUiCreated.gameObject);
		MakeScreen(openingUi, ref openingUiCreated);
		
	}


	//S///////////////////////////////////////////////////////////     exit new game plus explainer       /////////////////////////////////////////////////////////////

	public void CloseNewGamePLusIntroduction()
	{
		Destroy(newGamePLusIntroductionCreated.gameObject);

		SubFunBringUiToTop(openingUiCreated);

	}
}
