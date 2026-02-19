using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolQuadri : MonoBehaviour
{
    [SerializeField] private List<Texture2D> Dipinti;
    [SerializeField] private PictureFramesManager _cornice;

    private int currentindex = 0;

    public int Punteggio;

    public int QuadriRimasti
    {
        get
        {
            return Dipinti.Count;
        }
    }

    public GameObject GetQuadro(bool PuoEssereRaccolto)
    {
        if(currentindex >= Dipinti.Count)
            return new GameObject("Errore");

        PictureFramesManager tmp = _cornice;
        tmp.Image = Dipinti[currentindex];
        tmp.Pool = this;
        tmp.puoEssereRaccolto = PuoEssereRaccolto;
        currentindex++;
        tmp.Initialize();

        return tmp.gameObject;
    }

    public void ResetIndexPool()
    {
        currentindex = 0;
    }
}
