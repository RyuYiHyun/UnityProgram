using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratedMotion
{
	public static float SameSpeed(float start, float end, float value) // 같은 빠르기
	{
		return (end - start) * value + start;
	}
	public static Vector3 SameSpeed(Vector3 start, Vector3 end, float value) // 같은 빠르기
	{
		return (end - start) * value + start;
	}

	public static float FirstSlowLostFast(float start, float end, float value) // (처음엔 느리다)점점 빠르게
	{
		end -= start;
		return end * value * value + start;
	}
	public static float FirstFastLostSlow(float start, float end, float value)// (처음엔 빠르다)점점 느리게
	{
		end -= start;
		return -end * value * (value - 2) + start;
	}
	public static Vector3 FirstFastLostSlow(Vector3 start, Vector3 end, float value)// (처음엔 빠르다)점점 느리게
	{
		end -= start;
		return -end * value * (value - 2) + start;
	}
}
