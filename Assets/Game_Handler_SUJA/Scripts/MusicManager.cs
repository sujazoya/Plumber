using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SettingMenu
{
	public GameObject settingPanel;
	public Button     settingButton;
	public Button     settingCloseButton;
	public Button     mainSoundButton;
	public GameObject mainSoundoff;
	public Button[] musicButton;
	public GameObject[] offGameobject;

}

public class MusicManager : MonoBehaviour
{
    [Header("Every Musib Need Unique Key")]
	public  string musicKey;
	public static string currentMusicKey;   
    public AudioClip[] music;					// list of available music tracks
    private AudioSource musicAudio;				// AudioSource component for playing music  
    public static MusicManager mymusicManager;	
	[SerializeField] SettingMenu settingMenu;
	string mainSoundKey = "MySound";
	string currentSoundKey;
	[SerializeField]AudioListener mainAudio;
	
	// Start is called before the first frame update
	void Awake()
    {

	   currentSoundKey = mainSoundKey;
		mainAudio = Camera.main.transform.GetComponent<AudioListener>();
		currentMusicKey = musicKey;
		if (mymusicManager != null)
		{
			Debug.LogError("More than one SoundManager found in the scene");
			return;
		}
		mymusicManager = this;
		
		if(!GetComponent <AudioSource>())
        {
			musicAudio = gameObject.AddComponent<AudioSource>();
        }
        else
        {
			musicAudio=GetComponent<AudioSource>();
		}				
		musicAudio.playOnAwake = false;
		musicAudio.loop = true;
        if (settingMenu.musicButton.Length > 0)
        {
            for (int i = 0; i < settingMenu.musicButton.Length; i++)
            {
				settingMenu.musicButton[i].onClick.AddListener(ToggleMusic);
			}
        }
        if (settingMenu.offGameobject.Length > 0)
        {
			if (!PlayerPrefs.HasKey(musicKey))
			{				
				for (int i = 0; i < settingMenu.offGameobject.Length; i++)
				{
					settingMenu.offGameobject[i].SetActive(false);
				}		

			}
			else
			{
				for (int i = 0; i < settingMenu. offGameobject.Length; i++)
				{
					settingMenu.offGameobject[i].SetActive(true);
				}				
			}
		}

    }
    private void Start()
    {
		if (!PlayerPrefs.HasKey(mainSoundKey))
		{			
			mainAudio.enabled = true;
			settingMenu.mainSoundoff.SetActive(false);
		}
		else
		{
			mainAudio.enabled = false;
			settingMenu.mainSoundoff.SetActive(true);
		}
        if (settingMenu.settingButton)
        {
			settingMenu.settingButton.onClick.AddListener(OpenSttingMenu);		
			
			;
		}
		if (settingMenu.settingCloseButton)
        {
			settingMenu.settingCloseButton.onClick.AddListener(CloseSttingMenu);
		}
        if (settingMenu.mainSoundButton)
        {
			settingMenu.mainSoundButton.onClick.AddListener(ToggleMainSound);
		}
		settingMenu.settingPanel.SetActive(false);
	}

	void OpenSttingMenu()
    {
		settingMenu.settingPanel.SetActive(true);
	}
	void CloseSttingMenu()
	{
		settingMenu.settingPanel.SetActive(false);
	}
	public void ToggleMusic(AudioSource musicName)
	{
		if (!PlayerPrefs.HasKey(musicKey))
		{
			PlayerPrefs.SetString(musicKey, musicKey);
			currentMusicKey = musicKey;
			musicName.Stop();
            if (settingMenu.offGameobject.Length > 0)
            {
                for (int i = 0; i < settingMenu.offGameobject.Length; i++)
                {
					settingMenu.offGameobject[i].SetActive(true);

				}
            }

        }
        else
        {
			PlayerPrefs.DeleteKey(musicKey);
			currentMusicKey = string.Empty;
			musicName.Play();
			if (settingMenu.offGameobject.Length > 0)
			{
				for (int i = 0; i < settingMenu.offGameobject.Length; i++)
				{
					settingMenu.offGameobject[i].SetActive(false);
				}
			}
		}
	}
	void ToggleMusic()
    {
		if (!PlayerPrefs.HasKey(musicKey))
		{
			PlayerPrefs.SetString(musicKey, musicKey);
			currentMusicKey = musicKey;
			musicAudio.Stop();
			if (settingMenu.offGameobject.Length > 0)
			{
				for (int i = 0; i < settingMenu.offGameobject.Length; i++)
				{
					settingMenu.offGameobject[i].SetActive(true);

				}
			}

		}
		else
		{
			PlayerPrefs.DeleteKey(musicKey);
			currentMusicKey = string.Empty;
			musicAudio.Play();
			if (settingMenu.offGameobject.Length > 0)
			{
				for (int i = 0; i < settingMenu. offGameobject.Length; i++)
				{
					settingMenu.offGameobject[i].SetActive(false);
				}
			}
		}
	}
	void ToggleMainSound()
	{
		if (!PlayerPrefs.HasKey(mainSoundKey))
		{
			PlayerPrefs.SetString(mainSoundKey, mainSoundKey);
			currentSoundKey = mainSoundKey;
			mainAudio.enabled=false;			
			settingMenu.mainSoundoff.SetActive(true);
		}
		else
		{
			PlayerPrefs.DeleteKey(mainSoundKey);
			currentMusicKey = string.Empty;
			mainAudio.enabled = true;
			settingMenu.mainSoundoff.SetActive(false);
		}
	}

	public static void PlayMusic(string trackName)
    {
        if (mymusicManager == null)
        {
            Debug.LogWarning("Attempt to play a sound with no SoundManager in the scene");
            return;
        }

        if (!PlayerPrefs.HasKey(currentMusicKey))
        {
			mymusicManager.musicAudio.time = 0.0f;
			mymusicManager.musicAudio.volume = 1.0f;

			mymusicManager.PlaySound(trackName, mymusicManager.music, mymusicManager.musicAudio);
		}
        // reset track to beginning
      
    }

	/// <summary>
	/// Pauses the music.
	/// </summary>
	/// <param name="fadeTime">Fade out time.</param>
	public static void PauseMusic(float fadeTime)
	{
		if (!PlayerPrefs.HasKey(currentMusicKey))
		{
			if (fadeTime > 0.0f)
				mymusicManager.StartCoroutine(mymusicManager.FadeMusicOut(fadeTime));
			else
				mymusicManager.musicAudio.Pause();
		}
		
	}

	/// <summary>
	/// Unpauses the music.
	/// </summary>
	public static void UnpauseMusic()
	{
		if (!PlayerPrefs.HasKey(currentMusicKey))
		{
			mymusicManager.musicAudio.volume = 1.0f;
			mymusicManager.musicAudio.Play();
		}
		
	}
	public static void StopMusic()
    {
		if (!mymusicManager.musicAudio)
			return;
        if (mymusicManager.musicAudio.isPlaying)
        {
			mymusicManager.musicAudio.Stop();
		}
	 
	}

	/// <summary>
	/// Plays a sound using a given AudioSource
	/// </summary>
	private void PlaySound(string soundName, AudioClip[] pool, AudioSource audioOut)
	{
		// loop through our list of clips until we find the right one.
		foreach (AudioClip clip in pool)
		{
			if (clip.name == soundName)
			{
				PlaySound(clip, audioOut);
				return;
			}
		}

		Debug.LogWarning("No sound clip found with name " + soundName);
	}

	/// <summary>
	/// Plays a sound using a given AudioSource
	/// </summary>
	private void PlaySound(AudioClip clip, AudioSource audioOut)
	{
		audioOut.clip = clip;
		audioOut.Play();
	}

	/// <summary>
	/// Co-Routine for fading out the music
	/// </summary>
	/// <param name="time">Fade time</param>
	IEnumerator FadeMusicOut(float time)
	{
		float startVol = musicAudio.volume;
		float startTime = Time.realtimeSinceStartup;

		while (true)
		{
			// use realtimeSinceStartup because Time.time doesn't increase when the game is paused.
			float t = (Time.realtimeSinceStartup - startTime) / time;
			if (t < 1.0f)
			{
				musicAudio.volume = (1.0f - t) * startVol;
				yield return 0;
			}
			else
			{
				break;
			}
		}

		// once we've fully faded out, pause the track
		musicAudio.Pause();
	}
}
