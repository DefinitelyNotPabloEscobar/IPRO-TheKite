using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private int difficulty;

    public int getDifficulty()
    {
        return this.difficulty;
    }

    public void setDifficulty(int d)
    {
        this.difficulty = d;
    }
}
