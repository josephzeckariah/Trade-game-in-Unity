using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//-//////////////////////////////////////////////////////////////////////////////////////////////////       Memories       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	[Header("peoperies")]
    public float cameraMovementSpeed = 6;

	[Header("Manual connections")]
	public SpriteRenderer backgroundImage;

	///////////////////////////////////////////////////////////// Auto connections
	Camera ourCamera;

	///////////////////////////////////////////////////////////// private memory
	Vector2 areaCameraIsConstrainedIn;
	Vector2 worldUnitsOfBackgroundHalfed;

	Coroutine ourMouseOnEdgeCameraMovement;

	Vector3 CentrePointDependingGameType;            //centre point to start and endgameon
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//+///////////////////////////////////////////////////////////////////////////////////////////////         Actions        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void Awake()
	{
		ourCamera = GetComponent<Camera>();

		GameStateInformationProvider.AnyGameStart += OnGameStart;
		GameStateInformationProvider.AllSigns100 += OnAllSigns100;
	}
	//S///////////////////////////////////////////////////////////     Start       /////////////////////////////////////////////////////////////
	private void Start()
	{
		StartFunCalculateBackgroundForConstrainr();
		CalculateCameraAreaConstrained(new Vector2(0,0));

		GameStateInformationProvider.ScreenSizeChanged += CalculateCameraAreaConstrained;
		//StartCoroutine(AlwaysCheckForCameraChangeToReCalculateConstrains());   //check continous if camera size has changed

	}

	                  void StartFunCalculateBackgroundForConstrainr()
	{
		Sprite backgroundSprite = backgroundImage.sprite;
		Vector2 imagePixelSize = new Vector2(backgroundSprite.texture.width, backgroundSprite.texture.height);
		float pixelPerUnitOfBackGRound = backgroundSprite.pixelsPerUnit;

		worldUnitsOfBackgroundHalfed = (imagePixelSize / pixelPerUnitOfBackGRound) / 2;
	}
	                  void CalculateCameraAreaConstrained(Vector2 unused)
	{

		float cameraYSize = ourCamera.orthographicSize;
		float cameraAspect = ourCamera.aspect;
		float cameraXSize = cameraYSize * cameraAspect;

		Vector2 cameraSize = new Vector2(cameraXSize, cameraYSize);
		areaCameraIsConstrainedIn = new Vector2(worldUnitsOfBackgroundHalfed.x - cameraSize.x, worldUnitsOfBackgroundHalfed.y - cameraSize.y);
	}




	//S///////////////////////////////////////////////////////////     Always       /////////////////////////////////////////////////////////////
	void OnGameStart()
	{
		DecideCentrePointOfGameAndGoToIt();


		ourCamera.orthographicSize = 5f;
		ourMouseOnEdgeCameraMovement = StartCoroutine(MoveCameraIfOnEdgeOfScreen());
	}

	                                 void DecideCentrePointOfGameAndGoToIt()
	{
		if (GameStateInformationProvider.currentGameType == GameStates.NormalGame)
		{
			CentrePointDependingGameType = new Vector3(5, -1, -10);
		}
		else
		{
			CentrePointDependingGameType = new Vector3(-10, 1, -10);
		}
		this.transform.position = CentrePointDependingGameType;

	}


	                                 IEnumerator MoveCameraIfOnEdgeOfScreen()
	{
		while (true)
		{
			ContinousCalculateCameraMovementFromMousePosition();
			yield return null;
		}
	}
	                                             void ContinousCalculateCameraMovementFromMousePosition()
	{
		if (GeneralInformationProvider.gameIsOnMobile)
		{
			if (Input.touchCount == 0)
			{
				return;
			}
		}

		Vector3 mousePostionInViewPort = ourCamera.ScreenToViewportPoint(Input.mousePosition);
		Vector3 translateAmount = new Vector3();
		//Get the y axis Movemeent and save it translateAmount variable
		if (mousePostionInViewPort.y > 0.8)
		{
			translateAmount.y = Mathf.InverseLerp(0.8f, 1, mousePostionInViewPort.y);

		}
		else if (mousePostionInViewPort.y < 0.2)
		{
			translateAmount.y = -Mathf.InverseLerp(0.2f, 0, mousePostionInViewPort.y);
		}
		//Get the x axis Movemeent and save it translateAmount variable
		if (mousePostionInViewPort.x > 0.8)
		{
			translateAmount.x = Mathf.InverseLerp(0.8f, 1, mousePostionInViewPort.x);

		}
		else if (mousePostionInViewPort.x < 0.2)
		{
			translateAmount.x = -Mathf.InverseLerp(0.2f, 0, mousePostionInViewPort.x);
		}


		//check constrains
		if (ourCamera.transform.position.x > areaCameraIsConstrainedIn.x && translateAmount.x > 0)
		{
			translateAmount.x = 0;
		}
		if (ourCamera.transform.position.x < -areaCameraIsConstrainedIn.x && translateAmount.x < 0)
		{
			translateAmount.x = 0;
		}
		if (ourCamera.transform.position.y < -areaCameraIsConstrainedIn.y && translateAmount.y < 0)
		{
			translateAmount.y = 0;
		}
		if (ourCamera.transform.position.y > areaCameraIsConstrainedIn.y && translateAmount.y > 0)
		{
			translateAmount.y = 0;
		}




		//the move action
		ourCamera.transform.Translate(translateAmount * Time.deltaTime * cameraMovementSpeed, Space.Self);


	}


	void OnAllSigns100()
	{
		StopCoroutine(ourMouseOnEdgeCameraMovement);
		StartCoroutine(EndAnimation());
	}


	//S///////////////////////////////////////////////////////////     End aniamtion      /////////////////////////////////////////////////////////////

	            IEnumerator EndAnimation()
	{
		
		yield return StartCoroutine(MoveToward(this.transform, CentrePointDependingGameType, 0.5f));

	
		if (GameStateInformationProvider.ZoomStarted != null)
		{
			GameStateInformationProvider.ZoomStarted();
		}                     //         -------------------------------------------------------->>>

		yield return ZoomCamera(ourCamera, 8,8);

		if (GameStateInformationProvider.GameEnded != null)
		{
			GameStateInformationProvider.GameEnded();
		}                      //         -------------------------------------------------------->>>
	}                                               //         -------------------------------------------------------->>>


	                   IEnumerator MoveToward(Transform objectToMove,Vector3 targetLocation,float speed)
	{
		Vector3 direction = targetLocation - objectToMove.position;



		while (ReturnTrueIfPointIsBeforeTheOtherOnTheDirection(objectToMove.position, direction, targetLocation))
		{


			objectToMove.position += new Vector3(direction.x * speed, direction.y * speed, 0f) * Time.deltaTime;

			yield return null;
		}

	}
	                           bool ReturnTrueIfPointIsBeforeTheOtherOnTheDirection(Vector3 pointToCheck, Vector3 direction, Vector3 pointToCheckIfAfter)
	{
		bool xIsOk = true;                                          //onlu works in 2d
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

	                    IEnumerator ZoomCamera(Camera orthographiccameraToZoom,float amountToGoTo,float timeTakenToGo)
	{
		bool CameraIsSmallerThanTarget = orthographiccameraToZoom.orthographicSize < amountToGoTo;

		float DiferenceInAmount = amountToGoTo - orthographiccameraToZoom.orthographicSize;

		if (CameraIsSmallerThanTarget)
		{
			while (orthographiccameraToZoom.orthographicSize < amountToGoTo)
			{
				orthographiccameraToZoom.orthographicSize += (DiferenceInAmount/timeTakenToGo) * Time.deltaTime;

				yield return null;
			}
		}
		else if (!CameraIsSmallerThanTarget)
		{
			while (orthographiccameraToZoom.orthographicSize > amountToGoTo)
			{
				orthographiccameraToZoom.orthographicSize += (DiferenceInAmount / timeTakenToGo) * Time.deltaTime;

				yield return null;
			}
		}
	
	}

}
