using UnityEngine;

public class MultiplayerSushiNote : MultiplayerNoteController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();        
        destination = new Vector2(0, -2);
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Sushi/{noteType}");
        noteType = isTarget? 1: 0;
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
