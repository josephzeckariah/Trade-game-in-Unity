using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingExchangeElemnt : MonoBehaviour
{


    //by other propertis
    [HideInInspector]
    public Sprite ourSprite;
	//[HideInInspector]
	//public Vector3 ourMoveDirection; //
    [HideInInspector]
    public RectTransform ourMoveTarget;
	[HideInInspector]
	public RectTransform ourMoveBase;
	[HideInInspector]
	public MovingObjectGenerator ourHeadGenerator;
	[HideInInspector]
	public bool isOutGoing;

	//auto connection
	Image ourImageRenderer;

	//private memory
	Vector3 ourLineDirection;
	// Start is called before the first frame update
	void Awake()
    {
		ourImageRenderer = GetComponent<Image>();
    }
	private void Start()
	{
		ourImageRenderer.sprite = ourSprite;
	}

	// Update is called once per frame
	void Update()
    {
		
		MoveToDestination();
	}
	void MoveToDestination()
	{
		
		if (isOutGoing)
		{			
			this.transform.Translate(ourLineDirection * Time.deltaTime * 2, Space.World);
		}
		else if (!isOutGoing)
		{
			this.transform.Translate(-ourLineDirection * Time.deltaTime * 2, Space.World);
		}	
	}



	public void ForighnOrderOnLineRotated(Vector3 newDirection)
    {
		SunFunRecalculateDirection(newDirection);
		SubFunRotateAroundExchangeBase(newDirection);
	}

    void SubFunRotateAroundExchangeBase(Vector3 newDirection)
    {
		//dd
		Vector3 vectorFromMovingObjectToBase = (this.transform.position - ourMoveBase.transform.position);
		float magnitudeOflIne = vectorFromMovingObjectToBase.magnitude;
		Vector3 rotatedVectorToBase = newDirection * magnitudeOflIne;
		Vector3 finalPositionOfMovingObject = ourMoveBase.transform.position + rotatedVectorToBase;
		this.transform.position = finalPositionOfMovingObject;


	}
	void SunFunRecalculateDirection(Vector3 newDirection)
    {

		ourLineDirection = newDirection;
	}




}
