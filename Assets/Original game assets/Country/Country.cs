using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class Country : MonoBehaviour                    //holds informatin of country area and colour also
														//its initial sighn stats which is given by game maker  and it makes it's sighns at start of game   
{
	[Header("Properties")]
	public Color colorOfCountrysProduction;          



	[Header("interpart connections")]
	public GameObject thisCountriesArea;



	//  memory
		[HideInInspector]
	public Need ourmainSign;
	public Dictionary<Needs, int> ourCountriesNeedsAndTheirValue = new Dictionary<Needs, int>(); //////comesfrom gameManager  // for initial sighn set
	List<Need> ourCountriesSigns = new List<Need>();


	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	
	public void ForighnOrderByGameMakerToStartMakingSigns()
	{
		
		StartFunMakeSighnsAndGiveThemInitialValueInRandomAreaInsideCountry(ourCountriesNeedsAndTheirValue);   //make the sighns
		StartFunMakeCountryMainSighn();
		//StarFunForignOrderRecalculateMainSignPercent();
	}

	void StartFunMakeCountryMainSighn()
	{
		Need mainSign = makeSighn(false);
		
		mainSign.SetAsMainSign(SubFunCalculateMainSignPercent());

		ourmainSign = mainSign;
	}
	/////////////////////////////////////////////////////////////
	void StartFunMakeSighnsAndGiveThemInitialValueInRandomAreaInsideCountry(Dictionary<Needs,int> requiredNeedsAndThierValues)   //foreach need in the dictionary make a sighn with its corresponding value
    {
        foreach (var needAndThereValue in requiredNeedsAndThierValues)
        {
			Need newSighn = makeSighn(true);  //here make the sighn of the need

			//SubFunSetSighnInitialColour(newSighn);           //then feed that sighn into these two value setters			
			newSighn.SetInitialNeedSighnData(needAndThereValue.Key, needAndThereValue.Value);
			ourCountriesSigns.Add(newSighn);
			newSighn.gameObject.SetActive(false);
		}

	}


	Need makeSighn(bool Inrandompoint)                                //pichs a random spot in area and instantiates the universal sighn in it.
	{
		Vector3 sighnPosition;

		if (Inrandompoint)
		{
			sighnPosition = SubSubFunCalculateSighnPosition();
		}
		else
		{
			sighnPosition = this.transform.position;
		}
		
		
		GameObject newSighn = Instantiate(GameMaker.needSighnTemplate, sighnPosition, Quaternion.identity);
		newSighn.transform.parent = this.transform;

		Need newObjectNeedComponent = newSighn.GetComponent<Need>();
		newObjectNeedComponent.ourCountryParent = this;
	

		return newObjectNeedComponent;
	}
	Vector3 SubSubFunCalculateSighnPosition()
	{
		Vector3 scaleOfCountryAreaHalfed = thisCountriesArea.transform.localScale / 2;

		Vector3 RotatedOffsetFromArea = Quaternion.Euler(thisCountriesArea.transform.localRotation.eulerAngles) *
			new Vector3(Random.Range(-scaleOfCountryAreaHalfed.x, scaleOfCountryAreaHalfed.x), Random.Range(-scaleOfCountryAreaHalfed.y, scaleOfCountryAreaHalfed.y), 0);

		Vector3 sighnPosition = thisCountriesArea.transform.position + RotatedOffsetFromArea;

		return sighnPosition;
	}


	/////////////////////////////////////////////////////////////

	public void StarFunForignOrderRecalculateMainSignPercent(Color colorOfNewMeterCounty)
	{
		int valueOfSign = SubFunCalculateMainSignPercent();
		ourmainSign.ForignOrderChangeMainSignValue(valueOfSign,colorOfNewMeterCounty);
	}
	int SubFunCalculateMainSignPercent()
	{
		float totalNumberOfImballance= 0;
		for(int currentSignIndex = 0; currentSignIndex <= ourCountriesSigns.Count - 1; currentSignIndex++)
		{
			Need signWeAreChecking = ourCountriesSigns[currentSignIndex];
			int currentValueOfInballance = Mathf.Abs(100 - signWeAreChecking.currentSignValue);
			totalNumberOfImballance += currentValueOfInballance;
		}
		float totatlNeedSunSignsValues = ourCountriesSigns.Count * 100;
		float missingPercent = totalNumberOfImballance/totatlNeedSunSignsValues;
		float FinalValueOfMainSign = 100 - (100*missingPercent);
		return (int)FinalValueOfMainSign;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public void ForighnOrderAskForANewPosition(Need sighnNeedsNewPosition)        // activated by any sub need telling the father country it's positiong is uncomfortable (colliding) and needs a new one
	{
		sighnNeedsNewPosition.transform.position = SubSubFunCalculateSighnPosition();
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	public void ForignOrderMouseEnterHoverOverCountry()
	{
		ourmainSign.gameObject.SetActive(false);
		for(int signs = 0;signs <=ourCountriesSigns.Count-1;signs++)
		{
			ourCountriesSigns[signs].gameObject.SetActive(true);
		}
	}
	////////////////////////////////////////////////////////////////////
	public void ForignOrderMouseExitedHoverOverCountry()
	{	
		for (int signs = 0; signs <= ourCountriesSigns.Count - 1; signs++)
		{
			ourCountriesSigns[signs].gameObject.SetActive(false);
		}
		ourmainSign.gameObject.SetActive(true);
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}

