using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum LanguagesInGame { English, Arabic }
[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(ArabicFixerTMPRO))]
public class LanguageChanger : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public string EnglishText;
	public string ArabicText;
	
	public TMP_FontAsset EnglishFont;
	public TMP_FontAsset ArabicFont;

	///////////////////////////////////////////////////////////// Auto connections
	TextMeshProUGUI ourTMProToChange;
	ArabicFixerTMPRO ourArabicFixerTMPRO;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void Awake()
	{
		ourTMProToChange = GetComponent<TextMeshProUGUI>();
		ourArabicFixerTMPRO = GetComponent<ArabicFixerTMPRO>();

		GameStateInformationProvider.LanguageChanged += ChangeLanguage;
	}

	private void Start()
	{
		ChangeLanguage(GameStateInformationProvider.currentLanguageSelected);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
			{
			GameStateInformationProvider.LanguageChanged(LanguagesInGame.English);
			GameStateInformationProvider.currentLanguageSelected =LanguagesInGame.English;
		}
		if (Input.GetKeyDown(KeyCode.Q)){
			GameStateInformationProvider.LanguageChanged(LanguagesInGame.Arabic);
			GameStateInformationProvider.currentLanguageSelected = LanguagesInGame.Arabic;
		}
	}

	void ChangeLanguage(LanguagesInGame languageToChangeInto)
	{
		switch (languageToChangeInto)
		{
			case LanguagesInGame.English:
				ourTMProToChange.font = EnglishFont;
				ourTMProToChange.text = EnglishText;
				ourTMProToChange.horizontalAlignment = HorizontalAlignmentOptions.Left;
				break;
			case LanguagesInGame.Arabic:
			
				ourTMProToChange.font = ArabicFont;
			ourTMProToChange.text = ArabicText;
				ourArabicFixerTMPRO.FixArabicText();
				ourTMProToChange.horizontalAlignment = HorizontalAlignmentOptions.Right;
			break;
		}



	}

	private void OnDestroy()
	{
		GameStateInformationProvider.LanguageChanged -= ChangeLanguage;
	}
}
