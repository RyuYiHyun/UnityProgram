using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratedMotion
{
	public static float SameSpeed(float start, float end, float value) // ���� ������
	{
		return (end - start) * value + start;
	}
	public static Vector3 SameSpeed(Vector3 start, Vector3 end, float value) // ���� ������
	{
		return (end - start) * value + start;
	}

	public static float FirstSlowLostFast(float start, float end, float value) // (ó���� ������)���� ������
	{
		end -= start;
		return end * value * value + start;
	}
	public static float FirstFastLostSlow(float start, float end, float value)// (ó���� ������)���� ������
	{
		end -= start;
		return -end * value * (value - 2) + start;
	}
	public static Vector3 FirstFastLostSlow(Vector3 start, Vector3 end, float value)// (ó���� ������)���� ������
	{
		end -= start;
		return -end * value * (value - 2) + start;
	}
}
