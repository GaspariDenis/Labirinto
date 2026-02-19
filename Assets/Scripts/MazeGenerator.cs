using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private Transform _initPoint;

    [SerializeField] private MazeCell _cellPrefab;

    [SerializeField] private GameObject Entrata;
    [SerializeField] private GameObject Uscita;

    [SerializeField] private int _mazeWidth;

    [SerializeField] private int _mazeDepth;

    [SerializeField] private Transform _pavimento;
    [SerializeField] private Transform _Soffitto;

    [SerializeField] private PoolQuadri Pool;

    [SerializeField] private Timer cronometro;

    private MazeCell[,] _maze;

    private void OnTriggerEnter(Collider other)
    {
        cronometro.InBox = true;
    }

    private void OnTriggerExit(Collider other)
    {
        cronometro.InBox = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _maze = new MazeCell[_mazeWidth, _mazeDepth];

        for(int x = 0; x < _mazeWidth; x++) 
        {
            for(int z = 0; z < _mazeDepth; z++)
            {
                _maze[x,z] = Instantiate(_cellPrefab, new Vector3(x * _cellPrefab.WallWidth, 0, z * _cellPrefab.WallDepth) + _initPoint.transform.position, Quaternion.identity);
            }
        }

        _pavimento.localScale = (_pavimento.localScale + new Vector3(_mazeWidth, 0, _mazeDepth));
        _pavimento.localScale = new Vector3(_pavimento.localScale.x * _cellPrefab.WallWidth, _pavimento.localScale.y, _pavimento.localScale.z * _cellPrefab.WallDepth);
        _pavimento.position = new Vector3( _mazeWidth * _cellPrefab.WallWidth / 2 + _pavimento.position.x, _pavimento.position.y,_mazeDepth * _cellPrefab.WallDepth / 2 + _pavimento.position.z);
        
        _Soffitto.localScale = _pavimento.localScale;
        _Soffitto.position = new Vector3(_pavimento.position.x, _Soffitto.position.y, _pavimento.position.z);

        Entrata.transform.position = new Vector3(
            _initPoint.position.x + _mazeDepth * _cellPrefab.WallDepth - _cellPrefab.WallWidth,
            _initPoint.position.y + Entrata.GetComponent<BoxCollider>().size.y / 2 - 0.25f,
            _initPoint.position.z - _cellPrefab.WallDepth / 2 + 0.2f
            );
        _maze[_mazeWidth - 1, 0].IsDecorato = true;

        //_maze[0,_mazeDepth].ClearRighttWall();
        Uscita.transform.position = new Vector3(
            _initPoint.position.x /*- 1.4f*/,
            _initPoint.position.y + Entrata.GetComponent<BoxCollider>().size.y / 2 - 0.25f,
            _initPoint.position.z + _mazeDepth * _cellPrefab.WallDepth -_cellPrefab.WallDepth / 2 -0.25f /*- 0.7f*/
            );
        //Uscita.transform.rotation = Quaternion.RotateTowards(Uscita.transform.rotation, Quaternion.Euler(0, -45, 0), 360);

        _maze[0, _mazeDepth - 1].MetteMuroPerLaPorta(Muro.Avanti);

        _maze[0, _mazeDepth - 1].IsDecorato = true;

        GenerateMaze(null, _maze[0,0]);

        //Inserisci Quadri
        List<MazeCell> Possibili = new List<MazeCell>();
        for (int i = 0; i < _mazeWidth; i++)
        {
            for (int j = 0; j < _mazeDepth; j++)
            {
                if(_maze[i, j].PossibilitaQuadro && !_maze[i,j].IsDecorato)
                {
                    Possibili.Add(_maze[i, j]);
                }
            }
        }

        for (int i = 0; i < Possibili.Count; i++) {

            if (Pool.QuadriRimasti < 0)
                break;

            System.Random r = new System.Random();

            MazeCell tmp = Possibili[r.Next(0, Possibili.Count)];

            tmp.InserisciQuadro(Pool.GetQuadro(true));

            Possibili.Remove(tmp);
        }


        //Inserisce le luci
        for (int i = 0; i < _mazeWidth; i++)
        {
            for (int j = 0; j < _mazeDepth; j++)
            {
                if(j % 2 == 0)
                    continue;
                if (!_maze[i, j].IsDecorato)
                    _maze[i, j].InserisciLucernario();
            }
        }

        //Box Collider
        BoxCollider box = this.gameObject.GetComponent<BoxCollider>();
        BoxCollider prefab = _cellPrefab.gameObject.GetComponent<BoxCollider>();
        box.size = new Vector3(
                _mazeWidth * (_cellPrefab.WallWidth + 1),
                2f+ 2f,
                _mazeDepth * (_cellPrefab.WallDepth + 1)
            );

        box.center = new Vector3(
                _mazeWidth * _cellPrefab.WallWidth / 2,
                3f / 2,
                _mazeDepth * _cellPrefab.WallDepth / 2
            );
    }

    private void GenerateMaze(MazeCell previous, MazeCell current)
    {
        current.Visit();
        ClearWalls(previous, current);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(current);

            if (nextCell != null)
            {
                GenerateMaze(current, nextCell);
            }
        }while(nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell current) 
    {
        var unvisited = GetUnivisitedCell(current);
        return unvisited.OrderBy(_ => Random.Range(0, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnivisitedCell(MazeCell current)
    {
        int x = (int)Mathf.Abs(_initPoint.transform.position.x - current.transform.position.x) / current.WallWidth;
        int z = (int)Mathf.Abs(_initPoint.transform.position.z - current.transform.position.z) / current.WallDepth;

        if (x + 1 < _mazeWidth)
        {
            var cellRight = _maze[x + 1, z];
            if (!cellRight.IsVisited)
            {
                yield return cellRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellLeft = _maze[x - 1, z];
            if (!cellLeft.IsVisited)
            {
                yield return cellLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellFront = _maze[x, z + 1];
            if (!cellFront.IsVisited)
            {
                yield return cellFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellBack = _maze[x, z - 1];
            if (!cellBack.IsVisited)
            {
                yield return cellBack;
            }
        }
    }

    private void ClearWalls(MazeCell previous, MazeCell current)
    {
        if(previous == null)
        {
            return;
        }

        if(previous.transform.position.x < current.transform.position.x)
        {
            previous.ClearRighttWall();
            current.ClearLeftWall();
            return;
        }

        if (previous.transform.position.x > current.transform.position.x)
        {
            previous.ClearLeftWall();
            current.ClearRighttWall();
            return;
        }

        if (previous.transform.position.z < current.transform.position.z)
        {
            previous.ClearFrontWall();
            current.ClearBackWall();
            return;
        }

        if (previous.transform.position.z > current.transform.position.z)
        {
            previous.ClearBackWall();
            current.ClearFrontWall();
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
