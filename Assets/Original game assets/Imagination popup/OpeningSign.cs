using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningSign : MonoBehaviour
{
	public ImaginationSignShower ourHeadSignShower;

	private void Start()
	{
		RepoSitionOpeningSign(Vector2.zero);
		GameMaker.gameScreenSizeChanged += RepoSitionOpeningSign;
	}
	public void OpeningScreenOnExit()
	{
		ourHeadSignShower.ForignOrderOpeningSCreenClosed();

		Destroy(gameObject);
	}
	void RepoSitionOpeningSign(Vector2 newSize)
	{
		this.GetComponent<RectTransform>().sizeDelta = ourHeadSignShower.canvasRectTransform.sizeDelta;
	}

}
