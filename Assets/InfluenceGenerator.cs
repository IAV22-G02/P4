using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts.G02
{
    public class InfluenceGenerator : MonoBehaviour
    {
        private static InfluenceGenerator instance_;
        
        [SerializeField]
        GameObject container;

        [SerializeField]
        GameObject cell;

        private int filas, columnas;
        private Grid grid;

        private InfluenceCellBehaviour[,] matriz;
        //  Para ocultar el mapa
        private bool visible = false;
        //  Terrain del escenario
        private Terrain terrain;

        Vector3 posIni;

        private InfluenceCellBehaviour MaxprioAzul;
        private InfluenceCellBehaviour MaxprioAmarillo;

        public Grid getMapGrid() { return grid; }

        private void Awake()
        {
            if (instance_ == null)
            {
                instance_ = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public static InfluenceGenerator GetInstance()
        {
            return instance_;
        }

        //  Construye las casillas a lo largo del mapa
        void Start()
        {
            grid = GetComponent<Grid>();
            terrain = GetComponent<Terrain>();

            filas = (int)(terrain.terrainData.size.x / grid.cellSize.x);
            columnas = (int)(terrain.terrainData.size.z / grid.cellSize.z);

            matriz = new InfluenceCellBehaviour[filas, columnas];
            Vector3 minPos;
            minPos.x = (grid.cellSize.x / 2);
            minPos.z = (grid.cellSize.z / 2);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    GameObject currCasilla = Instantiate(cell, container.transform);
                    currCasilla.transform.localScale = grid.cellSize;
                    Vector3 pos = new Vector3(minPos.x + (i * grid.cellSize.x), -2.0f, minPos.z + (j * grid.cellSize.z));
                    currCasilla.transform.localPosition = pos;
                    matriz[i, j] = currCasilla.GetComponent<InfluenceCellBehaviour>();
                    matriz[i, j].fil = i;
                    matriz[i, j].col = j;
                    if (i == 0 && j == 0) posIni = matriz[i, j].transform.position;

                }
            }
        }

        //  Actualiza una casilla al entrar una unidad en ella
        public void ActualizaPrioridadAlEntrar(InfluenceCellBehaviour casilla, UnitType unit_)
        {
            casilla.UnidadEntraCasilla(unit_, unit_.influencia);

            if (unit_.unit == Unit.Defense) return;

            //Casillas adyacentes
            for (int i = casilla.fil - unit_.rango; i <= casilla.fil + unit_.rango; i++)
            {
                for (int j = casilla.col - unit_.rango; j <= casilla.col + unit_.rango; j++)
                {
                    //comprobamos que no nos salimos de la matriz
                    if (i >= 0 && i < filas && j >= 0 && j < columnas && casilla != matriz[i, j])
                    {
                        //comprobamos que prioridad le corresponde
                        matriz[i, j].UnidadEntraCasilla(unit_, unit_.influencia - 1);
                    }
                }
            }



            if (MaxprioAzul == null || MaxprioAzul.harkonnenPriority < casilla.harkonnenPriority)
            {
                MaxprioAzul = casilla;
            }
            if (MaxprioAmarillo == null || MaxprioAmarillo.fremenPriority < casilla.fremenPriority)
            {
                MaxprioAmarillo = casilla;
            }

        }

        //  Actualiza una casilla al salir
        public void ActualizaPrioridadAlSalir(InfluenceCellBehaviour casilla, UnitType unit_)
        {
            casilla.UnityOnOut(unit_, unit_.influencia);
            if (unit_.unit == Unit.Defense) return;

            int inf = unit_.influencia - 1;
            //recorremos la submatriz correspondiente
            for (int i = casilla.fil - unit_.rango; i <= casilla.fil + unit_.rango; i++)
            {
                for (int j = casilla.fil - unit_.rango; j <= casilla.fil + unit_.rango; j++)
                {
                    //comprobamos que no nos salimos de la matriz
                    if (i >= 0 && i < filas && j >= 0 && j < columnas && casilla != matriz[i, j])
                    {
                        //comprobamos que prioridad le corresponde
                        matriz[i, j].UnityOnOut(unit_, unit_.influencia - 1);
                    }
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (visible)
                {
                    for (int i = 0; i < container.transform.childCount; i++)
                    {
                        container.transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < container.transform.childCount; i++)
                    {
                        container.transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = true;
                    }
                }

                visible = !visible;
            }
        }

        //Devuelve la casilla en función de un transform
        public InfluenceCellBehaviour GetCasillaCercana(Transform pos)
        {
            int indX = Mathf.Abs((int)((pos.position.x - posIni.x) / grid.cellSize.x));
            int indZ = Mathf.Abs((int)((pos.position.z - posIni.z) / grid.cellSize.z));

            return matriz[indX, indZ];
        }

        public InfluenceCellBehaviour getEnemyMaxPrio(Team team)
        {

            if (team == Team.Fremen)
            {
                return MaxprioAzul;
            }
            else
            {
                return MaxprioAmarillo;
            }
        }

        public int getEnemyPrio(Team team)
        {
            if (team == Team.Fremen)
            {
                return MaxprioAzul.harkonnenPriority;
            }
            else
            {
                return MaxprioAmarillo.fremenPriority;
            }
        }

        public void ActualizarMapaInfluencia()
        {

        }
    }
}
