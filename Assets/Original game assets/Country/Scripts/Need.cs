using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Need : MonoBehaviour            
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[Header("Communiaction Information")]
    public Needs sighnNeedType;	
	public int currentSignValue = 0;
	public bool locationIsUncomfortable = false;
	[HideInInspector]
	public bool isMainSign = false;

    [Header("Auto connections")]
	[HideInInspector]
	public Country ourCountryParent;

	[Header("interpart connections")]
	//Our Parts:
	public Image sighnImage;
	public Image sighnMeter;

	public LanguageChanger sighnNumberPercent;
	public LanguageChanger signWrittenWOrds;
	public LanguageChanger signCountryName;

	public Image sighnUpperBack;
	public GameObject metersParent;         //future use
	public GameObject ArrowForHighlighting;


    //private memory
    List<RectTransform> theMetersOfOurSign = new List<RectTransform>() ;



	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	//S///////////////////////////////////////////////////////////     Sign positioner Away from others       /////////////////////////////////////////////////////////////
	private void OnTriggerStay(Collider other)      //if sighn position is uncomfortable ask father country to for a new one
	{
		ourCountryParent.ForighnOrderAskForANewPosition(this);
		
	}




	//S///////////////////////////////////////////////////////////     Main sign set functions       /////////////////////////////////////////////////////////////
	public void SetAsMainSign( int initialValue)                                                                       //here is the functions to set initial data when sign is created
	{
		////////////////////////////////////////////////////////////// change image
		Sprite imageOfSighn = GeneralInformationProvider.Smile;
		this.sighnImage.sprite = imageOfSighn;                              

		///////////////////////////////////////////////////////////// name of sign
		signWrittenWOrds.allighnWordsInMiddle = true;
		signWrittenWOrds.EnglishText = "Satisfaction";
		signWrittenWOrds.ArabicText = "ألرفاهية";
		signWrittenWOrds.ChangeLanguage(GameStateInformationProvider.currentLanguageSelected);

		signCountryName.allighnWordsInMiddle = true;
		signCountryName.EnglishText = this.ourCountryParent.name;
		signCountryName.ArabicText = this.ourCountryParent.ArabicName;
		signCountryName.ChangeLanguage(GameStateInformationProvider.currentLanguageSelected);


		/////////////////////////////////////////////////////////////
		float valueOfCOlorDifference = 0.2f;	
		Color colorOfSign = new Color(ourCountryParent.colorOfCountrysProduction.r - valueOfCOlorDifference, ourCountryParent.colorOfCountrysProduction.g - valueOfCOlorDifference,
			ourCountryParent.colorOfCountrysProduction.b - valueOfCOlorDifference) ;
		this.sighnUpperBack.color = colorOfSign;
		//AddNewMeter(initialValue, colorOfSign / 2);

		ForignOrderChangeMainSignValue(initialValue, colorOfSign / 2);

		this.transform.localScale = Vector3.one * 1.5f;

		this.GetComponent<BoxCollider>().enabled = false;
		isMainSign = true;
	}                                                        
	public void SetInitialNeedSighnData(Needs sighnType,int initialValue)
    {
		
		Sprite imageOfSighn = GeneralInformationProvider.needsAssets[sighnType];
		this.sighnImage.sprite = imageOfSighn;                             //change image



		/////////////////////////////////////////////////////////////change name
		signWrittenWOrds.allighnWordsInMiddle = true;
		signWrittenWOrds.EnglishText = sighnType.ToString();
		signWrittenWOrds.ArabicText = GeneralInformationProvider.needsNamesAndTranslation[sighnType.ToString()];
		signWrittenWOrds.ChangeLanguage(GameStateInformationProvider.currentLanguageSelected);

		signCountryName.allighnWordsInMiddle = true;
		signCountryName.EnglishText = this.ourCountryParent.name;
		signCountryName.ArabicText = this.ourCountryParent.ArabicName;
		signCountryName.ChangeLanguage(GameStateInformationProvider.currentLanguageSelected);

		/////////////////////////////////////////////////////////////
		

		Image sighnUpperBackToChange = this.sighnUpperBack;
		sighnUpperBackToChange.color = ourCountryParent.colorOfCountrysProduction;     //change sign back color

		AddNewMeter(initialValue, ourCountryParent.colorOfCountrysProduction / 2);
        sighnNeedType = sighnType;                                  //save sighn type

	}





	//S///////////////////////////////////////////////////////////     Add and Remove meters       /////////////////////////////////////////////////////////////

	public void RemoveFromFirstMeterAndAddANew(float ValueToRemoveFromMetersInPositive,Color colorOfReplacedMeter)
	{
		float newSizeOfMeter = ((theMetersOfOurSign[0].sizeDelta.x / metersParent.GetComponent<RectTransform>().sizeDelta.x) * 100) - ValueToRemoveFromMetersInPositive;
		SubFunSetMeterSize(theMetersOfOurSign[0].gameObject, newSizeOfMeter);

		AddNewMeter((int)ValueToRemoveFromMetersInPositive, colorOfReplacedMeter);
		SubFunAddSignNumber(-(int)ValueToRemoveFromMetersInPositive * 2);  //2 times because adding a meter added a value
	}
	public void AddNewMeter(int meterPercent,Color meterColor)
	{
		GameObject newMeter = GameObject.Instantiate(sighnMeter.gameObject, metersParent.transform);
		newMeter.SetActive(true);

		newMeter.GetComponent<Image>().color = meterColor;

		SubFunSetMeterPosition(newMeter, meterPercent);
		SubFunSetMeterSize(newMeter, meterPercent);

		SubFunAddSignNumber(meterPercent);

		theMetersOfOurSign.Add(newMeter.GetComponent<RectTransform>());

	}

	                       void SubFunSetMeterPosition(GameObject meterToCHange, float valueTochangeInto)      //changes meter 
    {
		float lengthOfPreviousMeters = 0;

		if (theMetersOfOurSign.Count != 0)
		{
			 foreach(RectTransform previousMeter in theMetersOfOurSign)
             {
				lengthOfPreviousMeters += previousMeter.sizeDelta.x;
			 }
		}

		float pointToPlaceNewMeterAFterOffset = lengthOfPreviousMeters ;
		meterToCHange.transform.localPosition = new Vector3(pointToPlaceNewMeterAFterOffset, 0, 0);  //set meter position 
	}

	                       void SubFunSetMeterSize(GameObject meterToCHange, float valueTochangeInto)
	{
		float meterAt100Percent = metersParent.GetComponent<RectTransform>().sizeDelta.x;  //get total length from meter parent
		float currentMeterHight = sighnMeter.rectTransform.sizeDelta.y;
		float sizeOfInitialMeter = (valueTochangeInto / 100) * meterAt100Percent;

		meterToCHange.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeOfInitialMeter, currentMeterHight);  //set meter length


	}


	                       void SubFunAddSignNumber(int valueToAdd)
    {

        this.currentSignValue += valueToAdd;
        this.sighnNumberPercent.ArabicText= currentSignValue.ToString()+"%";
        this.sighnNumberPercent.EnglishText= currentSignValue.ToString()+"%";
		this.sighnNumberPercent.ChangeLanguage(GameStateInformationProvider.currentLanguageSelected);

	}





	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public void ForignOrderChangeMainSignValue(int valueTochangeInto,Color colorOfNewMeter)
	{
		int valueOfnewMeter = valueTochangeInto - currentSignValue;
		AddNewMeter(valueOfnewMeter, colorOfNewMeter);
		/*GameObject meterOfObject = this.theMetersOfOurSign[0].gameObject;

		SubFunSetMeterSize(meterOfObject, valueTochangeInto);
		currentSignValue = 0;
		SubFunAddSignNumber(valueTochangeInto);*/

		/*this.currentSignValue += valueTochangeInto;
		this.sighnNumberPercent.text = currentSignValue.ToString() + "%";*/
	}
}
