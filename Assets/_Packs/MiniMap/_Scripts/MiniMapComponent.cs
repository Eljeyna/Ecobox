using UnityEngine;

public class MiniMapEntity
{
	public Sprite icon;
	public Vector2 size;
}

public class MiniMapComponent : MonoBehaviour
{
	[Tooltip("Set the icon of this gameobject")]
	public Sprite icon;
	[Tooltip("Set size of the icon")]
	public Vector2 size = new Vector2(20,20);

	MiniMapController miniMapController;
	MiniMapEntity mme;
	MapObject mmo;

	void OnEnable()
	{
		miniMapController = GameObject.Find("CanvasMiniMap").GetComponent<MiniMapController>();
		mme = new MiniMapEntity();
		mme.icon = icon;
		mme.size = size;

		mmo = miniMapController.RegisterMapObject(this.gameObject, mme);
	}

	void OnDisable()
	{
		miniMapController.UnregisterMapObject(mmo, gameObject);
	}

	void OnDestroy()
	{
		miniMapController.UnregisterMapObject(mmo, gameObject);
	}
}
