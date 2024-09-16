using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeMaker : MonoBehaviour
{
	[Header("Manual connections")]
    public static GameObject exchangeAsset;
	public GameObject ExchangeAssetINspector;

	public GameObject parentOfExchangeObjects;
	private void Awake()
	{	
		exchangeAsset = ExchangeAssetINspector.gameObject;
	}


	public Exchange MakeTradeRoute(Need startingNeed, Needs needThatsExchanged)
	{
		GameObject instantiatedExchangeObj = Instantiate(exchangeAsset, startingNeed.transform.position, Quaternion.identity, parentOfExchangeObjects.transform);

		instantiatedExchangeObj.name = "Exchange In " + startingNeed.sighnNeedType.ToString() + " Between " + startingNeed.ourCountryParent.name+" and "; //set up name

		Exchange instantiatedExchange = instantiatedExchangeObj.GetComponent<Exchange>();
		instantiatedExchange.exchangeNeed = needThatsExchanged;
		instantiatedExchange.exchangeStartingSign = startingNeed;

		return instantiatedExchange;
	}
}
