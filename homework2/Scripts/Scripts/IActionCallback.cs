using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionEventType :int { Started, Competeted}

public interface IActionCallback
{
    void ActionEvent(Action source,ActionEventType events =ActionEventType.Competeted);
}
