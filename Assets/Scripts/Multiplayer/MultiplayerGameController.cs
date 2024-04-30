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

    public NetworkList<NoteData> notes;
    public Transform hostDestination, clientDestination;
    public static Vector2 _hostDestination, _clientDestination;
    public static bool isHost;

    protected override void Start()
    {
        base.Start();
        notes = new NetworkList<NoteData>();
        _hostDestination = hostDestination.position;
        _clientDestination = clientDestination.position;
        isHost = IsHost;
    }

    public void RecordNote(GameObject noteObject)
    {
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
        if (noteID == notes.Count && isHost)
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
