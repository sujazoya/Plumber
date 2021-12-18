using System.Collections;
using UnityEngine;

public class BaseController : MonoBehaviour
{
	public GameObject gameMaster;

	public string sceneName;

	protected virtual void Awake()
	{
		if (GameMaster.instance == null && gameMaster != null)
		{
			Object.Instantiate(gameMaster);
		}
	}

	protected virtual void Start()
	{
		if (JobWorker.instance.onEnterScene != null)
		{
			JobWorker.instance.onEnterScene(sceneName);
		}
	}

	public virtual void OnApplicationPause(bool pause)
	{
		UnityEngine.Debug.Log("On Application Pause");
		if (!pause)
		{
			Timer.Schedule(this, 0.5f, delegate
			{
				//CUtils.ShowInterstitialAd();
			});
		}
	}

	private IEnumerator SavePrefs()
	{
		while (true)
		{
			yield return new WaitForSeconds(5f);
			PlayerPrefs.Save();
		}
	}
}
