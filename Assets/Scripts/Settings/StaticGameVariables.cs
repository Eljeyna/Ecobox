using UnityEngine;
using TMPro;
using UnityEngine.UI;

public static class StaticGameVariables
{
    #region Variables
    public enum Language
    {
        Russian,
        English,
    }

    public enum ActionType
    {
        DropItem,
        DisassembleItem,
    }

    public static Language language;
    public static ActionType actionWithItem;
    public static Item.ItemType currentItemCategory = Item.ItemType.WeaponMelee;

    public static GameObject player;
    public static GameObject slotSelected;

    public static Color slotDefaultColor;
    public static Color slotColor;
    public static Color[] colorItems;

    public static Inventory inventory;
    public static Item itemSelected;

    public static CanvasGroup inventoryGroup;

    public static Canvas inventoryCanvas;
    public static Canvas itemInfoCanvas;
    public static Canvas gameUI;
    public static Canvas staticUI;
    public static Canvas inGameUI;
    public static Canvas yesNoCanvas;

    public static Slider yesNoSlider;
    public static TMP_Text yesNoAmount;

    public static TMP_Text itemName;
    public static TMP_Text itemDescription;

    public static Button buttonUseItem;
    public static Button buttonDropItem;
    public static Button buttonDisItem;

    public static float progress;

    public static float defaultTimeScale;
    public static float defaultFixedDeltaTime;
    #endregion

    #region Initialize
    public static void InitializeAwake()
    {
        language = (Language)PlayerPrefs.GetInt("Language", 0);
        if (language != 0)
        {
            ChangeLanguage((int)language);
        }

        player = GameObject.Find("_PLAYER");
        GameObject inventoryObject = GameObject.Find("Inventory");
        GameObject yesNoObject = GameObject.Find("YesNoMenu");
        GameObject listButtons = GameObject.Find("InventoryButtons");

        inventoryGroup = inventoryObject.GetComponent<CanvasGroup>();

        inventoryCanvas = inventoryObject.GetComponent<Canvas>();
        itemInfoCanvas = GameObject.Find("ItemInfo").GetComponent<Canvas>();
        gameUI = GameObject.Find("DynamicUI").GetComponent<Canvas>();
        staticUI = GameObject.Find("StaticUI").GetComponent<Canvas>();
        inGameUI = GameObject.Find("InGameInterface").GetComponent<Canvas>();
        yesNoCanvas = yesNoObject.GetComponent<Canvas>();

        yesNoSlider = yesNoObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        yesNoAmount = GameObject.Find("YesNoMenuAmount").GetComponent<TMP_Text>();

        itemName = GameObject.Find("ItemName").GetComponent<TMP_Text>();
        itemDescription = GameObject.Find("ItemDescription").GetComponent<TMP_Text>();

        buttonUseItem = listButtons.transform.GetChild(0).GetComponent<Button>();
        buttonDropItem = listButtons.transform.GetChild(1).GetComponent<Button>();
        buttonDisItem = listButtons.transform.GetChild(2).GetComponent<Button>();

        buttonUseItem.interactable = false;
        buttonDropItem.interactable = false;
        buttonDisItem.interactable = false;
        yesNoCanvas.enabled = false;

        yesNoSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        slotDefaultColor = new Color(80f / 255f, 80f / 255f, 80f / 255f, 0.5f);
        slotColor = new Color(1f, 1f, 1f, 0.5f);

        colorItems = new Color[6];
        colorItems[0] = new Color(72f / 255f, 60f / 255f, 60f / 255f, 1f);
        colorItems[1] = new Color(86f / 255f, 183f / 255f, 93f / 255f, 1f);
        colorItems[2] = new Color(86f / 255f, 137f / 255f, 183f / 255f, 1f);
        colorItems[3] = new Color(147f / 255f, 86f / 255f, 183f / 255f, 1f);
        colorItems[4] = new Color(183f / 255f, 175f / 255f, 86f / 255f, 1f);
        colorItems[5] = new Color(183f / 255f, 121f / 255f, 86f / 255f, 1f);

        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public static void Initialize()
    {
        inventory = player.GetComponent<Player>().inventory;
    }
    #endregion

    #region Functions
    public static void ChangeCategoryItem(Item.ItemType itemCategory)
    {
        if (currentItemCategory == itemCategory)
        {
            return;
        }

        itemSelected = null;
        currentItemCategory = itemCategory;
        itemInfoCanvas.enabled = false;
        DisableInventoryButtons();
        inventory.CallUpdateInventory();
    }

    public static void UseItem()
    {
        if (itemSelected)
        {
            float amount = itemSelected.itemAmount;
            itemSelected.Use();

            if (itemSelected.itemAmount <= 0)
            {
                inventory.RemoveItem(itemSelected);
                itemSelected = null;
            }

            if (itemSelected && itemSelected.itemAmount != amount)
            {
                inventory.CallUpdateInventory();
            }
            else if (itemSelected == null)
            {
                DisableInventoryButtons();
                itemInfoCanvas.enabled = false;
                inventory.CallUpdateInventory();
            }
        }
    }

    public static void ConfirmApply()
    {
        if (actionWithItem == ActionType.DropItem) DropItemMultiple();
        else if (actionWithItem == ActionType.DisassembleItem) DisassembleItemMultiple();

        HideConfirmMenu();
    }

    public static void DropItem()
    {
        actionWithItem = ActionType.DropItem;

        if (itemSelected && itemSelected.itemAmount > 1)
        {
            OpenConfirmMenu();
        }
        else
        {
            DisableInventoryButtons();
            itemInfoCanvas.enabled = false;
            inventory.RemoveItem(itemSelected);
            itemSelected = null;
        }

        inventory.CallUpdateInventory();
    }

    public static void DropItemMultiple()
    {
        itemSelected.itemAmount -= (int)yesNoSlider.value;

        if (itemSelected && itemSelected.itemAmount <= 0)
        {
            inventory.RemoveItem(itemSelected);
            itemSelected = null;
        }

        inventory.CallUpdateInventory();
    }

    public static void DisassembleItem()
    {
        actionWithItem = ActionType.DisassembleItem;

        if (itemSelected && itemSelected.itemAmount > 1)
        {
            OpenConfirmMenu();
        }
        else
        {
            DisableInventoryButtons();
            itemInfoCanvas.enabled = false;
            inventory.RemoveItem(itemSelected);
            itemSelected = null;
        }

        inventory.CallUpdateInventory();
    }

    public static void DisassembleItemMultiple()
    {
        itemSelected.itemAmount -= (int)yesNoSlider.value;

        if (itemSelected && itemSelected.itemAmount <= 0)
        {
            inventory.RemoveItem(itemSelected);
            itemSelected = null;
        }

        inventory.CallUpdateInventory();
    }

    public static void OpenConfirmMenu()
    {
        yesNoAmount.text = "1";
        yesNoSlider.value = 1;
        yesNoSlider.maxValue = itemSelected.itemAmount;
        inventoryGroup.blocksRaycasts = false;
        yesNoCanvas.enabled = true;
    }

    public static void HideConfirmMenu()
    {
        DisableInventoryButtons();

        itemInfoCanvas.enabled = false;
        yesNoCanvas.enabled = false;
        inventoryGroup.blocksRaycasts = true;
    }

    public static void OpenInventory()
    {
        PauseGame();
        HideInGameUI();
        staticUI.enabled = true;
        inventoryCanvas.enabled = true;
    }

    public static void HideInventory()
    {
        if (slotSelected)
            slotSelected.GetComponent<Image>().color = slotDefaultColor;
        itemSelected = null;
        itemInfoCanvas.enabled = false;
        inventoryCanvas.enabled = false;
        staticUI.enabled = false;
        DisableInventoryButtons();
        ShowInGameUI();
        ContinueGame();
    }

    public static void ShowInGameUI()
    {
        inGameUI.enabled = true;
    }

    public static void HideInGameUI()
    {
        inGameUI.enabled = false;
    }

    public static void DisableInventoryButtons()
    {
        buttonUseItem.interactable = false;
        buttonDropItem.interactable = false;
        buttonDisItem.interactable = false;
    }

    public static void EnableInventoryButtons()
    {
        buttonUseItem.interactable = true;
        buttonDropItem.interactable = true;
        buttonDisItem.interactable = true;
    }

    public static void ChangeLanguage(int languageChange)
    {
        if ((int)language == languageChange)
        {
            return;
        }

        language = (Language)languageChange;

        GameObject[] allUI = GameObject.FindGameObjectsWithTag("Translate");
        foreach (GameObject child in allUI)
        {
            TranslateUI translation = child.GetComponent<TranslateUI>();
            TMP_Text textUI = child.transform.GetChild(0).GetComponent<TMP_Text>();
            textUI.text = translation.languages[languageChange];
        }

        PlayerPrefs.SetInt("Language", languageChange);
        PlayerPrefs.Save();
    }

    public static void PauseGame()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }

    public static void ContinueGame()
    {
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }

    public static void ValueChangeCheck()
    {
        yesNoAmount.text = yesNoSlider.value.ToString();
    }
    #endregion
}
