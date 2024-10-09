using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Tick();
    void Enter();
    void Exit();
}
