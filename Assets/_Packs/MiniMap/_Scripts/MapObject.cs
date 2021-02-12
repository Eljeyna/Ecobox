using UnityEngine;
using UnityEngine.UI;

public class MapObject : MonoBehaviour
{
	MiniMapController mmc;
	GameObject owner;
	Camera mapCamera;
	Image spr;
	GameObject panelGO;

	Vector3 viewPortPos;
	RectTransform rt;
	Vector3[] cornerss = new Vector3[4];

	RectTransform sprRect;
	Vector2 screenPos;

	private void OnDestroy()
	{
		mmc.target = null;
		mapCamera = null;
		owner = null;
	}

	void Update()
	{
		if (ReferenceEquals(owner, null))
			Destroy(gameObject);
		else
			SetPositionCam();
	}

	public void SetMiniMapEntityValues(MiniMapController controller, MiniMapEntity mme, GameObject attachedGO, Camera renderCamera, GameObject parentPanelGO)
	{
		owner = attachedGO;
		mapCamera = renderCamera;
		panelGO = parentPanelGO;
		spr = gameObject.GetComponent<Image>();
		spr.sprite = mme.icon;
		sprRect = spr.gameObject.GetComponent<RectTransform>();
		sprRect.sizeDelta = mme.size;
		rt = panelGO.GetComponent<RectTransform>();
		mmc = controller;
		SetPositionCam();
	}

	void SetPositionCam()
	{
		transform.SetParent(panelGO.transform, false);
		SetPosition();
	}
	
	void SetPosition()
	{
		if (!owner || !owner.transform || !mmc.target)
		{
			return;
		}

		rt.GetWorldCorners(cornerss);
		screenPos = RectTransformUtility.WorldToScreenPoint(mapCamera, owner.transform.position);
		
		sprRect.anchoredPosition = screenPos-rt.sizeDelta/2f;
	}
}
