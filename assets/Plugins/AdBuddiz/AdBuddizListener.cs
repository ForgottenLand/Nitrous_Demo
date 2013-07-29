using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_ANDROID
public class AdBuddizListener : MonoBehaviour
{
	public void OnEnable()
	{
		// to start caching ads as soon as your app starts
		AdBuddizBinding.cacheAds();
	}
	
	public void failToLoadAd(string cause)
	{
		//FIXME to remove before publishing to GooglePlay
		AdBuddizBinding.showToast(cause); 
	}
}
#endif
