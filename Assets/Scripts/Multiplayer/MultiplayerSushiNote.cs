using UnityEngine;

public class MultiplayerSushiNote : MultiplayerNoteController
{
    public Sprite hostSprite, clientSprite;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();        
        destination = MultiplayerGameController.isHost? MultiplayerGameController._hostDestination : MultiplayerGameController._clientDestination;
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Sushi/{noteType}");
        noteType = isTarget? 1: 0;
        targetSource = (noteType > 0) ? "ServiceBell" : "None";
        if (isTarget)
        {
            display.GetComponent<SpriteRenderer>().sprite = (Random.value > 0.5f)? hostSprite : clientSprite;
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
