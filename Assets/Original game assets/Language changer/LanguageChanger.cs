using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum LanguagesInGame { English, Arabic }
[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(ArabicFixerTMPRO))]

//GameStateInformationProvider.needsNames.Add("ss", "ss");
public class LanguageChanger : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	
	///////////////////////////////////////////////////////////// Properties
	public string EnglishText;
	public string ArabicText;

	public bool allighnWordsInMiddle;

	///////////////////////////////////////////////////////////// Asset refrence
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



	public void ChangeLanguage(LanguagesInGame languageToChangeInto)
	{
		switch (languageToChangeInto)
		{
			case LanguagesInGame.English:
				ourTMProToChange.font = EnglishFont;
				ourTMProToChange.text = EnglishText;

				if (allighnWordsInMiddle)
				{
					ourTMProToChange.horizontalAlignment = HorizontalAlignmentOptions.Center;
				}
				else
				{
					ourTMProToChange.horizontalAlignment = HorizontalAlignmentOptions.Left;
				}
				
				break;
			case LanguagesInGame.Arabic:
			
				ourTMProToChange.font = ArabicFont;
			ourTMProToChange.text = ArabicText;

				if (allighnWordsInMiddle)
				{
					ourTMProToChange.horizontalAlignment = HorizontalAlignmentOptions.Center;
				}
				else
				{
					ourTMProToChange.horizontalAlignment = HorizontalAlignmentOptions.Right;
				}

				ourArabicFixerTMPRO.FixArabicText();
			break;
		}



	}

	private void OnDestroy()
	{
		GameStateInformationProvider.LanguageChanged -= ChangeLanguage;
	}
}
