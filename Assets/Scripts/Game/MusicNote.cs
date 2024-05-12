using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : NoteController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        noteType = Random.Range(0, 3);
        Sprite[] sprites = Resources.LoadAll<Sprite>("Orchestra/instra");
        Debug.Log("sprites: " + sprites);
        targetSource = $"instra_{noteType}";
        destination = new Vector2(-99, -99);
        display.GetComponent<SpriteRenderer>().sprite = sprites[noteType];
        if (!isTarget)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // displayRb.gameObject.GetComponent<Animator>().SetTrigger("Moved");
    }

    protected override void MoveToDestination()
    {
        destroyTimer = fullBeat / 8;
    }
}
