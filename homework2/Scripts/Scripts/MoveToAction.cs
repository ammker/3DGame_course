using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAction : Action
{
    public Vector3 target;
    public float speed;

    private MoveToAction() { }
    public static MoveToAction getAction(Vector3 target, float speed)
    {
        MoveToAction action = ScriptableObject.CreateInstance<MoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if (this.transform.position == target)
        {
            this.destroy = true;
            this.callback.ActionEvent(this);
        }
        else
        {
            //this.callback.ActionEvent(this, ActionEventType.Started);
        }
    }

    public override void Start()
    {
       
    }
}
