using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EmptyFun();
public delegate void Vector2Fun(Vector2 vec2);
public delegate void ExchangeDelegatee(Needs needOfExchange);

public enum ExchangeMakingState { LookingToStart, LookingToEnd }
public static class GameStateInformationProvider 
{
	//S///////////////////////////////////////////////////////////    General      /////////////////////////////////////////////////////////////
	public static Vector2Fun ScreenSizeChanged;
    public static EmptyFun GameStarted;
	//S///////////////////////////////////////////////////////////     Signs      /////////////////////////////////////////////////////////////
	public static EmptyFun OpeningScreenClosed;

	//S///////////////////////////////////////////////////////////     Exchange     /////////////////////////////////////////////////////////////
	public static ExchangeDelegatee anEchangeEnded;
	public static ExchangeDelegatee anEchangeStarted;

	public static ExchangeMakingState currentExchangeState;

}
