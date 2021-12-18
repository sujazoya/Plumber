/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using GoogleMobileAds.Api;
using UnityEngine;
using System.Collections;
using suja;

public class RewardedVideoCallBack : MonoBehaviour
{
	private const string ACTION_NAME = "rewarded_video";

	private void Start()
	{
		///Timer.Schedule(this, 0.1f, CallAddEvents);
		///
		StartCoroutine(AddEvents());
	}
	public IEnumerator AddEvent()
	{		
		yield return new WaitUntil(() => AdmobAdmanager.readyToShoAd);
		Timer.Schedule(this, 0.1f, CallAddEvents);
	}
	void CallAddEvents()
    {
		StartCoroutine(AddEvents());
	}
	IEnumerator AddEvents()
	{
		yield return new WaitUntil(() => AdmobAdmanager.readyToShoAd);
		if (AdmobAdmanager.Instance.CurrentRewardedAd() != null)
		{
			AdmobAdmanager.Instance.CurrentRewardedAd().OnUserEarnedReward += HandleRewardBasedVideoRewarded;
		}
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
	}

	private void OnDestroy()
	{

		if (AdmobAdmanager.Instance.CurrentRewardedAd() != null)
		{
			AdmobAdmanager.Instance.CurrentRewardedAd().OnUserEarnedReward -= HandleRewardBasedVideoRewarded;
		}
	}
}
