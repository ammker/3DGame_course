using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController
{

    public GameObject character;
    private int type;        //0:Devil 1:Priest
    private int index;      //从0开始 0-2:Devil 3-5:Priest
    private int state;    //0:在船上  1：在左岸  2：在右岸

    public float moveSpeed = 20;
    //public Move move;

    public CharacterController(string t, int i)
    {
        if (t == "Devil") type = 0;
        else if (t == "Priest") type = 1;

        index = i;
        state = 1;
    }

    public int getState()
    {
        return state;
    }

    public int getCharacterIndex()
    {
        return this.index;
    }

    public int getType()
    {
        return this.type;
    }

    public void setName(string n)
    {
        this.character.name = n;
    }


    public void setPositionOnLeftBank()
    {
        this.state = 1;
        //move.setDestination(new Vector3(-20 + index*2, 2f, 0));
    }

    public Vector3 getPosOnLeftBank()
    {
        return new Vector3(-20 + index * 2, 2f, 0);
    }

    public void setPositionOnRightBank()
    {
        this.state = 2;
        //move.setDestination(new Vector3(10 + index*2, 2f, 0));
    }

    public Vector3 getPosOnRightBank()
    {
        return new Vector3(10 + index * 2, 2f, 0);
    }
    public void setPositionOnBoat()//Vector3 pos
    {
        this.state = 0;
        //move.setDestination(pos);
    }
/*
    public void moveOnBoat(int boatState, int posOnBoat)
    {
        if (boatState == 1)
        {
            Vector3 target = character.transform.position + new Vector3(8, 0, 0);
            move.setDestination(target);
        }
        else if (boatState == 2)
        {
            Vector3 target = character.transform.position + new Vector3(-8, 0, 0);
            move.setDestination(target);
        }
    }
*/
    public Vector3 getDestinationOnBoat(int boatState, int posOnBoat)
    {
        if (boatState == 1)
        {
            return character.transform.position + new Vector3(8, 0, 0);
        }
        else if (boatState == 2)
        {
            return character.transform.position - new Vector3(8, 0, 0);
        }
        return Vector3.zero;
    }

    public Vector3 getPos()
    {
        return this.character.transform.position;
    }

    public GameObject getGameobj()
    {
        return this.character;
    }

    public void reset()
    {
        character.transform.position = new Vector3(-20f + index*2, 2f, 0);
        //move.reset();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
