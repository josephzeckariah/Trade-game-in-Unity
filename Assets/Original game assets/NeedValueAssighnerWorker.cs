using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedValueAssighnerWorker : MonoBehaviour
{   //this worker is told by his boss when to Act an action
	//it only action is to calculate and assighn a ballanced need values


	public void AssighnChoosenNeedsValueToChoosenCountries(List<Needs> choosenNeeds, List<Country> choosenCountries)
	{
		SubFunAssighnNeedsToEachCountryInABallancedNeg1_Zero_Pos1(choosenNeeds, choosenCountries);

		SubFunBallamceNeedsBetwenCountries(choosenNeeds, choosenCountries);

		SubFunAssighnNumberToNeedsInEachCountry(choosenNeeds, choosenCountries);
	}

	void SubFunAssighnNeedsToEachCountryInABallancedNeg1_Zero_Pos1(List<Needs> choosenNeeds, List<Country> choosenCountries)
	{

		foreach (Country choosenCountry in choosenCountries)
		{
			foreach (Needs need in choosenNeeds)
			{
				choosenCountry.ourCountriesNeedsAndTheirValue.Add(need, 0);                     //Assighn empty
			}
		}

		foreach (Country choosenCountry in choosenCountries)
		{
			Dictionary<Needs, int> copyOfCOuntiesList = new Dictionary<Needs, int>();
			foreach (var keyValueToBeCopied in choosenCountry.ourCountriesNeedsAndTheirValue)      //make a copy for tecknical reasons
			{
				copyOfCOuntiesList.Add(keyValueToBeCopied.Key, keyValueToBeCopied.Value);
			}

			foreach (var keyValueInCopiedList in copyOfCOuntiesList)
			{
				choosenCountry.ourCountriesNeedsAndTheirValue[keyValueInCopiedList.Key] = Random.Range(-1, 2);        //give a negatve ,0,positive to each need in each country
			}

			if (choosenNeeds.Count < 2)         //if needs are less than 2 then dont try to ballance stop
			{

				break;
			}


			foreach (var keyValueToBeCopied in choosenCountry.ourCountriesNeedsAndTheirValue)      //recopy new values into the copy list
			{
				copyOfCOuntiesList[keyValueToBeCopied.Key] = keyValueToBeCopied.Value;
			}



			Needs choosenNeedToCheckForBallance;
			foreach (var keyValueInCopiedList in copyOfCOuntiesList)                           // correct needs to have both negate and positive
			{
				if (keyValueInCopiedList.Value != 0)                                    //if there is an opposite
				{
					choosenNeedToCheckForBallance = keyValueInCopiedList.Key;                 //save it
					int oppositeNeededForBallance = 0 - keyValueInCopiedList.Value;           //know it opposite

					bool thereIsAnOpposite = false;
					foreach (var keyValueToCheckForBallance in choosenCountry.ourCountriesNeedsAndTheirValue)      //check for opposite
					{
						if (keyValueToCheckForBallance.Value == oppositeNeededForBallance)
						{
							thereIsAnOpposite = true;
							break;
						}
					}
					if (thereIsAnOpposite == false)          //if no opposite           // then choose another random need to be the opposite
					{
						List<int> enumCodesForReCorrection = new List<int>();
						foreach (var needToValue in choosenCountry.ourCountriesNeedsAndTheirValue)         //make a list of the country needs without the one we choose to check for opposite          
						{
							enumCodesForReCorrection.Add((int)needToValue.Key);
						}
						enumCodesForReCorrection.Remove((int)choosenNeedToCheckForBallance);

						int codeOfneedChoosenToCorrect = enumCodesForReCorrection[Random.Range(0, enumCodesForReCorrection.Count)];   // pick a enum int randomly for the need to play change to opposite
						choosenCountry.ourCountriesNeedsAndTheirValue[(Needs)codeOfneedChoosenToCorrect] = oppositeNeededForBallance;           //opposite it
					}
					break;
				}
			}

		}


	}
	//the following mithod is composed of 3 foreach loops that ballances the need values between countries
	void SubFunBallamceNeedsBetwenCountries(List<Needs> choosenNeeds, List<Country> choosenCountries)           
	{
		Dictionary<Needs, int> needsUnBallancedAndTheirNeededValue = new Dictionary<Needs, int>();        //make a list of needs that need to be ballanced across countries
		foreach (Needs needToCheckForBallance in choosenNeeds)                   //go through each need
		{
			int valueOfNeedToCheckIfBallanced;
			foreach (Country countryToCheckItsNeedValue in choosenCountries)        //and foreach country
			{
				if (countryToCheckItsNeedValue.ourCountriesNeedsAndTheirValue[needToCheckForBallance] != 0)      //if need has a value that needs to be ballanced
				{
					valueOfNeedToCheckIfBallanced = countryToCheckItsNeedValue.ourCountriesNeedsAndTheirValue[needToCheckForBallance];           //save this need
					int oppositeNeededForBallance = 0 - (valueOfNeedToCheckIfBallanced);
					foreach (Country countryToCheckForOpposite in choosenCountries)
					{
						if (countryToCheckForOpposite.ourCountriesNeedsAndTheirValue[needToCheckForBallance] == oppositeNeededForBallance)       //and for each country check if opposite exists
						{
							goto continueNextNeed;          //if exist skip thos neeed it's fine
						}
					}
					needsUnBallancedAndTheirNeededValue.Add(needToCheckForBallance, oppositeNeededForBallance);      //if not mark this need then continue to check next
					goto continueNextNeed;
				}
			}
		continueNextNeed:;

		}






		Dictionary<Country, List<Needs>> NeedsThatCanBeChangedInEachCountry = new Dictionary<Country, List<Needs>>();          //make a list of needs that can be changed in each country without deballancing the country.
		foreach (Country countryToCheckIfNeedCanBeChangedSafly in choosenCountries)
		{
			NeedsThatCanBeChangedInEachCountry.Add(countryToCheckIfNeedCanBeChangedSafly, new List<Needs>());

			List<Needs> needsInThisCountryThatHaveNeg1 = new List<Needs>();
			List<Needs> needsInThisCountryThatHavePos1 = new List<Needs>();
			foreach (Needs needToCheckIfItCanItBeChangedSafly in choosenNeeds)
			{
				int valueOfNeedInCountry = countryToCheckIfNeedCanBeChangedSafly.ourCountriesNeedsAndTheirValue[needToCheckIfItCanItBeChangedSafly];
				if (valueOfNeedInCountry == 0)
				{
					NeedsThatCanBeChangedInEachCountry[countryToCheckIfNeedCanBeChangedSafly].Add(needToCheckIfItCanItBeChangedSafly);
				}
				else if (valueOfNeedInCountry == -1)
				{
					needsInThisCountryThatHaveNeg1.Add(needToCheckIfItCanItBeChangedSafly);
				}
				else if (valueOfNeedInCountry == 1)
				{
					needsInThisCountryThatHavePos1.Add(needToCheckIfItCanItBeChangedSafly);

				}

			}
			if (needsInThisCountryThatHaveNeg1.Count > 1)
			{
				foreach (Needs needToAddBecausItsChangable in needsInThisCountryThatHaveNeg1)
				{
					NeedsThatCanBeChangedInEachCountry[countryToCheckIfNeedCanBeChangedSafly].Add(needToAddBecausItsChangable);
				}
			}
			if (needsInThisCountryThatHavePos1.Count > 1)
			{
				foreach (Needs needToAddBecausItsChangable in needsInThisCountryThatHavePos1)
				{
					NeedsThatCanBeChangedInEachCountry[countryToCheckIfNeedCanBeChangedSafly].Add(needToAddBecausItsChangable);
				}
			}
		}








		//the following loop fixed international unbalanced needs
		foreach (Needs needThatNeedsToBeBallanceInternationally in needsUnBallancedAndTheirNeededValue.Keys)           //fore each need unballanced internationally
		{
			List<Country> countriesThatCanChangeItsValueOfThisNeed = new List<Country>();

			foreach (Country countryToCheckIfItHaveOurNeedChangable in NeedsThatCanBeChangedInEachCountry.Keys)               //make alist of the countries that can change its value of this need.
			{
				if (NeedsThatCanBeChangedInEachCountry[countryToCheckIfItHaveOurNeedChangable].Contains(needThatNeedsToBeBallanceInternationally))
				{
					countriesThatCanChangeItsValueOfThisNeed.Add(countryToCheckIfItHaveOurNeedChangable);
				}
			}

			int valueNeededToBallanceNeed = needsUnBallancedAndTheirNeededValue[needThatNeedsToBeBallanceInternationally];         //
			int oppositeOfNeededValue = 0 - valueNeededToBallanceNeed;
			foreach (Country countryChangable in countriesThatCanChangeItsValueOfThisNeed)                     //check each country changable 
			{
		
				if (countryChangable.ourCountriesNeedsAndTheirValue[needThatNeedsToBeBallanceInternationally] != oppositeOfNeededValue)   //and if its not the orignal unballanced change it
				{
					countryChangable.ourCountriesNeedsAndTheirValue[needThatNeedsToBeBallanceInternationally] = valueNeededToBallanceNeed;
					NeedsThatCanBeChangedInEachCountry[countryChangable].Remove(needThatNeedsToBeBallanceInternationally);          //and remove the chagned need from list of need that can be changed
					
					foreach(Country countryWithChangable in NeedsThatCanBeChangedInEachCountry.Keys)
					{
						if(NeedsThatCanBeChangedInEachCountry[countryWithChangable].Contains(needThatNeedsToBeBallanceInternationally))           //and look for the original unballanced and if it was changable then remove it
						{
							if (countryChangable.ourCountriesNeedsAndTheirValue[needThatNeedsToBeBallanceInternationally] == oppositeOfNeededValue)
							{
								NeedsThatCanBeChangedInEachCountry[countryWithChangable].Remove(needThatNeedsToBeBallanceInternationally);
								break;
							}
						}
						
					}                                                                     
					goto ContinueWithoutZeroingEverything;
				}
			}
			foreach (Country country in choosenCountries)          //if no changable found just change the need in everyone to 0 
			{
				country.ourCountriesNeedsAndTheirValue[needThatNeedsToBeBallanceInternationally] = 0;
			}
		ContinueWithoutZeroingEverything:;
		}


	}
	void SubFunAssighnNumberToNeedsInEachCountry(List<Needs> choosenNeeds, List<Country> choosenCountries)
	{
	    
		foreach(Needs needToAssighnItsNumbers in choosenNeeds)              //for each need to through the process to assighn numbers
		{
			List<Country> numberOfCOuntriesWIthNegValueFOrTHisNeed = new List<Country>();
			List<Country> numberOfCOuntriesWIthPosValueFOrTHisNeed = new List<Country>();

			foreach(Country coutnryToCheckItsNeedSighn in choosenCountries)                                   //first calculate number of countries that have neg value to this need and the numberof countries wiht positive
			{
				if (coutnryToCheckItsNeedSighn.ourCountriesNeedsAndTheirValue[needToAssighnItsNumbers] == -1)
				{
					numberOfCOuntriesWIthNegValueFOrTHisNeed.Add(coutnryToCheckItsNeedSighn);
				}else if (coutnryToCheckItsNeedSighn.ourCountriesNeedsAndTheirValue[needToAssighnItsNumbers] == 1)
				{
					numberOfCOuntriesWIthPosValueFOrTHisNeed.Add(coutnryToCheckItsNeedSighn);
				}
			}
			//Debug.Log("numbberOfCOuntries with neg valie = " + numberOfCOuntriesWIthNegValueFOrTHisNeed.Count + "numbberOfCOuntries with positive valie = " + numberOfCOuntriesWIthPosValueFOrTHisNeed.Count + " in "+ needToAssighnItsNumbers);



			

			



			int sideWithLowerNumberOfCountriesParticipating = Mathf.Min(numberOfCOuntriesWIthNegValueFOrTHisNeed.Count, numberOfCOuntriesWIthPosValueFOrTHisNeed.Count);             //then gifure the side with lower number of countries neg or pos
			int possibeValueOfInbbalance = sideWithLowerNumberOfCountriesParticipating * 100;
			float possibeValueOfInbbalanceQuarted = (possibeValueOfInbbalance * (0.25f)) /10;

			int valueOfTotalInballanceIntheNeed = ((int)Random.Range(possibeValueOfInbbalanceQuarted, possibeValueOfInbbalanceQuarted * 3))* 10 ;    //then calculate radnomly  a (totalvalueof unballance)important to distripute upoun the coutries
		
			//Debug.Log(needToAssighnItsNumbers + " will have an inballanceof " + valueOfTotalInballanceIntheNeed);



			Dictionary<Country,int> ourDistributionOfeachNeedToEachCountry = new Dictionary<Country,int>();  //make a memory holder to store your distribution plan before sending them
			foreach(Country country in choosenCountries)
			{
				
				ourDistributionOfeachNeedToEachCountry.Add(country, 0);
			}                                                                          //////////////



		                                                               //Start distibuting the inballance on the negative side then the positive side
			SubSubFunDistribute_thisValueOfInbalance_upoun_theseCounters(valueOfTotalInballanceIntheNeed, numberOfCOuntriesWIthNegValueFOrTHisNeed,-1,ref ourDistributionOfeachNeedToEachCountry);
			SubSubFunDistribute_thisValueOfInbalance_upoun_theseCounters(valueOfTotalInballanceIntheNeed, numberOfCOuntriesWIthPosValueFOrTHisNeed,1,ref ourDistributionOfeachNeedToEachCountry);



			foreach(Country countryWeWillItsVAlue in ourDistributionOfeachNeedToEachCountry.Keys)         //finaly sen each country its assighned value;
			{
				countryWeWillItsVAlue.ourCountriesNeedsAndTheirValue[needToAssighnItsNumbers] = 100+ourDistributionOfeachNeedToEachCountry[countryWeWillItsVAlue];
				Debug.Log(countryWeWillItsVAlue + " will have a value of " + 100+ourDistributionOfeachNeedToEachCountry[countryWeWillItsVAlue] +" in "+ needToAssighnItsNumbers);
			}

		}
	}


	void SubSubFunDistribute_thisValueOfInbalance_upoun_theseCounters(int valueIfInballance,List<Country> countriesToDistributeUpoun,int inverementDirection_Neg1_Or_Pos1,ref Dictionary<Country,int> dictionaryofOutputValue)
	{

		int valueOfDistrubution;
		if (countriesToDistributeUpoun.Count < valueIfInballance / 10)      //a samll precaution here
		{
			valueOfDistrubution = 10;
		}
		else
		{
			valueOfDistrubution = 5;
		}



		int counterOfDistibution = valueIfInballance;        // the distribution counter is here
		while (counterOfDistibution > 0)
		{
			foreach (Country countryWithNegValue in countriesToDistributeUpoun)         //iterate through each country ,each time adding10 and removing 10 from our distripution
			{
				if (counterOfDistibution > 0)
				{
					dictionaryofOutputValue[countryWithNegValue] += valueOfDistrubution * inverementDirection_Neg1_Or_Pos1; //direction is for neg or pos value
					counterOfDistibution -= valueOfDistrubution;
				}
				else
				{
					break;
				}
			}
		}
		
	}


}
