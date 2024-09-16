using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exchange : MonoBehaviour
{
	//auto properties
	[HideInInspector]
	public Needs exchangeNeed;
	[HideInInspector]
	public bool startSignIsBuying;

	[Header("Manual Connections")]
    [SerializeField]
    ExchangeLine ourline;
    [SerializeField]
    MovingObjectGenerator ourMovingObjectGenerator;

    //auto connected
    [HideInInspector]
    public Need exchangeStartingSign;
	[HideInInspector]
	public Need exchangeEndtingSign;



	//private memory
	Coroutine ourLineSetUpProcess;
	void Awake()
    {
		
	}

	private void Start()
	{
		StartFunDecideWhetherBuyingOrSelling();

		StartFunGiveSubWorkerThierInformation();


		ourLineSetUpProcess = StartCoroutine(LineSetUp()); //upoun spawning start the follow mouth procedure

		ourMovingObjectGenerator.ForighnOrderSetUpLineInformationAndStartSpawningObjects();
		//StartCoroutine(ourMovingObjectGenerator.ForighnOrderSpawnMovingElemnt()); //order subworker to start

	}

	void StartFunDecideWhetherBuyingOrSelling()
	{
		if (exchangeStartingSign.currentSignValue < 100)
		{
			startSignIsBuying = true;
		}
		else if (exchangeStartingSign.currentSignValue > 100)
		{
			startSignIsBuying = false;
		}
	}
	void StartFunGiveSubWorkerThierInformation()
	{
		ourMovingObjectGenerator.needToSpawn = exchangeNeed;
		ourMovingObjectGenerator.startSignIsBuying = startSignIsBuying;
	}
	

	IEnumerator LineSetUp()
    {
		//Start ien Make MOving elemnt

		while (true)
        {

			SubIenumUpdateSelf(SubValueInputFunGetLocationOfMouse());
            yield return null;
        }
    }

	private void SubIenumUpdateSelf(Vector3 locationToUpdateTO)
	{
		SubFunUpdateRotation(locationToUpdateTO);

		this.ourline.ForeighnOrderUpdateLine(locationToUpdateTO);
	}
    void SubFunUpdateRotation(Vector3 endPoint)
    {
        Vector3 locationDifference = endPoint - this.transform.position;
        float Angle = Mathf.Atan2(locationDifference.y, locationDifference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Angle));
    }
    Vector3 SubValueInputFunGetLocationOfMouse()
    {
		Vector3 locationUncorrected = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 correctedPos = new Vector3(locationUncorrected.x, locationUncorrected.y, 0);
        return correctedPos;
	} //gets mouse position ignoring z.


	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public void ForighnOrderCompleteExchangeOnThisSign(Need signToEndIn) //upoun ending exchange on this sighn
    {
        exchangeEndtingSign = signToEndIn;

		if (ourLineSetUpProcess != null)                // stopp following the mouse
		{
			StopCoroutine(ourLineSetUpProcess);
		}

		SubIenumUpdateSelf(signToEndIn.transform.position);  //and update the line to end in it

		SubFunCalculateAndModefySignsWeConnect();
		SubFunChangeAskCountriesOfBothSignsToRecalucateMainSign();

		this.gameObject.name += signToEndIn.ourCountryParent.name; //update name

	}
    void SubFunCalculateAndModefySignsWeConnect()
    {
        float smallestDifferenceFrom100InTwoSigns = Mathf.Min(Mathf.Abs(100-exchangeStartingSign.currentSignValue), Mathf.Abs(100-exchangeEndtingSign.currentSignValue));   //first we calculate the value which we will work with (the smallest dif. from 100)
        float valueToAddToTheCountries = smallestDifferenceFrom100InTwoSigns;

        if (exchangeStartingSign.currentSignValue < 100)
        {
            exchangeStartingSign.AddNewMeter((int)valueToAddToTheCountries, exchangeEndtingSign.ourCountryParent.colorOfCountrysProduction);
        }else if (exchangeStartingSign.currentSignValue > 100)
        {
			exchangeStartingSign.RemoveFromFirstMeterAndAddANew((int)valueToAddToTheCountries, exchangeEndtingSign.ourCountryParent.colorOfCountrysProduction);
			//exchangeStartingSign.AddNewMeter((int)valueToAddToTheCountries, exchangeEndtingSign.ourCountryParent.colorOfCountrysProduction);
		}


		if (exchangeEndtingSign.currentSignValue < 100)
		{
			exchangeEndtingSign.AddNewMeter((int)valueToAddToTheCountries, exchangeStartingSign.ourCountryParent.colorOfCountrysProduction);
		}
		else if (exchangeEndtingSign.currentSignValue > 100)
		{
			exchangeEndtingSign.RemoveFromFirstMeterAndAddANew((int)valueToAddToTheCountries, exchangeStartingSign.ourCountryParent.colorOfCountrysProduction);
			//exchangeEndtingSign.AddNewMeter((int)valueToAddToTheCountries, exchangeStartingSign.ourCountryParent.colorOfCountrysProduction);
		}
	}            //update the small signs
	void SubFunChangeAskCountriesOfBothSignsToRecalucateMainSign()      //update the big sign
	{
		exchangeStartingSign.ourCountryParent.StarFunForignOrderRecalculateMainSignPercent(exchangeEndtingSign.ourCountryParent.colorOfCountrysProduction);
		exchangeEndtingSign.ourCountryParent.StarFunForignOrderRecalculateMainSignPercent(exchangeStartingSign.ourCountryParent.colorOfCountrysProduction);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public void ForighnOrderCancelLine()
	{
		ourMovingObjectGenerator.ForighnOrderRemoveAllMovingElemnts();
		Destroy(this.gameObject);
	}

}
