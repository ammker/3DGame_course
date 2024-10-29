using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgment
{
    public bool checkWin(BoatController boat, BankController leftBank, BankController rightBank)
    {
        if (boat.getNumEmptyPos() == 2 && (leftBank.getNumDevil() + leftBank.getNumPriest() == 0) && (rightBank.getNumDevil() + rightBank.getNumPriest() == 6))
            return true;
        return false;
    }

    public bool checkLose(BoatController boat, BankController leftBank, BankController rightBank)
    {
        int countDevilLeft = leftBank.getNumDevil(), countPriestLeft = leftBank.getNumPriest();
        int countDevilRight = rightBank.getNumDevil(), countPriestRight = rightBank.getNumPriest();
        int[] personOnBoat = boat.getPersonOnBoat();   
        int d = 0, p = 0;
        for (int i = 0; i < 2; i++)
        {
            if (personOnBoat[i] < 3 && personOnBoat[i] >= 0) d++;
            else if (personOnBoat[i] >= 3) p++;
        }
        if (boat.getState() == 1)
        {
            countDevilLeft += d;
            countPriestLeft += p;
        }
        else if (boat.getState() == 2)
        {
            countDevilRight += d;
            countPriestRight += p;
        }
        if ((countDevilLeft > countPriestLeft && countPriestLeft != 0) || (countDevilRight > countPriestRight && countPriestRight != 0))
        {
            return true;
        }
        return false;
    }
}
