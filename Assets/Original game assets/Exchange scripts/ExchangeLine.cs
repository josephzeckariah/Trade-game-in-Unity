using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeLine : MonoBehaviour
{
    //auto connection
    RectTransform rectTransform;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }



	public void ForeighnOrderUpdateLine(Vector3 endPoint)
	{
		
		UpdateLength(endPoint);
	}


	 void UpdateLength(Vector3 endPoint)
    {
        float Distance =Vector3.Distance( this.transform.position, endPoint );


        float hight = this.rectTransform.sizeDelta.y;

		this.rectTransform.sizeDelta = new Vector2(Distance,hight);
    }
}
