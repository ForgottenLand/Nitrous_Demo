using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_ANDROID
public class AdBuddizBinding
{
	private static AndroidJavaObject _AdBuddizPlugin;

	static AdBuddizBinding()
	{
		if (Application.platform != RuntimePlatform.Android)
			return;

		_AdBuddizPlugin = new AndroidJavaObject("com.purplebrain.adbuddiz.sdk.AdBuddizUnityBinding");
	}
	
	public static void cacheAds()
	{
		if (Application.platform != RuntimePlatform.Android)
			return;

		_AdBuddizPlugin.Call("cacheAds");
	}
			
	public static void showAd()
	{
		if (Application.platform != RuntimePlatform.Android)
			return;

		_AdBuddizPlugin.Call("showAd");
	}
	
	public static void showAd(string placementId)
	{
		if (Application.platform != RuntimePlatform.Android)
			return;

		_AdBuddizPlugin.Call("showAd", placementId);
	}
	
	public static void showToast(string text)
	{
		if (Application.platform != RuntimePlatform.Android)
			return;

		_AdBuddizPlugin.Call("showToast", text);
	}
}
#endif
