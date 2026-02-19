using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PictureFramesManager : MonoBehaviour
{
    [Header("Image That Will Be Used For The Frame\n(Make Sure To Check The Preferred Size For Correct Image Scale)")]
    [SerializeField] public Texture2D Image;

    [Header("Used For The Glass Reflection On Lights")]
    [Range(0.0f, 1.0f)] public float GlassReflection = 0.5f;

    [Header("Objects For The Script")]
    [SerializeField] GameObject ImageObject;
    [SerializeField] GameObject GlassObject;

    public PoolQuadri Pool;
    public bool puoEssereRaccolto;

    private bool WasInit = false;

    Material FrameMaterial;
    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        if (WasInit)
            return;
        WasInit = true;

        if (Image != null)
        {
            Renderer Imagerenderer = ImageObject.GetComponent<Renderer>();
            Material ImageMaterial = Imagerenderer.sharedMaterial;
            ImageMaterial.mainTexture = Image;
        }

        Renderer Glassrenderer = GlassObject.GetComponent<Renderer>();
        Material[] FrameMaterials = Glassrenderer.sharedMaterials;
        for (int a = 0; a < FrameMaterials.Length; a++) {
            if (FrameMaterials[a].name.Contains("Picture Frames")) {
                FrameMaterial =FrameMaterials[a];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!WasInit)
            return;

        if (FrameMaterial != null) {
            FrameMaterial.SetFloat("_Metallic",GlassReflection);
        }

        if (PlayerTrigger)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                this.gameObject.SetActive(false);
                Pool.Punteggio++;
            }

        }
    }

    bool PlayerTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if(puoEssereRaccolto)
            PlayerTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerTrigger = false;
    }
}
