using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankController
{
    public GameObject bank;
    private int index;                //0:左岸 1:右岸
    public int[] indexCharactersOnBank;   //-1：空  0：牧师  1：恶魔

    public BankController(string i)
    {
        if (i == "left") index = 0;
        else if (i == "right") index = 1;

        indexCharactersOnBank = new int[6];
        for (int j = 0; j < 6; j++)
        {
            indexCharactersOnBank[j] = -1;
        }
    }

    public void removeACharacter(CharacterController charactercontroller)
    {
        indexCharactersOnBank[charactercontroller.getCharacterIndex()] = -1;
    }

    public void putACharacter(CharacterController charactercontroller)
    {
        indexCharactersOnBank[charactercontroller.getCharacterIndex()] = charactercontroller.getType();
    }

    public void setName(string n)
    {
        this.bank.name = n;
    }
    public int getNumDevil()
    {
        int count = 0;
        for (int i = 0; i < 6; i++)
        {
            if (indexCharactersOnBank[i] == 0) count++;
        }
        return count;
    }

    public int getNumPriest()
    {
        int count = 0;
        for (int i = 0; i < 6; i++)
        {
            if (indexCharactersOnBank[i] == 1) count++;
        }
        return count;
    }

    public void reset()
    {
        if (index == 0)
        {
            int j;
            for (j = 0; j < 3; j++)
                indexCharactersOnBank[j] = 0;
            for (; j < 6; j++)
                indexCharactersOnBank[j] = 1;
        }

        else if (index == 1)
        {
            for (int j = 0; j < 6; j++)
            {
                indexCharactersOnBank[j] = -1;
            }
        }
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
