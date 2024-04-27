using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerGameController : GameController
{
    public struct NoteData : INetworkSerializable, IEquatable<NoteData>
    {
        public int noteID;
        public bool isClicked;
        public Vector2 destination;
        public FixedString32Bytes gradeText;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref noteID);
            serializer.SerializeValue(ref isClicked);
            serializer.SerializeValue(ref destination);
            serializer.SerializeValue(ref gradeText);
        }

        public bool Equals(NoteData other) { return other.noteID == noteID; }
    }

    public static NetworkList<NoteData> notes;

    protected override void Start()
    {
        base.Start();
        notes = new NetworkList<NoteData>();
    }

    protected override void RecordNote(GameObject noteObject)
    {
        base.RecordNote(noteObject);
        MultiplayerNoteController noteController = noteObject.GetComponent<MultiplayerNoteController>();
        int id = noteController.noteID;
        
        if (id < notes.Count)
        {
            UpdateNoteListServerRpc(new NoteData
            {                
                noteID = id,
                isClicked = true,
                destination = noteController.destination,
                gradeText = noteController.gradeText.text
            });  
        }
    }

    protected override void GenerateNote()
    {
        if (noteID == notes.Count && IsHost)
        {
            notes.Add(new NoteData
            {
                noteID = noteID,
                isClicked = false,
                gradeText = ""
            });
        }

        base.GenerateNote();        
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateNoteListServerRpc(NoteData noteData)
    {
        notes[noteData.noteID] = noteData;    
    }
}
