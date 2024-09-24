using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	//manual properties
	[SerializeField]
	float timeBeforeTutorialStart = 10f;
	[SerializeField]
	float timeWaitedBeforeAnnouncingItsExchangeStateDidntChangeForTooLong = 10f;
	[SerializeField]
	float timeWaitedBeforeDecidingNoExchangewasMadeForAWhile = 16f;
	float timeBetweenChecks = 0.5f;

	//autoConnections
	TutorialArrowManager ourSignArrowHighlighter;

	//memory
	bool tutorialHasStarted;

	bool NoExchangeStartedForAwhile;
	bool ExchanheHeldForRooLong;

	bool NoExchangeEndedForTooLong;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	//I///////////////////////////////////////////////////////////     Initalize       /////////////////////////////////////////////////////////////
	private void Awake()
	{
		ourSignArrowHighlighter = this.GetComponentInChildren<TutorialArrowManager>();

		GameStateInformationProvider.OpeningScreenClosed += StartWorkers;

		GameStateInformationProvider.anEchangeEnded += GetIfExchangeHasEnded;

	}
	//S///////////////////////////////////////////////////////////     Start       ////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void StartWorkers()
	{
		StartCoroutine(WorkerToStartCountThenStartTutorial());
		StartCoroutine(WorkerToAlwaysCheckeIfExchangeDidntStartOrEndForAWhile(timeWaitedBeforeAnnouncingItsExchangeStateDidntChangeForTooLong));
		StartCoroutine(WorkerToDecideIfNoExchangeWasCompletedForTooLong(timeBetweenChecks, timeWaitedBeforeDecidingNoExchangewasMadeForAWhile));

	}
	            IEnumerator WorkerToStartCountThenStartTutorial()
	{
		yield return new WaitForSecondsRealtime(timeBeforeTutorialStart);
		//start  //send to sign to make tutorial signs
	}

	//OA///////////////////////////////////////////////////////////     Always cycle      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//S////////////////////////////////////////////////////     Worker to decide if exchange didnt start or end for too long    /////////////////////////////////////////////////////////////
	IEnumerator WorkerToAlwaysCheckeIfExchangeDidntStartOrEndForAWhile(float _timeWaitedBeforeAnnouncingItsExchangeStateDidntChangeForTooLong)
	{
		float StateTimer;
		///////////////////////////////////////////////////////////// exchange state chooser              //comes to this block every time an exchange statee changes , from here it goes to one of the folowwing two blocks
		                                   DecideWhereToGo:           //   <<--comehere
		StateTimer = 0;
		NoExchangeStartedForAwhile =false;
		ExchanheHeldForRooLong = false;

		if (GameStateInformationProvider.currentExchangeState == ExchangeMakingState.LookingToStart)
		{
			     goto CountingToSeeIfNothingStarted;         // goto--->>
		}
		else if(GameStateInformationProvider.currentExchangeState == ExchangeMakingState.LookingToEnd)
		{
			     goto CountingToSeeIfNothingEnded;           // goto--->>
		}
		//////////////////////


		///////////////////////////////////////////////////////////// While looking to StartExchange:
	                                  CountingToSeeIfNothingStarted:             //   <<--comehere
		while (GameStateInformationProvider.currentExchangeState == ExchangeMakingState.LookingToStart)
		{
			StateTimer += Time.deltaTime;
			if(StateTimer > _timeWaitedBeforeAnnouncingItsExchangeStateDidntChangeForTooLong)
			{
				NoExchangeStartedForAwhile = true;
			}

			yield return null;
		}
		          goto DecideWhereToGo;            // goto--->>
		//////////////////////
		
		///////////////////////////////////////////////////////////// while looking to end exchange:
		                                  CountingToSeeIfNothingEnded:              //   <<--comehere
	    while (GameStateInformationProvider.currentExchangeState == ExchangeMakingState.LookingToEnd)
		{
			StateTimer += Time.deltaTime;
			if (StateTimer > _timeWaitedBeforeAnnouncingItsExchangeStateDidntChangeForTooLong)
			{
				ExchanheHeldForRooLong = true;
			}

			yield return null;
		}
		         goto DecideWhereToGo;   // goto--->>
		//////////////////////
	}  //decide the thw bools didnt start and held for too long




	//S//////////////////////////////////////////////////// Worker to decide if no exchange completed for a while     /////////////////////////////////////////////////////////////
	void GetIfExchangeHasEnded(Needs needOfExchange)
	{
		anExchangeEnded = true;
	}  //provides if exchange ended to worker below
	bool anExchangeEnded;

	IEnumerator WorkerToDecideIfNoExchangeWasCompletedForTooLong(float timeBetweenChecks ,float timeBeforeTheyDecideThatItsTooLongWithoughtAnExchange)
	{
		yield return new WaitForSecondsRealtime(timeBeforeTutorialStart);      //wait till tutorial start then count
		/////////////////////////////////////////////////////////////
		float noExchangeCompletedTimer = 0;

		while(true)
		{
			/////////////////////////////////////////////////////////////
			if (noExchangeCompletedTimer > timeBeforeTheyDecideThatItsTooLongWithoughtAnExchange)
			{
				NoExchangeEndedForTooLong = true;
			}
			//////////////////////////////
			 if (anExchangeEnded == true)
			{
				noExchangeCompletedTimer = 0;
				NoExchangeEndedForTooLong = false;
				anExchangeEnded = false;
			}
			/////////////////////////////////////////////////////////////
			noExchangeCompletedTimer += timeBetweenChecks;
			yield return new WaitForSecondsRealtime(timeBetweenChecks);
		}
	}



	//S////////////////////////////////////////////////////    Final Action Decider     ///////////////////////////////////////////////////////////// decides highlight based on information by previous ienuemrators
	IEnumerator WorkerToConnectTheFactsAndDecideWhatToHighlight()
	{
		/////////////////////////////////////////////////////////////time before tutorial start
		while (true) 
		{
			if (tutorialHasStarted)
			{
				StartCoroutine(HighlightingTillTutorialEnds());
				break;
			}
			yield return null;
		}

		}
		IEnumerator HighlightingTillTutorialEnds()
		{
		while (tutorialHasStarted)
		{

			Debug.Log("Highlight all");
			Debug.Log("Highlight some");
			Debug.Log("Highlight nothing");

			if (NoExchangeStartedForAwhile)
			{
				Debug.Log("Highlight all");
			}
			else if (ExchanheHeldForRooLong)
			{
				Debug.Log("Highlight some");
			}

			if (NoExchangeEndedForTooLong) //check if to go to highlight till exchangeend cycle
			{
				yield return StartCoroutine(HighlightingTillAnExchangesGetsEnded());
			}

		}

		
		///////////////////////////////////////////////////////////// end
	}
	            IEnumerator HighlightingTillAnExchangesGetsEnded()
	{
		while (true) //no exchange ended
		{
			/*//if(State = starting exchange){
				ourSignArrowHighlighter all;
			}
			else if(state = end exchange)
			{
				 highlightsome of other need
			}*/
			yield return new WaitForSecondsRealtime(1);
		}
	}



}
