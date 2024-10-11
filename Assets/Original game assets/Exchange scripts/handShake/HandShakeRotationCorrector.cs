using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandShakeRotationCorrector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CorrectRotation());
    }

    IEnumerator CorrectRotation()
    {
        while (true)
        {
			this.transform.rotation = Quaternion.identity;
			yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}
