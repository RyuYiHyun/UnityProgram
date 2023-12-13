using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionsExtensions
{
    // 타겟과의 방향에 따라 Directions의 Enum 값이 리턴된다.
    // 여기서 this는 Tile클래스의 확장 메소드로 인식 -> GetDirections함수와 DirectionsExtensions 클래스 모두  static이여야함
    // 확장 메소드란? 컴파일러가 추가적으로 Tile.GetDirections()으로 동작 가능하게 만듬
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

    // 방향을 오일러 각도로 반환한다.
    public static Vector3 ToEuler(this Directions d)
    {
        return new Vector3(0, (int)d * 90, 0);
    }
}
