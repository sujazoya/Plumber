using MS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScene : MonoBehaviour
{
	[Header("Menu")]
	public Text starRanklbl;

	public Text coinLbl;

	public Image starRankBar;

	public GameObject groupList;

	[Header("Setting")]
	public Toggle soundToggle;

	public Toggle musicToggle;

	[Header("Level Detail View")]
	[SerializeField]
	private Popup LevelDetailPopup;

	[SerializeField]
	private Text LD_TitleLbl;

	[SerializeField]
	private Text LD_LevelCompletdLbl;

	[SerializeField]
	private GameObject LD_AwardGoldImage;

	[SerializeField]
	private Image LD_BGImage;

	[SerializeField]
	private ScrollRect LD_scrollRect;

	[SerializeField]
	private Transform LD_parentContent;

	[SerializeField]
	private Button LD_prefabBtn;

	[Header("")]
	public CanvasGroup[] canvasGroups;

	public Popup[] popups;

	public static HomeScene instance;

	private void Start()
	{
		instance = this;
		SetupSettingToggle();
		if (GameManager.openLevelSelection)
		{
			GameManager.openLevelSelection = false;
			List<LevelGroupButton> list = new List<LevelGroupButton>(groupList.GetComponentsInChildren<LevelGroupButton>());
			if (GameManager.currentLevelGroup != null && list.Exists((LevelGroupButton obj) => obj.levelGroup.Equals(GameManager.currentLevelGroup)))
			{
				ShowDetailLevel(list.Find((LevelGroupButton obj) => obj.levelGroup.Equals(GameManager.currentLevelGroup)));
			}
		}
		//Music.instance.Play(Music.Type.MainMusic);
		Application.targetFrameRate = 60;       
		//CUtils.ShowInterstitialAd();
	}

	private void SetupSettingToggle()
	{
		//soundToggle.isOn = Sound.instance.IsEnabled();
		//musicToggle.isOn = Music.instance.IsEnabled();
		//soundToggle.onValueChanged.AddListener(delegate(bool arg0)
		//{
		//	Sound.instance.SetEnabled(arg0);
		//	PlayButton();
		//});
		//musicToggle.onValueChanged.AddListener(delegate(bool arg0)
		//{
		//	Music.instance.SetEnabled(arg0, updateMusic: true);
		//	PlayButton();
		//});
		UpdateUI();
	}

	public void UpdateUI()
	{
		coinLbl.text = GameManager.Coin + string.Empty;
		starRanklbl.text = GameManager.StarLevel + string.Empty;
		starRankBar.fillAmount = GameManager.StarLevelProgress;
	}

	public void ShowDetailLevel(LevelGroupButton lb)
	{
		int completedLevel = lb.levelGroup.CompletedLevel;
		LD_TitleLbl.text = lb.levelGroup.LevelGroupName.ToUpper();
		LD_LevelCompletdLbl.text = completedLevel + " / " + lb.levelGroup.TotalLevel;
		LD_AwardGoldImage.SetActive(completedLevel >= lb.levelGroup.TotalLevel);
		//LD_BGImage.sprite = lb.levelGroup.LevelDetailBG;
		LD_AwardGoldImage.GetComponentInParent<Image>().sprite = lb.AwardGoldImage.GetComponentInParent<Image>().sprite;
		LD_LevelCompletdLbl.GetComponentInParent<Image>().sprite = lb.LevelCompletdLbl.GetComponentInParent<Image>().sprite;
		for (int num = LD_parentContent.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.DestroyImmediate(LD_parentContent.GetChild(0).gameObject);
		}
		for (int i = 0; i < lb.levelGroup.TotalLevel; i++)
		{
			Button button = UnityEngine.Object.Instantiate(LD_prefabBtn, LD_parentContent);
			button.name = lb.levelGroup.LevelGroupName + "_" + (i + 1);
			button.GetComponentInChildren<Text>().text = i + 1 + string.Empty;
			if (i <= completedLevel)
			{
				
				button.interactable = true;
				//button.GetComponent<Image>().color = Color.white;
				//button.GetComponentInChildren<Text>().color = lb.levelGroup.bgColor;
				int lNo = i + 1;
				button.onClick.AddListener(delegate
				{
					GameManager.currentLevelGroup = lb.levelGroup;
					GameManager.CurrentLevelNo = lNo;
					SceneManager.LoadScene("GameScene");
					PlayButton();
				});
			}
			else
			{				
				button.interactable = false;
				//button.GetComponent<Image>().color = lb.levelGroup.bgColor;
				//button.GetComponentInChildren<Text>().color = Color.white;
			}
		}
		LD_scrollRect.verticalNormalizedPosition = 1f;
		LevelDetailPopup.Open();
	}
	
	public void OnSendFeedback()
	{
		Sound.instance.PlayButton();
		Application.OpenURL("mailto:" + GameConfig.instance.feedbackEmail);
	}
	public void PlayButton()
	{
		Sound.instance.PlayButton();
	}

	public void PlayBackButton()
	{
		Sound.instance.PlayButton(Sound.Button.Back);
	}

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.Escape))
		{
			return;
		}
		bool flag = false;
		CanvasGroup[] array = canvasGroups;
		foreach (CanvasGroup canvasGroup in array)
		{
			if (canvasGroup.alpha != 0f)
			{
				flag = true;
				break;
			}
		}
		Popup[] array2 = popups;
		foreach (Popup popup in array2)
		{
			if (popup.isOpen)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Application.Quit();
		}
	}
}
