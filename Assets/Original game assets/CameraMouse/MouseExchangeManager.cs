using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseExchangeManager : MonoBehaviour      //the mouseExchangeManager and its child ExchangeMaker are two units working together,
													   //the mouseExchangeManager looks for  Need objects and after selecting two ,
													   //the mouseExchangeManager Calls the its coWorker ExchangeMaker to make an Exchange line connecting them.
{
	//Auto connections
	ExchangeMaker ourExchangeMaker;


	//private memory
	Exchange exchangeObjectBeingHeld;
	Coroutine ourLookingToStartExchangeCoroutine;    //the coroutines memorized so that they may be stoped if needed
	Coroutine ourLookingToEndExchangeCoroutine;

	private void Awake() 
	{
		ourExchangeMaker = GetComponentInChildren<ExchangeMaker>();
	}

	private void Start()
	{
		ourLookingToStartExchangeCoroutine = StartCoroutine(ContinousCheckForStartingAnExchange());  //upoun start be in (waiting to select first sign) state


	}




	IEnumerator ContinousCheckForStartingAnExchange()
	{
		yield return new WaitForSecondsRealtime(0.5f);      //wait to avoid glitches
		while (true)
		{
			SubIenumCheckInputToSelectASign(FunMod_CheckSighnToStartExchange, null); //continously checks mouse input andif fireed on top of an acceptable need switches to the ienumerator
			yield return null;
		}

	}

	IEnumerator ContinousCheckForCompletingAnExchange(Need secondSignHit)
	{
		yield return new WaitForSecondsRealtime(0.5f);     //wait to avoid glitches

		while (true)
		{
			SubIenumCheckInputToCancelExchangeCreation();
			SubIenumCheckInputToSelectASign(FunMod_CheckSighnToCompleteExchange, secondSignHit); //continously checks mouse input andif fireed on top of an acceptable need switches to the ienumerator
			yield return null;
		}
	}

	void SubIenumCheckInputToCancelExchangeCreation()
	{
		if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			exchangeObjectBeingHeld.ForighnOrderCancelLine();
			//GameObject.Destroy(exchangeObjectBeingHeld); 

			StopCoroutine(ourLookingToEndExchangeCoroutine); // stop current cycle 
			ourLookingToStartExchangeCoroutine = StartCoroutine(ContinousCheckForStartingAnExchange()); //and start next phase
		}
	}




	delegate void actionsTakesANeed(Need sighnHit,Need optionalNeedSelected);
	void SubIenumCheckInputToSelectASign(actionsTakesANeed actionToDoUpounNeedHit,Need optionalNeedForChecking)
	{
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
			RaycastHit hitInfo = new RaycastHit();

			if (Physics.Raycast(ray, out hitInfo))
			{
				if (hitInfo.collider.gameObject.TryGetComponent<Need>(out Need needOfSighnHit))
				{
					actionToDoUpounNeedHit(needOfSighnHit, optionalNeedForChecking);
				}
			}
		}
	}
	 

	void FunMod_CheckSighnToStartExchange(Need sighnHit,Need wontbeUsed)
	{
		if (sighnHit.currentSignValue != 100)
		{
			Exchange exchangeCreated = ourExchangeMaker.MakeTradeRoute(sighnHit, sighnHit.sighnNeedType);

			exchangeObjectBeingHeld = exchangeCreated;

			StopCoroutine(ourLookingToStartExchangeCoroutine);           // stop current cycle 
			ourLookingToEndExchangeCoroutine = StartCoroutine(ContinousCheckForCompletingAnExchange(sighnHit)); //and start next phase

			//Signal
			GameMaker.anEchangeStarted(sighnHit.sighnNeedType);
		}
	}
	void FunMod_CheckSighnToCompleteExchange(Need sighnHit,Need secondSignHit)
	{
		bool theExchangeIsNotWithTheSameSign = !sighnHit.Equals(secondSignHit);
		bool ValuesOfTwoSignsAreNotBothGreaterOrSmallerThan100 = Mathf.Sign(100-sighnHit.currentSignValue) != Mathf.Sign(100-secondSignHit.currentSignValue);
		bool secondSighnIsnotAlready100 = sighnHit.currentSignValue != 100;
		bool bothSignsAreOfTHeSameType = sighnHit.sighnNeedType == secondSignHit.sighnNeedType;

		bool allTheBoveBoolConditionsAreTrue = theExchangeIsNotWithTheSameSign && ValuesOfTwoSignsAreNotBothGreaterOrSmallerThan100 && secondSighnIsnotAlready100 && bothSignsAreOfTHeSameType;
		if (allTheBoveBoolConditionsAreTrue)      
		{
			exchangeObjectBeingHeld.GetComponent<Exchange>().ForighnOrderCompleteExchangeOnThisSign(sighnHit);

			StopCoroutine(ourLookingToEndExchangeCoroutine); // stop current cycle 

			ourLookingToStartExchangeCoroutine = StartCoroutine(ContinousCheckForStartingAnExchange()); //and start next phase

			//Signal
			GameMaker.anEchangeEnded(sighnHit.sighnNeedType);
		}
	}



}
