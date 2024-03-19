using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailNote : NoteController
{
    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        isTarget = Random.value > 0.5f;
        noteType = (isTarget) ? 1: 0;
        Sprite[] sprites = Resources.LoadAll<Sprite>("Mail/Envelope");
        targetSource = $"Cardboard Box_{noteType}";
        destination = new Vector2(-99,-99);
        display.GetComponent<SpriteRenderer>().sprite = sprites[noteType];
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void MoveToDestination()
    {
        base.MoveToDestination();
        displayRb.gameObject.GetComponent<Animator>().SetTrigger("Moved");
    }
}
