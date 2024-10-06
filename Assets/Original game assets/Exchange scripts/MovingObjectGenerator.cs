using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
public struct MovingElemntLineInformation
{
	public bool lineIsSending;

	public RectTransform lineStartPoint;
	public RectTransform lineEndPoint;
	public RectTransform lineStartRemover;

	public Sprite spriteOfLineElemnts;

	public List<MovingExchangeElemnt> lineListOfObject;

}

public class MovingObjectGenerator : MonoBehaviour
{
	//manual properties
	public float disntaceOfMovingObjectGeneration = 1f;

	//auto properties
	[HideInInspector]
	public Needs needToSpawn;
	[HideInInspector]
	public bool startSignIsBuying;

	//Manual Connections
	public GameObject movingElemntTemplate;

    public ExchangeLine ourCoWorkerLine;
    public Exchange ourHeadExchange;

	public RectTransform ObjectSentSpawnPoint;
	public RectTransform ObjectSentEndPoint;
	public RectTransform ObjectRecievedSpawnPoint;
	public RectTransform ObjectReceivedEndPoint;	

	//private memory
	List<MovingExchangeElemnt> ourSentMovingExchangeElemntCreated = new List<MovingExchangeElemnt>();
	List<MovingExchangeElemnt> ourRecievedMovingExchangeElemntCreated = new List<MovingExchangeElemnt>();

	MovingElemntLineInformation recievedGoodsLineInformation;
	MovingElemntLineInformation SentGoodsLineInformation;

	GameObject movingObjectparent;
	public Vector3 ourLineDirection = new Vector3();


	public void ForighnOrderSetUpLineInformationAndStartSpawningObjects()
	{
		StartCoroutine(this.CheckExchangeRotationChange());

		SubAwakeSetUpParentOfMovingObjects();
		SubAwakeSetUpLinesSpawnAndEndLocations();
		SubAwakeSetUpOurLineStructs();

		StartCoroutine(ForighnOrderSpawnMovingElemnt());


	}


	void SubAwakeSetUpParentOfMovingObjects()
	{
		GameObject parentOfAllMovingElemnts;
		if (GameObject.Find("parentOfAllMovingElemnts") != null)
		{
			parentOfAllMovingElemnts = GameObject.Find("parentOfAllMovingElemnts");
		}
		else
		{
			parentOfAllMovingElemnts = new GameObject("parentOfAllMovingElemnts");
		}

		movingObjectparent = new GameObject(this.gameObject.name, typeof(Canvas));
		movingObjectparent.transform.SetParent(parentOfAllMovingElemnts.transform);

	}
	void SubAwakeSetUpLinesSpawnAndEndLocations()
	{

		ObjectSentEndPoint.position = ourCoWorkerLine.transform.position + Quaternion.Euler(ourHeadExchange.transform.eulerAngles) *  new Vector3(ourCoWorkerLine.GetComponent<RectTransform>().sizeDelta.x, -ourCoWorkerLine.GetComponent<RectTransform>().sizeDelta.y, 0);
		ObjectSentSpawnPoint.position = ourCoWorkerLine.transform.position + Quaternion.Euler(ourHeadExchange.transform.eulerAngles) * new Vector3(0, -ourCoWorkerLine.GetComponent<RectTransform>().sizeDelta.y, 0);
		ObjectRecievedSpawnPoint.position = ourCoWorkerLine.transform.position + Quaternion.Euler(ourHeadExchange.transform.eulerAngles) * new Vector3(ourCoWorkerLine.GetComponent<RectTransform>().sizeDelta.x, ourCoWorkerLine.GetComponent<RectTransform>().sizeDelta.y, 0);
		ObjectReceivedEndPoint.position = ourCoWorkerLine.transform.position + Quaternion.Euler(ourHeadExchange.transform.eulerAngles) * new Vector3(0, ourCoWorkerLine.GetComponent<RectTransform>().sizeDelta.y, 0);

	}
	void SubAwakeSetUpOurLineStructs()
	{
		recievedGoodsLineInformation.lineIsSending = false;
		recievedGoodsLineInformation.lineStartPoint = ObjectRecievedSpawnPoint;
		recievedGoodsLineInformation.lineEndPoint = ObjectReceivedEndPoint;
		recievedGoodsLineInformation.lineListOfObject = ourRecievedMovingExchangeElemntCreated;
		if(startSignIsBuying)
		{
			recievedGoodsLineInformation.spriteOfLineElemnts = GeneralInformationProvider.needsAssets[needToSpawn];
		}else if (!startSignIsBuying)
		{
			recievedGoodsLineInformation.spriteOfLineElemnts = GeneralInformationProvider.Money;
		}
		//recievedGoodsLineInformation.lineStartRemover = ObjectRecievedRemover;


		SentGoodsLineInformation.lineIsSending = true;
		SentGoodsLineInformation.lineStartPoint = ObjectSentSpawnPoint;
		SentGoodsLineInformation.lineEndPoint = ObjectSentEndPoint;
		SentGoodsLineInformation.lineListOfObject = ourSentMovingExchangeElemntCreated;
		if (startSignIsBuying)
		{
			SentGoodsLineInformation.spriteOfLineElemnts = GeneralInformationProvider.Money; 
		}
		else if (!startSignIsBuying)
		{
			SentGoodsLineInformation.spriteOfLineElemnts = GeneralInformationProvider.needsAssets[needToSpawn];
		}
		//SentGoodsLineInformation.lineStartRemover = ObjectSentRemover;

	}




	public IEnumerator ForighnOrderSpawnMovingElemnt()
	{

		while (true)
		{
			ManageLine(recievedGoodsLineInformation);
			ManageLine(SentGoodsLineInformation);				
				
			yield return null;
		}
	}


	void ManageLine(MovingElemntLineInformation lineToManage)
	{
		CheckToMakeNewMovingElemntsFromStartpoint(lineToManage);

		CeckStartAndEndOfLineToFillWithMovingElemnts(lineToManage);

		CheckToRemoveMovingElemnts(lineToManage);
	}
	void CeckStartAndEndOfLineToFillWithMovingElemnts(MovingElemntLineInformation lineToChackAndMakeObjectsOn)
	{
		CheckToMakeNewMovingElemntsFromStartpoint(lineToChackAndMakeObjectsOn);
		CheckToMakeNewMovingElemntsFromEndpoint(lineToChackAndMakeObjectsOn);
	}

	void CheckToMakeNewMovingElemntsFromStartpoint(MovingElemntLineInformation lineToWorkOn)
	{
		Vector3 directionToAddMovingElemntsIn = lineToWorkOn.lineIsSending ? -ourLineDirection : ourLineDirection;
		Vector3 directionOfLine = lineToWorkOn.lineIsSending ? ourLineDirection : -ourLineDirection;

		if (lineToWorkOn.lineListOfObject.Count != 0)
		{
			if (Vector3.Distance(lineToWorkOn.lineStartPoint.position, lineToWorkOn.lineListOfObject[lineToWorkOn.lineListOfObject.Count - 1].transform.position )> disntaceOfMovingObjectGeneration  &&      //if last object on line is 1 point far from the starter
				ReturnTrueIfPointIsBeforeTheOtherOnTheDirection(lineToWorkOn.lineListOfObject[lineToWorkOn.lineListOfObject.Count - 1].transform.position, -directionOfLine, lineToWorkOn.lineStartPoint.position)) //and is before it on the line 
			{
				GameObject newMadeElemnt = SpawnMovingObjects(lineToWorkOn.lineListOfObject[lineToWorkOn.lineListOfObject.Count - 1].transform.position + (-directionOfLine * disntaceOfMovingObjectGeneration) ,lineToWorkOn, lineToWorkOn.lineListOfObject.Count);  //then do creat object
				//lineToWorkOn.lineListOfObject.Insert(lineToWorkOn.lineListOfObject.Count, newMadeElemnt.GetComponent<MovingExchangeElemnt>());

	

			}
		}
		else if (lineToWorkOn.lineListOfObject.Count == 0)
		{

			SpawnMovingObjects(lineToWorkOn.lineStartPoint.position, lineToWorkOn,0);
		}
	}
	void CheckToMakeNewMovingElemntsFromEndpoint(MovingElemntLineInformation lineToWorkOn)
	{
		Vector3 directionToAddMovingElemntsIn = lineToWorkOn.lineIsSending ? ourLineDirection : -ourLineDirection;
		Vector3 directionOfLine = lineToWorkOn.lineIsSending ? ourLineDirection : -ourLineDirection;

		if (lineToWorkOn.lineListOfObject.Count != 0)
		{
			if (Vector3.Distance(lineToWorkOn.lineEndPoint.position, lineToWorkOn.lineListOfObject[0].transform.position) > disntaceOfMovingObjectGeneration &&      //if last object on line is 1 point far from the starter
				ReturnTrueIfPointIsBeforeTheOtherOnTheDirection(lineToWorkOn.lineListOfObject[0].transform.position, directionOfLine, lineToWorkOn.lineEndPoint.position)) //and is before it on the line 
			{
				GameObject newMadeElemnt = SpawnMovingObjects(lineToWorkOn.lineListOfObject[0].transform.position + (directionOfLine * disntaceOfMovingObjectGeneration), lineToWorkOn,0);
				//lineToWorkOn.lineListOfObject.Insert(0, newMadeElemnt.GetComponent<MovingExchangeElemnt>());
			}
		}
	}


	bool ReturnTrueIfPointIsBeforeTheOtherOnTheDirection(Vector3 pointToCheck,Vector3 direction,Vector3 pointToCheckIfAfter)
	{
		bool xIsOk = true;
		bool YIsOk = true;
		float numberOfZeroComparison = 0.2f;
		if (direction.x < numberOfZeroComparison && direction.x > -numberOfZeroComparison)
		{
			if (direction.y > 0f)
			{
				if (pointToCheck.y < pointToCheckIfAfter.y)
				{
					return true;

				}
				else if (pointToCheck.y > pointToCheckIfAfter.y)
				{
					return false;
				}
			}
			else if (direction.y < 0f)
			{
				if (pointToCheck.y > pointToCheckIfAfter.y)
				{
					return true;

				}
				else if (pointToCheck.y < pointToCheckIfAfter.y)
				{
					return false;
				}
			}

			else if (direction.x > 0f)
			{
				if (pointToCheck.x < pointToCheckIfAfter.x)
				{
					xIsOk = true;
				}
				else
				{
					xIsOk = false;
				}
			}
			else if (direction.x < 0f)
			{
				if (pointToCheck.x > pointToCheckIfAfter.x)
				{
					xIsOk = true;
				}
				else
				{
					xIsOk = false;
				}
			}
		}



	 if (direction.y < numberOfZeroComparison && direction.y > -numberOfZeroComparison)
		{
			if (direction.x > 0f)
			{
				if (pointToCheck.x < pointToCheckIfAfter.x)
				{
					return true;

				}
				else if (pointToCheck.x > pointToCheckIfAfter.x)
				{
					return false;
				}
			}
			else if (direction.x < 0f)
			{
				if (pointToCheck.x > pointToCheckIfAfter.x)
				{
					return true;

				}
				else if (pointToCheck.x < pointToCheckIfAfter.x)
				{
					return false;
				}
			}
		}
		else if (direction.y > 0f)
		{
			if (pointToCheck.y < pointToCheckIfAfter.y)
			{
				YIsOk = true;
			}
			else
			{
				YIsOk = false;
			}
		}
		else if (direction.y < 0f)
		{
			if (pointToCheck.y > pointToCheckIfAfter.y)
			{
				YIsOk = true;
			}
			else
			{
				YIsOk = false;
			}
		}



		if (xIsOk && YIsOk)
		{
			return true;
		}
		else
		{
			return false;
		}


	}




	void CheckToRemoveMovingElemnts(MovingElemntLineInformation lineToRemoveIn)
	{
		CheckFirstObjectToRemove(lineToRemoveIn);
	
		CheckLastObjectToRemove(lineToRemoveIn);

	}
	void CheckFirstObjectToRemove(MovingElemntLineInformation lineToRemoveIn) 
	{
		Vector3 directionOfLine = lineToRemoveIn.lineIsSending ? ourLineDirection : -ourLineDirection;


		if ( lineToRemoveIn.lineListOfObject.Count != 0)
		{
		
			//	SubSubIenumReorderListToAVoidGlitch(lineToRemoveIn);
			if (!ReturnTrueIfPointIsBeforeTheOtherOnTheDirection(lineToRemoveIn.lineListOfObject[0].transform.position, directionOfLine, lineToRemoveIn.lineEndPoint.position + (directionOfLine * 0.1f)))
				{
					RemoveMovingElement(lineToRemoveIn.lineListOfObject[0], lineToRemoveIn);
					
					if (lineToRemoveIn.lineListOfObject.Count == 0)

					{
						//	break;
					}
				}

			

		}
	}
	/*void SubSubIenumReorderListToAVoidGlitch(MovingElemntLineInformation lineToRemoveIn)
	{
		List<MovingExchangeElemnt> newList = new List<MovingExchangeElemnt>();
		foreach (MovingExchangeElemnt elemnt in lineToRemoveIn.lineListOfObject)
		{
			if (elemnt != null)
			{
				newList.Add(elemnt);
			}


		}

		lineToRemoveIn.lineListOfObject.Clear();
		foreach (MovingExchangeElemnt elemnt in newList)
		{
			lineToRemoveIn.lineListOfObject.Add(elemnt);

		}

	}*/
	void CheckLastObjectToRemove(MovingElemntLineInformation lineToRemoveIn)
	{
		Vector3 directionOfLine = lineToRemoveIn.lineIsSending ? ourLineDirection : -ourLineDirection;

		if (lineToRemoveIn.lineListOfObject.Count != 0)
		{
			if (ReturnTrueIfPointIsBeforeTheOtherOnTheDirection(lineToRemoveIn.lineListOfObject[lineToRemoveIn.lineListOfObject.Count-1].transform.position, directionOfLine, lineToRemoveIn.lineStartPoint.position + (-directionOfLine * 0.1f)))//(Vector3.Distance(lineToRemoveIn.lineListOfObject[lineToRemoveIn.lineListOfObject.Count-1].transform.position, objectRemoverToUse.position) < objectRemoverToUse.sizeDelta.x/2)
			{
				RemoveMovingElement(lineToRemoveIn.lineListOfObject[lineToRemoveIn.lineListOfObject.Count-1], lineToRemoveIn);
				if(lineToRemoveIn.lineListOfObject.Count ==0) 
				{
					//break;
				}
			}
		}
	}




	GameObject SpawnMovingObjects(Vector3 spawnPoint ,MovingElemntLineInformation lineObjectbelongTo,int indexOfListTOPutIN)//(Vector3 spawnPoint,Vector3 targetPosition, Sprite spriteofSpawned,List<MovingExchangeElemnt> listToAddTO)
	{
		GameObject instantiatedMovingObj = Instantiate(movingElemntTemplate, spawnPoint, Quaternion.identity,movingObjectparent.transform);

		MovingExchangeElemnt instantiatedMoving = instantiatedMovingObj.GetComponent<MovingExchangeElemnt>();
		lineObjectbelongTo.lineListOfObject.Insert(indexOfListTOPutIN,instantiatedMoving);
		instantiatedMoving.ourSprite = lineObjectbelongTo.spriteOfLineElemnts;
		instantiatedMoving.ourMoveBase = lineObjectbelongTo.lineIsSending ? lineObjectbelongTo.lineStartPoint : lineObjectbelongTo.lineEndPoint;
		instantiatedMoving.isOutGoing = lineObjectbelongTo.lineIsSending ? true : false;
		instantiatedMoving.ourMoveTarget = lineObjectbelongTo.lineEndPoint;
		instantiatedMoving.ourHeadGenerator = this;
		instantiatedMoving.ForighnOrderOnLineRotated(ourLineDirection);
		return instantiatedMovingObj;

	}
	void RemoveMovingElement(MovingExchangeElemnt movingObjectBiengRemoved, MovingElemntLineInformation lineObjectBelongTO)
	{
		lineObjectBelongTO.lineListOfObject.Remove(movingObjectBiengRemoved);
		Destroy(movingObjectBiengRemoved.gameObject);
		

	}


	IEnumerator CheckExchangeRotationChange() 
	{
		ourLineDirection = new Vector3(Mathf.Cos(ourHeadExchange.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(ourHeadExchange.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), 0);
		SubOenumTellOurMovingElemntsToRerotate(ourLineDirection);

		Vector3 previousRotation = ourHeadExchange.transform.rotation.eulerAngles;
		while(true)
		{
			if (ourHeadExchange.transform.rotation.eulerAngles != previousRotation)
			{
				ourLineDirection = new Vector3(Mathf.Cos(ourHeadExchange.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(ourHeadExchange.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), 0); //calculate the line direction and save it
				
				SubOenumTellOurMovingElemntsToRerotate(ourLineDirection);
			
				previousRotation = ourHeadExchange.transform.rotation.eulerAngles;		
			}

			yield return null;
		}

	}
	void SubOenumTellOurMovingElemntsToRerotate(Vector3 newLineDirection) 
	{
		if (ourRecievedMovingExchangeElemntCreated.Count != 0)
		{
			foreach (MovingExchangeElemnt thisMovingObject in ourRecievedMovingExchangeElemntCreated) //tell each movingElement to rotate by that amount
			{
				thisMovingObject.ForighnOrderOnLineRotated(newLineDirection);
			}
		}

		if (ourSentMovingExchangeElemntCreated.Count != 0)
		{
			foreach (MovingExchangeElemnt thisMovingObject in ourSentMovingExchangeElemntCreated)  //tell each movingElement to rotate by that amount
			{
				thisMovingObject.ForighnOrderOnLineRotated(newLineDirection);
			}
		}
	}


	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ForighnOrderRemoveAllMovingElemnts()
	{
		Destroy(movingObjectparent);
		
	}
}
