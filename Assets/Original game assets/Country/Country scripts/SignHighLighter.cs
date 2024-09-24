using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignHighLighter : MonoBehaviour
{
    public GameObject arrowSprite;


    
	public void StartArrowHighlight()
    {
        arrowSprite.SetActive(true);
    }
    public void StopArrowHighlight()
    {
		arrowSprite.SetActive(false);
	}

}
