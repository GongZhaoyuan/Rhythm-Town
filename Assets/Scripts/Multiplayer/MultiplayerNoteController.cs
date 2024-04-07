using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerNoteController : NoteController
{
    protected override void Start()
    {
        base.Start();
    }
    
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        MultiplayerGameController.NoteData noteData = MultiplayerGameController.notes[noteID];
        if (!isClicked && noteData.isClicked)
        {
            destination = noteData.destination;
            MoveToDestination();
            gradeText.text = noteData.gradeText.ToString();
            isClicked = true;
        }
    }
}
