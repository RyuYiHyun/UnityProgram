using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class Unit : MonoBehaviour
{
    [HideInInspector]public StateMachine stateMachine;
    public Tile endTile;
    public Tile tile;
    public Directions dir;
    public void Place(Tile _endTile)
    {
        if (tile != null && tile.content == gameObject)
        {
            tile.content = null;
        }
        tile = _endTile;
        if(tile != null)
        {
            tile.content = gameObject;
        }
    }
    // �ش� ���� ������Ʈ�� Position �� EulerAngles ���� �����մϴ�.
    public void Match()
    {
        transform.localPosition = tile.center;

        // Vector3(x, y, z)�� Rotation�� ���ϴ� �̴ϴ�.
        transform.localEulerAngles = dir.ToEuler();
    }
    public void Interpolation(Tile _tile)
    {
        transform.localPosition = _tile.center;// tile.center;
    }

    public void Turn(Directions direction)
    {
        if (dir != direction)
        {
            dir = direction;
            transform.localEulerAngles = dir.ToEuler();
        }
    }

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        if (stateMachine != null)
        {
            stateMachine.ChangeState<IdleState>();
        }
    }

    private void Update()
    {
        if(stateMachine != null)
        {
            stateMachine.CurrentState.OnUpdate();
        }
    }
}
