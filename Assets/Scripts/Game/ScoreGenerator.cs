using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGenerator
{
    static List<List<bool>> easy = new List<List<bool>>() {
        new List<bool>() {true, false, true, false},
        new List<bool>() {true, true, true, false},
        new List<bool>() {true, false, false, true}
    };

    public static Queue<bool> GetScore(int musicLength, int barLength, int seed)
    {
        float length = (musicLength - 2) * barLength;
        Queue<bool> score = new Queue<bool>();
        Random.InitState(seed);

        while (length > 0)
        {
            List<bool> choice = easy[Random.Range(0, easy.Count - 1)];
            for (int i = 0; i < choice.Count; i++)
            {
                score.Enqueue(choice[i]);
            }
            length -= choice.Count;
        }

        for (int i = 0; i < barLength * 3; i++)
        {
            score.Enqueue(false);
        }

        return score;
    }
}
