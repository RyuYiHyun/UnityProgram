using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionsExtensions
{
    // Ÿ�ٰ��� ���⿡ ���� Directions�� Enum ���� ���ϵȴ�.
    // ���⼭ this�� TileŬ������ Ȯ�� �޼ҵ�� �ν� -> GetDirections�Լ��� DirectionsExtensions Ŭ���� ���  static�̿�����
    // Ȯ�� �޼ҵ��? �����Ϸ��� �߰������� Tile.GetDirections()���� ���� �����ϰ� ����
    public static Directions GetDirection(this Tile t1, Tile t2) 
    {
        if (t1.pos.y < t2.pos.y)
            return Directions.North;
        if (t1.pos.x < t2.pos.x)
            return Directions.East;
        if (t1.pos.y > t2.pos.y)
            return Directions.South;
        return Directions.West;
    }

    // ������ ���Ϸ� ������ ��ȯ�Ѵ�.
    public static Vector3 ToEuler(this Directions d)
    {
        return new Vector3(0, (int)d * 90, 0);
    }
}
