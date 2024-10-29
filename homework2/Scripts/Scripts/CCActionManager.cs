using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager,IActionCallback
{
    public FirstController sceneController;
    protected new void Start()
    {
        sceneController = (FirstController)SSDirector.getInstance().currentSceneController;
    }
    public void moveBoat(BoatController boat)
    {
        MoveToAction action = MoveToAction.getAction(boat.getDestination(), boat.moveSpeed);
        this.RunAction(boat.getGameobj(), action, this);
        SSDirector.getInstance().moving = true;
    }

    public void moveCharacter(CharacterController characterCtrl, Vector3 destination)
    {
        Vector3 currentPos = characterCtrl.getPos();
        Vector3 middlePos = currentPos;
        if (destination.y > currentPos.y)
        {       
            middlePos.y = destination.y;
        }
        else
        {   
            middlePos.x = destination.x;
        }
        Action action1 = MoveToAction.getAction(middlePos, characterCtrl.moveSpeed);
        Action action2 = MoveToAction.getAction(destination, characterCtrl.moveSpeed);
        Action seqAction = SequenceAction.getAction(1, 0, new List<Action> { action1, action2 });
        this.RunAction(characterCtrl.getGameobj(), seqAction, this);
        SSDirector.getInstance().moving = true;
    }
    public void ActionEvent(Action source, ActionEventType events = ActionEventType.Competeted)
    {
        if (events == ActionEventType.Competeted) SSDirector.getInstance().moving = false;
        else
        {
            SSDirector.getInstance().moving = true;
        }
    }
}
