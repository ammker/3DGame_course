using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController
{
    public GameObject boat;
    private int boatState;                //1:左边 2：右边
    private int[] indexCharactersOnBoat;  //数字代表船上角色的编号，-1表示空

    public float moveSpeed = 20;
    //public Move move;

    //初始化
    public BoatController()
    {
        boatState = 1;
        indexCharactersOnBoat = new int[2];
        for (int i = 0; i < 2; i++)
        {
            indexCharactersOnBoat[i] = -1;
        }

    }

    public void setName(string n)
    {
        this.boat.name = n;
    }

    public int getState()
    {
        return this.boatState;
    }

    public int getNumEmptyPos()
    {
        int counter = 0;
        for (int i = 0; i < 2; i++)
        {
            if (indexCharactersOnBoat[i] == -1)
                counter++;
        }
        return counter;
    }

    public Vector3 getEmptyPos()
    {
        for (int i = 0; i < 2; i++)
        {
            if (indexCharactersOnBoat[i] == -1)
            {
                if (boatState == 1)
                    return new Vector3((-5f + 2*i),-0.5f, 0);
                else if (boatState == 2)
                    return new Vector3((3f + 2*i), -0.5f, 0);
                break;
            }
        }
        return Vector3.zero;
    }

    public void putACharacter(CharacterController charactercontroller)
    {
        for (int i = 0; i < 2; i++)
        {
            if (indexCharactersOnBoat[i] == -1)  //character坐标的变化不由boat实现，boat不用变
            {
                indexCharactersOnBoat[i] = charactercontroller.getCharacterIndex();
                break;
            }
        }
    }

    public void removeACharacter(CharacterController charactercontroller)
    {
        for (int i = 0; i < 2; i++)
        {
            if (indexCharactersOnBoat[i] == charactercontroller.getCharacterIndex())
            {
                indexCharactersOnBoat[i] = -1;
                break;
            }
        }
    }

    /* public void moveToAnotherBank()
     {
         if (boatState == 1)
         {
             Vector3 target = boat.transform.position + new Vector3(8, 0, 0);
             move.setDestination(target);
             this.boatState = 2;
         }
         else if (boatState == 2)
         {
             Vector3 target = boat.transform.position - new Vector3(8, 0, 0);
             move.setDestination(target);
             this.boatState = 1;
         }
     }
    */
    public Vector3 getDestination()
    {
        Vector3 target = Vector3.zero;
        if (boatState == 1)
        {
            target = boat.transform.position + new Vector3(8, 0, 0);
            this.boatState = 2;
        }
        else if (boatState == 2)
        {
            target = boat.transform.position - new Vector3(8, 0, 0);
            this.boatState = 1;
        }
        return target;
    }

    public GameObject getGameobj()
    {
        return this.boat;
    }

    public int[] getPersonOnBoat()
    {
        int[] result = new int[2];
        for (int i = 0; i < 2; i++)
        {
            result[i] = indexCharactersOnBoat[i];
        }
        return result;
    }

    public void reset()
    {
        boatState = 1;
        for (int i = 0; i < 2; i++)
        {
            indexCharactersOnBoat[i] = -1;
        }
        boat.transform.position = new Vector3(-4, -2, 0);
    }

}