using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerServiceBell : ServiceBell
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (!GameObject.Find("MultiplayerGameController").GetComponent<MultiplayerGameController>().IsHost)
        {
            transform.position = new Vector2(-transform.position.x, transform.position.y);
        }
    }
}
