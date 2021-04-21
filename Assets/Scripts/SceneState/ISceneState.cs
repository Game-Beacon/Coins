using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ISceneState
{
    protected GameLoop gameLoop;
    protected string _name;
    public string Name => _name;
    public ISceneState(GameLoop GameLoop)
    {
        gameLoop = GameLoop;
        _name=this.GetType().ToString();
    }
    // for State begin
    public abstract void SceneBegin();
    // for State end
    public abstract void SceneEnd();
    // for State update
    public abstract void SceneUpdate();
}
