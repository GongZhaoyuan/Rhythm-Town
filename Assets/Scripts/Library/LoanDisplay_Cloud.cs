using UnityEngine;
using UnityEngine.UI;

public class LoanDisplayManager_Cloud : MonoBehaviour
{
    public Image image;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;

    private BookManager_Cloud Cloud_bookManager;

    private void Start()
    {
        Cloud_bookManager = FindObjectOfType<BookManager_Cloud>();
        if (Cloud_bookManager == null)
        {
            Debug.LogError("Cloud_bookManager not found in the scene.");
        }
    }

    private void Update()
    {
        if (Cloud_bookManager != null
        // && Cloud_bookManager.loanDisplayStatus_Cloud.ContainsKey(Cloud_bookManager.missObject)
        )
        {
            bool missObjectValue = Cloud_bookManager
                .objectData_Cloud[Cloud_bookManager.missObject]
                .IsEquipped;
            bool coinObjectValue = Cloud_bookManager
                .objectData_Cloud[Cloud_bookManager.coinObject]
                .IsEquipped;
            bool rangeObjectValue = Cloud_bookManager
                .objectData_Cloud[Cloud_bookManager.rangeObject]
                .IsEquipped;
            bool energyObjectValue = Cloud_bookManager
                .objectData_Cloud[Cloud_bookManager.energyObject]
                .IsEquipped;

            if (missObjectValue)
            {
                image.sprite = sprite1;
                // Cloud_bookManager.objectData_Cloud[Cloud_bookManager.missObject].IsEquipped = false;
            }
            else if (coinObjectValue)
            {
                image.sprite = sprite2;
                // Cloud_bookManager.objectData_Cloud[Cloud_bookManager.coinObject].IsEquipped = false;
            }
            else if (rangeObjectValue)
            {
                image.sprite = sprite3;
                // Cloud_bookManager.objectData_Cloud[Cloud_bookManager.rangeObject].IsEquipped =
                //     false;
            }
            else if (energyObjectValue)
            {
                image.sprite = sprite4;
                // Cloud_bookManager.objectData_Cloud[Cloud_bookManager.energyObject].IsEquipped =
                //     false;
            }
        }
    }

    public void ResetLoanDisplayStatus()
    {
        foreach (var kvp in Cloud_bookManager.objectData_Cloud)
        {
            Cloud_bookManager.objectData_Cloud[kvp.Key].IsEquipped = false;
        }
    }
}
