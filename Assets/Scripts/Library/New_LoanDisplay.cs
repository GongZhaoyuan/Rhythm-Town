using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class New_LoanDisplayManager : MonoBehaviour
{
    public GameObject OnLoan;
    public GameObject miss_position;
    public GameObject coin_position;
    public GameObject range_position;
    public GameObject energy_position;

    private New_BookManager Cloud_bookManager;

    private void Start()
    {
        Cloud_bookManager = FindObjectOfType<New_BookManager>();
        if (Cloud_bookManager == null)
        {
            Debug.LogError("Cloud_bookManager not found in the scene.");
        }
        OnLoan.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Cloud_bookManager != null
        // && Cloud_bookManager.loanDisplayStatus_Cloud.ContainsKey(Cloud_bookManager.missObject)
        )
        {
            bool missObjectValue = Cloud_bookManager.objectData_Cloud["Miss"].IsEquipped;
            bool coinObjectValue = Cloud_bookManager.objectData_Cloud["Coin"].IsEquipped;
            bool rangeObjectValue = Cloud_bookManager.objectData_Cloud["Range"].IsEquipped;
            bool energyObjectValue = Cloud_bookManager.objectData_Cloud["Energy"].IsEquipped;

            if (missObjectValue)
            {
                OnLoan.gameObject.SetActive(true);
                OnLoan.transform.position = miss_position.transform.position;

                // Cloud_bookManager.objectData_Cloud[Cloud_bookManager.missObject].IsEquipped = false;
            }
            else if (coinObjectValue)
            {
                OnLoan.gameObject.SetActive(true);
                OnLoan.transform.position = coin_position.transform.position;
                // Cloud_bookManager.objectData_Cloud[Cloud_bookManager.coinObject].IsEquipped = false;
            }
            else if (rangeObjectValue)
            {
                OnLoan.gameObject.SetActive(true);
                OnLoan.transform.position = range_position.transform.position;
                // Cloud_bookManager.objectData_Cloud[Cloud_bookManager.rangeObject].IsEquipped =
                //     false;
            }
            else if (energyObjectValue)
            {
                OnLoan.gameObject.SetActive(true);
                OnLoan.transform.position = energy_position.transform.position;
                // Cloud_bookManager.objectData_Cloud[Cloud_bookManager.energyObject].IsEquipped =
                //     false;
            }
        }
    }
}
