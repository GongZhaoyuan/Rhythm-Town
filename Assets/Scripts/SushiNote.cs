using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiNote : NoteController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        noteType = (isTarget) ? 1: 0;
        destination = new Vector2(0, -2);
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Sushi/{noteType}");
        targetSource = (noteType > 0) ? "ServiceBell" : "None";
        if (isTarget)
        {
            display.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sushi/Sushi Orange");
        }
        else{
            if (Random.value > 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
