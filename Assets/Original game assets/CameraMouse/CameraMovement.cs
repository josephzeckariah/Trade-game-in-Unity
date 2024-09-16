using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [Header("peoperies")]
    public float cameraMovementSpeed = 6;

	[Header("Manual connections")]
	public SpriteRenderer backgroundImage;

    //auto connections
    Camera ourCamera;

	//private memory
	Vector2 areaCameraIsConstrainedIn;
	Vector2 worldUnitsOfBackgroundHalfed;

	private void Awake()
	{
		ourCamera = GetComponent<Camera>();
	}
	private void Start()
	{
		StartFunCalculateBackgroundForConstrainr();
		CalculateCameraAreaConstrained(new Vector2(0,0));

		GameMaker.gameScreenSizeChanged += CalculateCameraAreaConstrained;
		//StartCoroutine(AlwaysCheckForCameraChangeToReCalculateConstrains());   //check continous if camera size has changed

	}

	void Update()
    {
		ContinousCalculateCameraMovementFromMousePosition();

		


	}

	void StartFunCalculateBackgroundForConstrainr()
	{
		Sprite backgroundSprite = backgroundImage.sprite;
		Vector2 imagePixelSize = backgroundSprite.texture.Size();
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
	IEnumerator AlwaysCheckForCameraChangeToReCalculateConstrains()
	{
	
		float aspectSizeOfCamera = ourCamera.aspect;
		while (true)
		{
			if (ourCamera.aspect != aspectSizeOfCamera)   //if camera size has changed
			{
			//	CalculateCameraAreaConstrained();        //recalculate constrains
				aspectSizeOfCamera = ourCamera.aspect;
			}
			yield return new WaitForSecondsRealtime(1f);
		}
	}
	void ContinousCalculateCameraMovementFromMousePosition()
    {
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
		ourCamera.transform.Translate(translateAmount * Time.deltaTime* cameraMovementSpeed, Space.Self);

		
	}
}
