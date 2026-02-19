using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum Muro
{
    Avanti,
    Destra,
    Dietro,
    Sinistra
}

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _frontWall;
    [SerializeField] private GameObject _backWall;
    [SerializeField] private GameObject _lucernario;

    [SerializeField] private GameObject _MuroColliderAMeta;

    //front, right, back, left
    private bool[] EnabledWall = {true, true, true,true};
    private bool[] WallOccupato = {false, false, false, false};
    private int[] AngoloYDaApplicarePerMuro = { -90, 0, 90, 180 };
    public bool IsDecorato = false;

    public bool IsVisited { get; private set; }

   public int WallWidth { 
        get
        {
            return (int) _leftWall.transform.GetChild(0).localScale.z;
        } 
    }

    public int WallDepth
    {
        get
        {
            return (int)_frontWall.transform.GetChild(0).localScale.z;
        }
    }

    public bool PossibilitaQuadro
    {
        get
        {
            int count = 0;
            int index = -1;

            bool[] tmp = new bool[2 + EnabledWall.Length];

            tmp[0] = EnabledWall[EnabledWall.Length - 1];
            tmp[tmp.Length - 1] = EnabledWall[0];
            for (int i = 1; i < 1 + EnabledWall.Length; i++)
            {
                tmp[i] = EnabledWall[i - 1];
            }

            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i])
                    count++;
                else
                    count = 0;

                if (count == 3 && index == tmp.Length - 1)
                    index = i - 1;
                else if (count == 3)
                    index = i - 2;
            }

            return (index == -1) ? false : true;
        }
    }

    public void Visit()
    {
        IsVisited = true;
    }

    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
        EnabledWall[3] = false;
        WallOccupato[3] = true;
    }

    public void ClearRighttWall()
    {
        _rightWall.SetActive(false);
        EnabledWall[1] = false;
        WallOccupato[1] = true;
    }

    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
        EnabledWall[0] = false;
        WallOccupato[0] = true;
    }

    public void ClearBackWall()
    {
        _backWall.SetActive(false);
        EnabledWall[2] = false;
        WallOccupato[2] = true;
    }

    public void InserisciLucernario()
    {
        List<int> index = new List<int>(4);
        for (int i = 0; i < WallOccupato.Length; i++) {
            if (!WallOccupato[i]) { 
                index.Add(i);
            }
        }

        System.Random r = new System.Random();

        if (index.Count == 0)
            return;

        int ind = r.Next(0, index.Count);
        ind = index[ind];

        Transform Wall;
        switch (ind)
        {
            case 0:
                Wall = _frontWall.transform.GetChild(0);
                break;
            case 1:
                Wall = _rightWall.transform.GetChild(0);
                break;
            case 2:
                Wall = _backWall.transform.GetChild(0);
                break;
            case 3:
                Wall = _leftWall.transform.GetChild(0);
                break;
            default:
                Wall = this.transform;
                break;
        }
        Instantiate<GameObject>(_lucernario, Wall.position, Quaternion.RotateTowards(Wall.rotation, Quaternion.Euler(0, AngoloYDaApplicarePerMuro[ind], 0), 360));
        IsDecorato = true;
    }

    public void InserisciQuadro(GameObject quadro)
    {
        if (string.Equals(quadro.name, "Errore"))
            return;

        int count = 0;
        int index = 0;

        bool[] tmp = new bool[2 + EnabledWall.Length];

        tmp[0] = EnabledWall[EnabledWall.Length - 1];
        tmp[tmp.Length - 1] = EnabledWall[0];
        for (int i = 1; i < 1 + EnabledWall.Length; i++) 
        {
            tmp[i] = EnabledWall[i - 1];
        }

        for (int i = 0; i < tmp.Length; i++) {
            if (tmp[i])
                count++;
            else
                count = 0;

            if (count == 3 && index == tmp.Length - 1)
                index = i - 1;
            else if (count == 3)
                index = i - 2;
        }

        if (index >= EnabledWall.Length)
            index = EnabledWall.Length - 1;

        Transform Wall;
        Vector3 t;
        switch (index)
        {
            case 0:
                Wall = _frontWall.transform.GetChild(0);
                t = new Vector3(0, 0.5f, -0.2f);
                break;
            case 1:
                Wall = _rightWall.transform.GetChild(0);
                t = new Vector3(-0.2f, 0.5f, 0);
                break;
            case 2:
                Wall = _backWall.transform.GetChild(0);
                t = new Vector3(0, 0.5f, 0.2f);
                break;
            case 3:
                Wall = _leftWall.transform.GetChild(0);
                t = new Vector3(0.2f, 0.5f, 0);
                break;
            default:
                Wall = this.transform;
                t = new Vector3(0, 0.5f, 0);
                break;
        }

        Instantiate<GameObject>(quadro, Wall.position + t, Quaternion.RotateTowards(Wall.rotation, Quaternion.Euler(0, AngoloYDaApplicarePerMuro[index] - 90, 0), 360));
        IsDecorato = true;
    }

    public void InserisciForzatamenteQuadroIn(GameObject quadro, Muro muro, Quaternion initRotation)
    {
        Transform Wall;
        Vector3 t;
        switch (muro)
        {
            case Muro.Avanti:
                Wall = _frontWall.transform.GetChild(0);
                t = new Vector3(0.2f, 0.5f, -0.2f);
                break;
            case Muro.Destra:
                Wall = _rightWall.transform.GetChild(0);
                t = new Vector3(0.2f, 0.5f, -0.2f);
                break;
            case Muro.Dietro:
                Wall = _backWall.transform.GetChild(0);
                t = new Vector3(-0.2f, 0.5f, 0.2f);
                break;
            case Muro.Sinistra:
                Wall = _leftWall.transform.GetChild(0);
                t = new Vector3(-0.2f, 0.5f, 0.2f);
                break;
            default:
                Wall = this.transform;
                t = new Vector3(0, 0.5f, 0);
                break;
        }

        GameObject q = Instantiate<GameObject>(quadro, Wall.position + t, (muro == Muro.Dietro) ? initRotation :
                initRotation * Quaternion.Euler(0, 180, 0)
            );
        IsDecorato = true;
    }

    public void MetteMuroPerLaPorta(Muro muro)
    {
        GameObject padre;
        switch (muro)
        {
            case Muro.Avanti:
                padre = _frontWall;
                break;
            case Muro.Destra:
                padre = _rightWall;
                break;
            case Muro.Dietro:
                padre = _backWall;
                break;
            case Muro.Sinistra:
                padre = _leftWall;
                break;
            default:
                padre = _frontWall;
                break;
        }

        GameObject t = Instantiate<GameObject>(_MuroColliderAMeta, padre.transform.GetChild(0).position, padre.transform.GetChild(0).rotation);
        BoxCollider rg = padre.transform.GetChild(0).transform.GetComponent<BoxCollider>();
        rg.enabled = false;
        Destroy(padre.transform.GetChild(0).gameObject);
        padre.transform.SetParent(t.transform);
        
    }
}
