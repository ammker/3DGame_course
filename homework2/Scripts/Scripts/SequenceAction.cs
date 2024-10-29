using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceAction : Action, IActionCallback
{
    public List<Action> sequence;
    public int repeat = 1; // 1->only do it for once, -1->repeat forever
    public int start = 0;

    public static SequenceAction getAction(int repeat, int start, List<Action> sequence)
    {
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count)
        {
            sequence[start].Update();
        }
    }

    public void ActionEvent(Action source, ActionEventType events = ActionEventType.Competeted)
    {
        source.destroy = false;
        this.start++;
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            {
                this.destroy = true;
                this.callback.ActionEvent(this);
            }
        }
    }

    public override void Start()
    {
        foreach (Action action in sequence)
        {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }

    void OnDestroy()
    {
        foreach (Action action in sequence)
        {
            Destroy(action);
        }
    }
}

