using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//Examples for saving data in the cloud.
// var data = new Dictionary<string, object>{ { "MySaveKey", "HelloWorld" } };
// await CloudSaveService.Instance.Data.ForceSaveAsync(data);

public class New_BookManager : MonoBehaviour
{
    public TMP_Text infoText;

    //Level
    public TMP_Text CoinLevelText;
    public TMP_Text EnergyLevelText;
    public TMP_Text MissLevelText;
    public TMP_Text RangeLevelText;

    //Borrow and Return
    public Button Coin1Borrow;
    public Button Coin1Return;
    public Button Energy2Borrow;
    public Button Energy2Return;
    public Button Miss3Borrow;
    public Button Miss3Return;
    public Button Range4Borrow;
    public Button Range4Return;

    public Dictionary<string, BookData> objectData_Cloud;

    // LoanDisplayManager_Cloud loanDisplayManager = FindObjectOfType<LoanDisplayManager_Cloud>();



    public class BookData
    {
        public int Level { get; set; }
        public bool IsEquipped { get; set; }
    }

    void Start()
    {
        StartCoroutine(StartAsync());
    }

    private IEnumerator StartAsync()
    {
        //level later should be getting the initial from cloud

        objectData_Cloud = new Dictionary<string, BookData>()
        {
            {
                "Miss",
                new BookData { Level = 0, IsEquipped = false }
            },
            {
                "Coin",
                new BookData { Level = 1, IsEquipped = false }
            },
            {
                "Range",
                new BookData { Level = 2, IsEquipped = false }
            },
            {
                "Energy",
                new BookData { Level = 3, IsEquipped = false }
            }
        };
        yield return UnityServices.InitializeAsync();
    }

    public void CoinInfoClicked()
    {
        infoText.text = "Coin: Earn more coin for each game!";
    }

    public void EnergyInfoClicked()
    {
        infoText.text = "Energy: Less energy needed for each game!";
    }

    public void MissInfoClicked()
    {
        infoText.text = "Miss: Help you reduce the number of missed note!";
    }

    public void RangeInfoClicked()
    {
        infoText.text = "Range: The accuracy detection rage will increase!";
    }

    public void CoinLevelUP()
    {
        Debug.Log("coin level up");
        objectData_Cloud["Coin"].Level += 1;
        CoinLevelText.text = "Lv." + objectData_Cloud["Coin"].Level + "/bla";
        // UpdateRecordAsync();
    }

    public async void EnergyLevelUP()
    {
        objectData_Cloud["Energy"].Level += 1;
        EnergyLevelText.text = "Lv." + objectData_Cloud["Energy"].Level + "/bla";
        await UpdateRecordAsync();
    }

    public async void MissLevelUP()
    {
        objectData_Cloud["Miss"].Level += 1;
        MissLevelText.text = "Lv." + objectData_Cloud["Miss"].Level + "/bla";
        await UpdateRecordAsync();
    }

    public async void RangeLevelUP()
    {
        objectData_Cloud["Range"].Level += 1;
        RangeLevelText.text = "Lv." + objectData_Cloud["Range"].Level + "/bla";
        await UpdateRecordAsync();
    }

    public async void SetBookEquipped(string bookName, bool isEquipped)
    {
        foreach (var bookData in objectData_Cloud.Values)
        {
            bookData.IsEquipped = false;
        }

        if (objectData_Cloud.ContainsKey(bookName))
        {
            objectData_Cloud[bookName].IsEquipped = isEquipped;
        }
        await UpdateRecordAsync();
    }

    public void CoinBorrow()
    {
        if (Coin1Borrow.gameObject.activeSelf == true)
        {
            Coin1Borrow.gameObject.SetActive(false);
            Coin1Return.gameObject.SetActive(true);
            objectData_Cloud["Coin"].IsEquipped = true;
            SetBookEquipped("Coin", true);
        }
        else if (Coin1Return.gameObject.activeSelf == true)
        {
            Coin1Borrow.gameObject.SetActive(true);
            Coin1Return.gameObject.SetActive(false);
            objectData_Cloud["Coin"].IsEquipped = false;
            SetBookEquipped("Coin", false);
        }
    }

    public void EnergyBorrow()
    {
        if (Energy2Borrow.gameObject.activeSelf == true)
        {
            Energy2Borrow.gameObject.SetActive(false);
            Energy2Return.gameObject.SetActive(true);
            objectData_Cloud["Energy"].IsEquipped = true;
            SetBookEquipped("Energy", true);
        }
        else if (Energy2Return.gameObject.activeSelf == true)
        {
            Energy2Borrow.gameObject.SetActive(true);
            Energy2Return.gameObject.SetActive(false);
            objectData_Cloud["Energy"].IsEquipped = false;
            SetBookEquipped("Energy", false);
        }
    }

    public void MissBorrow()
    {
        if (Miss3Borrow.gameObject.activeSelf == true)
        {
            Miss3Borrow.gameObject.SetActive(false);
            Miss3Return.gameObject.SetActive(true);
            objectData_Cloud["Miss"].IsEquipped = true;
            SetBookEquipped("Miss", true);
        }
        else if (Miss3Return.gameObject.activeSelf == true)
        {
            Miss3Borrow.gameObject.SetActive(true);
            Miss3Return.gameObject.SetActive(false);
            objectData_Cloud["Miss"].IsEquipped = false;
            SetBookEquipped("Miss", false);
        }
    }

    public void RangeBorrow()
    {
        if (Range4Borrow.gameObject.activeSelf == true)
        {
            Range4Borrow.gameObject.SetActive(false);
            Range4Return.gameObject.SetActive(true);
            objectData_Cloud["Range"].IsEquipped = true;
            SetBookEquipped("Range", true);
        }
        else if (Range4Return.gameObject.activeSelf == true)
        {
            Range4Borrow.gameObject.SetActive(true);
            Range4Return.gameObject.SetActive(false);
            objectData_Cloud["Range"].IsEquipped = false;
            SetBookEquipped("Range", false);
        }
    }

    public async Task UpdateRecordAsync()
    {
        Debug.Log("cloud function called");
        // Save the objectLevelRecord_Cloud to the cloud or perform any other necessary updates
        var data = new Dictionary<string, object>
        {
            { "objectLevelRecord_Cloud", objectData_Cloud }
        };
        // Task CloudSaveService.Instance.Data.Player.SaveAsync(Dictionary<string, object> data)；
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }
}