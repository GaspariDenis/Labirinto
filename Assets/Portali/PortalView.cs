using DoorScript;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PortalView : MonoBehaviour
{
    public Door Current_Door;
    public PortalView Portale_Opposto;
    public Camera Camera_Propietaria;
    public Shader Shader_Portale;
    public GameObject Portal_View;

    private MeshRenderer _Mesh_Portale {
        get { return Portal_View.GetComponent<MeshRenderer>(); }
    }
    private Material _Material_Portale;

    // Start is called before the first frame update
    void Start()
    {
        Portale_Opposto.Camera_Propietaria.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        _Material_Portale = new Material(Shader_Portale);
        _Material_Portale.mainTexture = Portale_Opposto.Camera_Propietaria.targetTexture;
        _Mesh_Portale.material = _Material_Portale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Current_Door.open)
        {
            Portal_View.SetActive(false);
        }
        else
        {
            Portal_View.SetActive(true);
        }

        //Così le camere dei portali fanno gli stessi movimenti della main camera
        Vector3 Look = Portale_Opposto.transform.worldToLocalMatrix.MultiplyPoint3x4(Camera.main.transform.position);
        Look = new Vector3(-Look.x, Look.y, -Look.z);
        Camera_Propietaria.transform.localPosition = Look;

        //Rotazione
        Quaternion diff = transform.rotation * Quaternion.Inverse(Portale_Opposto.transform.rotation * Quaternion.Euler(0, 180, 0));
        Camera_Propietaria.transform.rotation = diff * Camera.main.transform.rotation;

        //Clipping
        Camera_Propietaria.nearClipPlane = Look.magnitude;
    }
}
