using DoorScript;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridoio : MonoBehaviour
{
    [SerializeField] private Transform initPoint;
    [SerializeField] private MazeCell prefabCell;
    [SerializeField] private GameObject Entrata;
    [SerializeField] private GameObject Uscita;
    [SerializeField] private GameObject Lampada;
    public bool CreationTrigger;
    [SerializeField] private Transform Pavimento;
    [SerializeField] private Transform Soffitto;
    private bool AlreadyInit = false;

    public int lunghezzaCorridoio;
    public PoolQuadri pool;

    private MazeCell[] corridoio;

    private void Update()
    {
        if(CreationTrigger && !AlreadyInit)
        {
            Initialize();
            AlreadyInit = true;
        }
    }

    private void Initialize()
    {
        //Crea Corridoio
        corridoio = new MazeCell[lunghezzaCorridoio];
        for (int i = 0; i < corridoio.Length; i++)
        {
            Vector3 p = initPoint.position + initPoint.right * i * prefabCell.WallWidth;

            corridoio[i] = Instantiate<MazeCell>(prefabCell, p/*new Vector3(
                    initPoint.position.x +  * i * prefabCell.WallWidth,
                    initPoint.position.y,
                    initPoint.position.z
                )*/,
                initPoint.rotation
            );

            if (i != 0)
                corridoio[i].ClearLeftWall();
            if(i != corridoio.Length - 1)
            corridoio[i].ClearRighttWall();
        }
        //corridoio[0].MetteMuroPerLaPorta(Muro.Sinistra);
        //corridoio[corridoio.Length-1].MetteMuroPerLaPorta(Muro.Destra);

        //Pavimento e Soffitto
        Pavimento.transform.position = initPoint.position + initPoint.right * (lunghezzaCorridoio - 1) * prefabCell.WallWidth / 2;

        Pavimento.transform.localScale = new Vector3(
                lunghezzaCorridoio * prefabCell.WallWidth + 3,
                Pavimento.transform.localScale.y,
                prefabCell.WallDepth
            );
        Pavimento.rotation = initPoint.transform.rotation;

        Soffitto.transform.position = new Vector3(
            Pavimento.transform.position.x,
            Soffitto.transform.position.y,
            Pavimento.transform.position.z
            );

        Soffitto.transform.localScale = new Vector3(
                lunghezzaCorridoio * prefabCell.WallWidth,
                Soffitto.transform.localScale.y,
                prefabCell.WallDepth
            );
        Soffitto.transform.rotation = initPoint.transform.rotation;

        //Inserisco Luci
        for (int i = 0; i < corridoio.Length - 1; i++)
        {
            if (i % 2 != 0)
            {
                corridoio[i].InserisciLucernario();
            }
        }

        //Inserisco Quadri
        pool.ResetIndexPool();

        for (int i = 0; i < pool.Punteggio; i++)
        {
            corridoio[(i % 2 != 0) ? i - 1 : i].InserisciForzatamenteQuadroIn(pool.GetQuadro(false), (i % 2 == 0) ? Muro.Avanti : Muro.Dietro, initPoint.transform.rotation);
        }

        //Posizionamento Porte
        Entrata.transform.position = initPoint.position + initPoint.right * -prefabCell.WallDepth / 2;
        Entrata.transform.position = new Vector3(
            Entrata.transform.position.x,
            Entrata.GetComponent<BoxCollider>().size.y / 2 - 0.2f + Entrata.transform.position.y,
            Entrata.transform.position.z + 0.2f);
        //Entrata.transform.rotation = Quaternion.Inverse(initPoint.transform.rotation);

        Uscita.transform.position = initPoint.position + initPoint.right * (lunghezzaCorridoio) * prefabCell.WallDepth;
        Uscita.transform.position = new Vector3(
                Uscita.transform.position.x,
                Uscita.transform.position.y + Entrata.GetComponent<BoxCollider>().size.y / 2 - 0.2f,
                Uscita.transform.position.z - prefabCell.WallDepth / 2 - 0.6f
            );
    }
}
