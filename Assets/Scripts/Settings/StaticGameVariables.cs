using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using System.IO;

public static class StaticGameVariables
{
    #region Variables
    public enum Language
    {
        English = 0,
        Russian,
    }

    public static readonly string[] languageKeys =
    {
        "English",
        "Russian"
    };

    public static string[] translationString = new string[2];

    public enum ActionType
    {
        DropItem,
        DisassembleItem,
    }

    public static bool isPause = true;
    public static float random;

    public static Language language;
    public static ActionType actionWithItem;
    public static Item.ItemType currentItemCategory = Item.ItemType.Weapon;

    public static GameObject slotSelected;

    public static Color slotDefaultColor;
    public static Color slotColor;
    public static Color[] colorItems;

    public static Item itemSelected;

    public static CanvasGroup inventoryGroup;

    public static Canvas inventoryCanvas;
    public static Canvas itemInfoCanvas;
    public static Canvas staticUI;
    public static Canvas inGameUI;
    public static Canvas quickUI;
    public static Canvas inventoryYesNoCanvas;

    public static Slider yesNoSlider;
    public static TMP_Text yesNoAmount;

    public static TMP_Text itemName;
    public static TMP_Text itemDescription;

    public static Button buttonUseItem;
    public static Button buttonDropItem;
    public static Button buttonDisItem;

    public static TMP_Text questName;
    public static TMP_Text taskDescription;

    public static Transform _DIALOGUES;
    public static Transform _ITEMS;

    public static float progress;

    #region Settings
    public static readonly float shakeForce = 2f;
    public static readonly float camMaxSize = 20f;
    public static readonly float camMinSize = 8f;

    public const int maxLevel = 60;
    public const int maxBonus = 20;
    #endregion

    #region Entity Maker
    public const float minDistance = 20f;
    public const float maxDistance = 70f;
    public const float distanceFade = 200f;
    #endregion
    
    #region Animations

    public static readonly int animationKeyID = Animator.StringToHash("Animation");
    public static readonly int animationMoveKeyID = Animator.StringToHash("IsMove");
    #endregion
    
    public static event System.EventHandler OnPauseGame;
    
    public static string _SAVE_FOLDER = Path.Combine(Application.persistentDataPath, "Saves");
    public static string defaultValueAmount = "1";
    #endregion

    #region SpecialSymbols
    public static readonly char genderSymbol = '$';
    public static readonly char splitSymbol  = '|';
    #endregion

    #region Initialize

    public static void InitializeLanguage()
    {
        StringBuilder sb = new StringBuilder("Language");
        int languageCheck = PlayerPrefs.GetInt(sb.ToString(), 0);
        ChangeLanguage(languageCheck);
    }

    public static void InitializeAwake()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("Inventory");
        GameObject inventoryObject = GameObject.Find(sb.ToString());
        sb.Clear();
        sb.Append("InventoryYesNoMenu");
        GameObject yesNoObject = GameObject.Find(sb.ToString());
        sb.Clear();
        sb.Append("InventoryButtons");
        GameObject listButtons = GameObject.Find(sb.ToString());

        inventoryGroup = inventoryObject.GetComponent<CanvasGroup>();

        inventoryCanvas = inventoryObject.GetComponent<Canvas>();
        sb.Clear();
        sb.Append("ItemInfo");
        itemInfoCanvas = GameObject.Find(sb.ToString()).GetComponent<Canvas>();
        sb.Clear();
        sb.Append("StaticUI");
        staticUI = GameObject.Find(sb.ToString()).GetComponent<Canvas>();
        sb.Clear();
        sb.Append("InGameInterface");
        inGameUI = GameObject.Find(sb.ToString()).GetComponent<Canvas>();
        sb.Clear();
        sb.Append("QuickMenu");
        quickUI = GameObject.Find(sb.ToString()).GetComponent<Canvas>();

        inventoryYesNoCanvas = yesNoObject.GetComponent<Canvas>();

        yesNoSlider = yesNoObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        sb.Clear();
        sb.Append("YesNoMenuAmount");
        yesNoAmount = GameObject.Find(sb.ToString()).GetComponent<TMP_Text>();

        sb.Clear();
        sb.Append("ItemName");
        itemName = GameObject.Find(sb.ToString()).GetComponent<TMP_Text>();
        sb.Clear();
        sb.Append("ItemDescription");
        itemDescription = GameObject.Find(sb.ToString()).GetComponent<TMP_Text>();

        sb.Clear();
        sb.Append("QuestName");
        questName = GameObject.Find(sb.ToString()).GetComponent<TMP_Text>();

        sb.Clear();
        sb.Append("TaskDescription");
        taskDescription = GameObject.Find(sb.ToString()).GetComponent<TMP_Text>();

        sb.Clear();
        sb.Append("_DIALOGUES");
        _DIALOGUES = GameObject.Find(sb.ToString()).transform;
        
        sb.Clear();
        sb.Append("_ITEMS");
        _ITEMS = GameObject.Find(sb.ToString()).transform;

        buttonUseItem = listButtons.transform.GetChild(0).GetComponent<Button>();
        buttonDropItem = listButtons.transform.GetChild(1).GetComponent<Button>();
        buttonDisItem = listButtons.transform.GetChild(2).GetComponent<Button>();

        buttonUseItem.interactable = false;
        buttonDropItem.interactable = false;
        buttonDisItem.interactable = false;
        inventoryYesNoCanvas.enabled = false;

        yesNoSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        slotDefaultColor = new Color(80f / 255f, 80f / 255f, 80f / 255f, 0.5f);
        slotColor = new Color(1f, 1f, 1f, 0.5f);

        colorItems = new Color[5];
        colorItems[0] = new Color(72f / 255f, 60f / 255f, 60f / 255f, 1f);
        colorItems[1] = new Color(86f / 255f, 183f / 255f, 93f / 255f, 1f);
        colorItems[2] = new Color(86f / 255f, 137f / 255f, 183f / 255f, 1f);
        colorItems[3] = new Color(147f / 255f, 86f / 255f, 183f / 255f, 1f);
        colorItems[4] = new Color(183f / 255f, 175f / 255f, 86f / 255f, 1f);
    }

    public static async void InitializeFinale()
    {
        Player.Instance.joystickMove = GameUI.Instance.joystickMove;
        Player.Instance.joystickAttack = GameUI.Instance.joystickAttack;

        if (Settings.Instance.gameIsLoaded)
        {
            await SaveLoadSystem.Instance.Load();
            
            Player.Instance.Initialize();

            Settings.Instance.gameIsLoaded = false;
            ResumeGame();
        }
        else
        {
            GameDirector.Instance.Initialize();
        }
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
        Player.Instance.inventory.CallUpdateInventory();
    }

    public static void UseItem()
    {
        if (!ReferenceEquals(itemSelected, null))
        {
            int amount = itemSelected.itemAmount;
            itemSelected.Use();

            if (itemSelected.itemAmount <= 0)
            {
                Player.Instance.inventory.RemoveItem(itemSelected);
                itemSelected = null;
            }

            if (itemSelected && itemSelected.itemAmount != amount)
            {
                Player.Instance.inventory.CallUpdateInventory();
            }
            else if (itemSelected == null)
            {
                DisableInventoryButtons();
                itemInfoCanvas.enabled = false;
                Player.Instance.inventory.CallUpdateInventory();
            }
        }
    }

    public static void ConfirmApply()
    {
        switch (actionWithItem)
        {
            case ActionType.DropItem:
                DropItemMultiple();
                break;
            case ActionType.DisassembleItem:
                DisassembleItemMultiple();
                break;
        }

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
            Player.Instance.inventory.RemoveItem(itemSelected);
            itemSelected = null;
        }

        Player.Instance.inventory.CallUpdateInventory();
    }

    public static void DropItemMultiple()
    {
        itemSelected.itemAmount -= (int)yesNoSlider.value;

        if (itemSelected && itemSelected.itemAmount <= 0)
        {
            Player.Instance.inventory.RemoveItem(itemSelected);
            itemSelected = null;
        }

        Player.Instance.inventory.CallUpdateInventory();
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
            Player.Instance.inventory.RemoveItem(itemSelected);
            itemSelected = null;
        }

        Player.Instance.inventory.CallUpdateInventory();
    }

    public static void DisassembleItemMultiple()
    {
        itemSelected.itemAmount -= (int)yesNoSlider.value;

        if (itemSelected && itemSelected.itemAmount <= 0)
        {
            Player.Instance.inventory.RemoveItem(itemSelected);
            itemSelected = null;
        }

        Player.Instance.inventory.CallUpdateInventory();
    }

    public static void OpenConfirmMenu()
    {
        yesNoAmount.text = defaultValueAmount;
        yesNoSlider.value = 1;
        yesNoSlider.maxValue = itemSelected.itemAmount;
        inventoryGroup.blocksRaycasts = false;
        inventoryYesNoCanvas.enabled = true;
    }

    public static void HideConfirmMenu()
    {
        DisableInventoryButtons();

        itemInfoCanvas.enabled = false;
        inventoryYesNoCanvas.enabled = false;
        inventoryGroup.blocksRaycasts = true;
    }

    public static async void OpenInventory()
    {
        itemSelected = null;

        await Player.Instance.inventory.PreloadInventory();
        Player.Instance.inventory.CallUpdateInventory();

        PauseGame();
        HideInGameUI();
        staticUI.enabled = true;
        inventoryCanvas.enabled = true;
    }

    public static void HideInventory()
    {
        Player.Instance.inventory.UnloadInventory();

        if (slotSelected)
            slotSelected.GetComponent<Image>().color = slotDefaultColor;
        itemInfoCanvas.enabled = false;
        inventoryCanvas.enabled = false;
        staticUI.enabled = false;
        DisableInventoryButtons();
        ShowInGameUI();
        ResumeGame();
    }

    public static void ShowInGameUI()
    {
        inGameUI.enabled = true;
    }

    public static void HideInGameUI()
    {
        inGameUI.enabled = false;
    }

    public static void ShowQuickMenu()
    {
        PauseGame();
        HideInGameUI();
        staticUI.enabled = true;
        quickUI.enabled = true;

    }

    public static void HideQuickMenu()
    {
        quickUI.enabled = false;
        staticUI.enabled = false;
        ShowInGameUI();
        ResumeGame();
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
        StringBuilder sb = new StringBuilder();
        language = (Language)languageChange;
        Translate.Instance.GetTranslate();
        
        sb.Append("Language");
        PlayerPrefs.SetInt(sb.ToString(), languageChange);
        PlayerPrefs.Save();
        
        if (!ReferenceEquals(GameDirector.Instance, null) && !ReferenceEquals(GameDirector.Instance.activeQuest, null))
        {
            GameDirector.Instance.UpdateQuestDescription();
        }
    }

    public static void PauseGame()
    {
        isPause = true;
        OnPauseGame?.Invoke(GameDirector.Instance, System.EventArgs.Empty);
    }

    public static void ResumeGame()
    {
        isPause = false;
        OnPauseGame?.Invoke(GameDirector.Instance, System.EventArgs.Empty);
    }

    public static float WaitInPause(float value)
    {
        return value + Time.deltaTime;
    }

    public static void ValueChangeCheck()
    {
        yesNoAmount.text = yesNoSlider.value.ToString();
    }

    public static void GetRandom()
    {
        random = Random.Range(0f, 1f);
    }

    public static bool InRandom(float value)
    {
        GetRandom();
        return random <= value;
    }

    public static float GetReachedDistance(CapsuleCollider2D collider)
    {
        Vector2 size = collider.size;
        return (size.x + size.y) / 2;
    }

    public static float GetAngleBetweenPositions(Vector3 pos1, Vector3 pos2)
    {
        Vector3 direction = pos1 - pos2;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public static string GetAsset(string path)
    {
#if UNITY_ANDROID && !UNITY_EDITOR_LINUX
        return GetRequest(Path.Combine(Application.streamingAssetsPath, path));
#else
        return new StringBuilder(Path.Combine(Application.streamingAssetsPath, path)).ToString();
#endif
    }

#if UNITY_ANDROID
    static string GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SendWebRequest();

            while (!webRequest.isDone) {}

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Debug.LogError(webRequest.error);
                return string.Empty;
            }

            return webRequest.downloadHandler.text;
        }
    }
#endif
    #endregion
}
