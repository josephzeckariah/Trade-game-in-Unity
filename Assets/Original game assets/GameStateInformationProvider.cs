using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EmptyFun();
public delegate void Vector2Fun(Vector2 vec2);
public delegate void ExchangeDelegatee(Needs needOfExchange);
public static class GameStateInformationProvider 
{
    public static Vector2Fun ScreenSizeChanged;
    public static EmptyFun GameStarted;
    public static EmptyFun OpeningScreenClosed;

	public static ExchangeDelegatee anEchangeEnded;

}
