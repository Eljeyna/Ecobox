using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
[ExecuteInEditMode]
public class MiniMapController : MonoBehaviour
{
	//public bool isRadialMask = false;
	//public Vector2 radialPadding = new Vector2(0.2f,0.2f);
	//[HideInInspector]
	public RenderTexture renderTex;
	//[HideInInspector]
	public Material mapMaterial;
	//[HideInInspector]
	public GameObject iconPref;
	//[Tooltip("The minimap rendering camera")]
	[HideInInspector]
	public Camera mapCamera;

	[Tooltip("The target which the minimap will be following")]
	public Transform target;
	//UI related variables
	[Tooltip("Set which layers to show in the minimap")]
	public LayerMask minimapLayers;
	[Tooltip("Set this true, if you want minimap border as background of minimap")]
	public bool showBackground;
	[Tooltip("The mask to change the shape of minimap")]
	public Sprite miniMapMask;
	[Tooltip("border graphics of the minimap")]
	public Sprite miniMapBorder;
	[Tooltip("Set opacity of minimap")]
	[Range(0,1)]
	public float miniMapOpacity=1;
	[Tooltip("border graphics of the minimap")]
	public Vector3 miniMapScale = new Vector3(1,1,1);

	//Render camera related variables
	[Tooltip("Camera offset from the target")]
	public Vector3 cameraOffset = new Vector3(0f, 7.5f, 0f);
	[Tooltip("Camera's orthographic size")]
	public float camSize = 15;
	[Tooltip("Camera's far clip")]
	public float camFarClip = 1000;
	[Tooltip("Adjust the rotation according to your scene")]
	public Vector3 rotationOfCam = new Vector3(90,0,0);
	[Tooltip("If true the camera rotates according to the target")]
	public bool rotateWithTarget = true;
	public Dictionary<GameObject, GameObject> ownerIconMap = new Dictionary<GameObject, GameObject>();

	private GameObject miniMapPanel;
	private Image mapPanelMask;
	private Image mapPanelBorder;
	private Image mapPanel;
	private Color mapColor;
	private Color mapBorderColor;

	private RectTransform mapPanelRect;
	private RectTransform mapPanelMaskRect;

	private Vector3 prevRotOfCam;
	Vector2 res;
	Image miniMapPanelImage;

	public void OnEnable()
	{
		ownerIconMap.Clear();
		GameObject maskPanelGO = transform.GetComponentInChildren<Mask>().gameObject;
		mapPanelMask = maskPanelGO.GetComponent<Image>();
		mapPanelBorder = maskPanelGO.transform.parent.GetComponent<Image>();
		miniMapPanel = maskPanelGO.transform.GetChild (0).gameObject;
		mapPanel = miniMapPanel.GetComponent<Image>();
		mapColor = mapPanel.color;
		mapBorderColor = mapPanelBorder.color;
		if(mapCamera==null) mapCamera = transform.GetComponentInChildren<Camera>();
		mapCamera.cullingMask = minimapLayers;

		mapPanelMaskRect = maskPanelGO.GetComponent<RectTransform>();
		mapPanelRect = miniMapPanel.GetComponent<RectTransform>();
		mapPanelRect.anchoredPosition = mapPanelMaskRect.anchoredPosition;
		res = new Vector2(Screen.width,Screen.height);

		miniMapPanelImage = miniMapPanel.GetComponent<Image>();
		miniMapPanelImage.enabled = !showBackground;
		SetupRenderTexture();
	}
	
	void OnDisable()
	{
		if (renderTex != null)
		{
			if (!renderTex.IsCreated())
			{
				renderTex.Release();
			}
		}
	}

	void OnDestroy()
	{
		if (renderTex != null)
		{
			if (!renderTex.IsCreated())
			{
				renderTex.Release();
			}
		}
	}

	public void LateUpdate()
	{
		mapPanelMask.sprite = miniMapMask;
		mapPanelBorder.sprite = miniMapBorder;
		mapPanelBorder.rectTransform.localScale = miniMapScale;
		mapBorderColor.a = miniMapOpacity;
		mapColor.a = miniMapOpacity;
		mapPanelBorder.color = mapBorderColor;
		mapPanel.color = mapColor;

		mapPanelMaskRect.sizeDelta = new Vector2(Mathf.RoundToInt(mapPanelMaskRect.sizeDelta.x),Mathf.RoundToInt(mapPanelMaskRect.sizeDelta.y));
		mapPanelRect.position = mapPanelMaskRect.position;
		mapPanelRect.sizeDelta = mapPanelMaskRect.sizeDelta;
		miniMapPanelImage.enabled = !showBackground;

		if (Screen.width != res.x || Screen.height != res.y)
		{
			SetupRenderTexture();
			res.x = Screen.width;
			res.y = Screen.height;
		}
		
		SetCam();
	}
	
	void SetupRenderTexture()
	{
		if (renderTex.IsCreated()) renderTex.Release();
		//Setup render texture and resize it.
		//New render texture was created, as premade render texture's size can't be changed
		renderTex = new RenderTexture ((int)mapPanelRect.sizeDelta.x, (int)mapPanelRect.sizeDelta.y, 24);
		//Create only creates new render texture in memory, if it is not already created
		renderTex.Create();

		mapMaterial.mainTexture = renderTex;
		mapCamera.targetTexture = renderTex;

		//Hack to refresh the minimap panel texture;
		mapPanelMaskRect.gameObject.SetActive(false);
		mapPanelMaskRect.gameObject.SetActive(true);
	}

	void SetCam()
	{
		mapCamera.orthographicSize = camSize;
		mapCamera.farClipPlane = camFarClip;
		if (target == null)
		{
			return;
		}
		
		mapCamera.transform.eulerAngles = rotationOfCam;

		if (rotateWithTarget)
		{
			mapCamera.transform.eulerAngles = target.eulerAngles + rotationOfCam;
		}
		mapCamera.transform.position = target.position + cameraOffset;
	}

	//Register's minimap objects here
	public MapObject RegisterMapObject(GameObject owner, MiniMapEntity mme)
	{
		GameObject curMGO = Instantiate(iconPref);
		MapObject curMO = curMGO.AddComponent<MapObject>();
		
		if (curMO.TryGetComponent(out Image image))
		{
			image.raycastTarget = false;
		}
		
		curMO.SetMiniMapEntityValues(this, mme,owner, mapCamera, miniMapPanel);
		ownerIconMap.Add(owner, curMGO);
		return owner.GetComponent<MapObject>();
	}

	//Unregister's minimap objects here
	public void UnregisterMapObject(MapObject mmo, GameObject owner)
	{
		if (!mmo)
		{
			return;
		}
		
		if (ownerIconMap.TryGetValue(owner, out GameObject valueOwner))
		{
			Destroy(ownerIconMap[owner]);
			ownerIconMap.Remove(owner);
		}
		
		Destroy(mmo);
	}
}
