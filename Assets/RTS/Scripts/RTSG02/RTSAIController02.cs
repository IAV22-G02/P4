/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autores originales: Opsive (Behavior Designer Samples)
   Revisión: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts.G02
{
    /*
     * Ejemplo básico sobre cómo crear un controlador basado en IA para el minijuego RTS.
     * Únicamente mandan unas órdenes cualquiera, para probar cosas aleatorias... 
     * pero no realiza análisis táctico, ni considera puntos de ruta tácticos, ni coordina acciones de ningún tipo .
     */
    public class RTSAIController02 : RTSAIController
    {
        enum PlayStyle { Agressive, Pasives }


        //Enum para determinar que queremos hacer
        enum Prioridades { DestroyNeutralCamp, DefenseBase, DefenseWorkers, CreateUnits, HurtEnemieEconomie, Attack }

        private GUIStyle _labelStyle;
        private GUIStyle _labelSmallStyle;

        public int minDesiredExtractors = 2;
        public int minDesiredDestructors = 2;
        public int minDesiredExplorers = 2;

        // No necesita guardar mucha información porque puede consultar la que desee por sondeo,
        // incluida toda la información de instalaciones y unidades, tanto propias como ajenas

        // Mi ú‹dice de controlador y un par de instalaciones para referenciar
        private int MyIndex { get; set; }
        private int FirstEnemyIndex { get; set; }
        private BaseFacility MyFirstBaseFacility { get; set; }
        private ProcessingFacility MyFirstProcessingFacility { get; set; }
        private BaseFacility FirstEnemyFirstBaseFacility { get; set; }
        private ProcessingFacility FirstEnemyFirstProcessingFacility { get; set; }


        //UNITIES
        private List<BaseFacility> MiBase;
        private List<ProcessingFacility> MiFactoria;
        private List<Extractor> MisExtractores;
        private List<ExplorationUnit> MisExploradores;
        private List<DestructionUnit> MisDestructores;

        //ENEMY UNITS
        private List<DestructionUnit> EnemyDestructores;
        private List<BaseFacility> EnemyBase;
        private List<ProcessingFacility> EnemyFactoria;
        private List<ExplorationUnit> EnemyExploradores;
        private List<ExtractionUnit> EnemyExtractores;

        private Prioridades prioridad;
        private PlayStyle ps;

        bool ExtractorJustCreated = false;

        // Número de paso de pensamiento 
        private int ThinkStepNumber { get; set; } = 0;

        // Última unidad creada
        private UnitPurpose LastUnit { get; set; }

        // Despierta el controlado y configura toda estructura interna que sea necesaria
        private void Awake()
        {
            Name = "Example 2";

            Author = "Jose Daniel Rave Robayo | " +
                     " Ángel López Benitez |" +
                     " Iván Prado Echegaray" +
                     "Juan Diego Mendoza Reyes";

            _labelStyle = new GUIStyle();
            _labelStyle.fontSize = 16;
            _labelStyle.normal.textColor = Color.white;

            _labelSmallStyle = new GUIStyle();
            _labelSmallStyle.fontSize = 11;
            _labelSmallStyle.normal.textColor = Color.white;

            MisExtractores = new List<Extractor>();
            MisExploradores = new List<ExplorationUnit>();
            MisDestructores = new List<DestructionUnit>();

            //Chosing playStyle
            ps = (PlayStyle)Random.Range(0, 2);

            //Empieza con un rush para dañar la economia enemiga
            if (ps == PlayStyle.Agressive) prioridad = Prioridades.HurtEnemieEconomie;
            else if (ps == PlayStyle.Pasives) prioridad = Prioridades.CreateUnits; //Si es pasivo, se encarga de defender y aumentar su economia

            Debug.Log(ps);
        }

        // El método de pensar que sobreescribe e implementa el controlador, para percibir (hacer mapas de influencia, etc.) y luego actuar.
        protected override void Think()
        {
            getIndexes();

            getUnits();

            myBuildings();

            ScanEnemy();

            //Si conseguimos dañar suficiente la economia enemiga, atacamos
            if (EnemyExtractores.Count == 0) prioridad = Prioridades.Attack;

            checkWorkersState();

            switch (prioridad)
            {
                case Prioridades.HurtEnemieEconomie:
                    attackEconomie();

                    //Despues de atacar priorizamos crear unidades
                    prioridad = Prioridades.CreateUnits;

                    break;

                case Prioridades.Attack:

                    sendDestructorsToEnemyBase();

                    //Despues de intentar atacar el nexo enemigo, defendemos
                    prioridad = Prioridades.DefenseBase;

                    break;

                case Prioridades.CreateUnits:

                    compras();

                    //Si tengo más destructores que el enemigo, intento ganar
                    if (EnemyDestructores.Count < MisDestructores.Count) prioridad = Prioridades.Attack;
                    else if (EnemyExploradores.Count <= MisExploradores.Count) prioridad = Prioridades.HurtEnemieEconomie;
                    else prioridad = Prioridades.DefenseBase;

                    break;

                case Prioridades.DefenseBase:

                    defenseTheBase();

                    break;

                case Prioridades.DefenseWorkers:

                    for(int i = 0; i < MisExploradores.Count; i++)
                    {
                        MisExploradores[i].Move(this, MisExtractores[i].getExtractor().transform);
                    }

                    break;

            }


            ThinkStepNumber++;
        }

        private void checkWorkersState()
        {
            bool workersUnderAttack = false;
            for (int i = 0; i < MisExtractores.Count && !workersUnderAttack; i++)
            {
                for (int e = 0; e < EnemyExploradores.Count && !workersUnderAttack; e++)
                {
                    Vector3 distExp = MisExtractores[i].getExtractor().transform.position - EnemyExploradores[e].transform.position;

                    if (distExp.magnitude < MisExtractores[i].getExtractor().Radius)
                    {
                        prioridad = Prioridades.DefenseWorkers;

                        workersUnderAttack = true;
                    }

                }

                for (int d = 0; d < EnemyDestructores.Count && !workersUnderAttack; d++)
                {
                    Vector3 distExp = MisExtractores[i].getExtractor().transform.position - EnemyDestructores[d].transform.position;

                    if (distExp.magnitude < MisExtractores[i].getExtractor().Radius)
                    {
                        prioridad = Prioridades.DefenseWorkers;

                        workersUnderAttack = true;
                    }

                }
            }

            if (!workersUnderAttack && ps == PlayStyle.Agressive) prioridad = Prioridades.HurtEnemieEconomie;
            else if (!workersUnderAttack && ps == PlayStyle.Pasives) prioridad = Prioridades.CreateUnits;
        }

        private void defenseTheBase()
        {
            for (int i = 0; i < MisDestructores.Count; i++)
            {
                Transform destiny;

                destiny = MiBase[0].transform;

                MisDestructores[i].Move(this, destiny);

            }

            for (int i = 0; i < MisExploradores.Count; i++)
            {
                MisExploradores[i].Move(this, MiBase[0].transform);
            }

            if (EnemyDestructores.Count < MisDestructores.Count) prioridad = Prioridades.Attack;
            else if (EnemyExploradores.Count < MisExploradores.Count) prioridad = Prioridades.HurtEnemieEconomie;
            else prioridad = Prioridades.CreateUnits;
        }

        private void compras()
        {
            //Priorizamos nuestra propia economia, creamos alguna unidad recolectora adicional para jugar más a Macro Game
            int money = GameManager.Instance.GetMoney(MyIndex);
            int unitMax = GameManager.Instance.ExtractionUnitsMax;
            int unitCost = GameManager.Instance.ExtractionUnitCost;

            reponerUnidades(money, ref unitMax, ref unitCost);

            if (money > unitCost && MisExtractores.Count < unitMax && !ExtractorJustCreated)
            {
                Extractor actExtractor = new Extractor(GameManager.Instance.CreateUnit(this, MiBase[0],
                    GameManager.UnitType.EXTRACTION).GetComponent<ExtractionUnit>());

                //Asignarle un campo de melagne, que no se como se hace

                ExtractorJustCreated = true;
            }

            unitMax = GameManager.Instance.ExplorationUnitsMax;
            unitCost = GameManager.Instance.ExplorationUnitCost;

            if (money > unitCost && MisExploradores.Count < unitMax)
            {
                ExplorationUnit newRecon = (ExplorationUnit)GameManager.Instance.CreateUnit(this, MiBase[0], GameManager.UnitType.EXPLORATION);

                MisExploradores.Add(newRecon);
            }

            unitMax = GameManager.Instance.DestructionUnitsMax;
            unitCost = GameManager.Instance.DestructionUnitCost;

            if (money > unitCost && MisDestructores.Count < unitMax)
            {
                DestructionUnit newDest = (DestructionUnit)GameManager.Instance.CreateUnit(this, MiBase[0], GameManager.UnitType.DESTRUCTION);

                MisDestructores.Add(newDest);
            }
        }

        private void reponerUnidades(int money, ref int unitMax, ref int unitCost)
        {
            if (MisExtractores.Count < minDesiredExtractors)
            {
                if (money > unitCost && MisExploradores.Count < unitMax)
                {
                    ExplorationUnit newRecon = (ExplorationUnit)GameManager.Instance.CreateUnit(this, MiBase[0], GameManager.UnitType.EXPLORATION);


                    MisExploradores.Add(newRecon);
                }
            }
            else if (MisExploradores.Count < minDesiredExplorers)
            {
                unitMax = GameManager.Instance.ExplorationUnitsMax;
                unitCost = GameManager.Instance.ExplorationUnitCost;

                if (money > unitCost && MisExploradores.Count < unitMax)
                {
                    ExplorationUnit newRecon = (ExplorationUnit)GameManager.Instance.CreateUnit(this, MiBase[0], GameManager.UnitType.EXPLORATION);

                    MisExploradores.Add(newRecon);
                }
            }
            else if (MisDestructores.Count < minDesiredDestructors)
            {
                unitMax = GameManager.Instance.DestructionUnitsMax;
                unitCost = GameManager.Instance.DestructionUnitCost;

                if (money > unitCost && MisDestructores.Count < unitMax)
                {
                    DestructionUnit newDest = (DestructionUnit)GameManager.Instance.CreateUnit(this, MiBase[0], GameManager.UnitType.DESTRUCTION);

                    MisDestructores.Add(newDest);
                }
            }
        }

        private void sendDestructorsToEnemyBase()
        {
            for (int i = 0; i < MisDestructores.Count; i++)
            {
                bool Menaced = MisDestructores[i].IsMenaced();
                Transform destiny;

                if (Menaced) destiny = MiBase[0].transform;
                else destiny = EnemyBase[0].transform;

                Vector3 dist = EnemyBase[0].transform.position - MisDestructores[i].transform.position;

                if (dist.magnitude < MisDestructores[i].Radius) MisDestructores[0].Attack(this, EnemyBase[0].transform.position);
                else MisDestructores[0].Move(this, destiny);
            }
        }

        private void attackEconomie()
        {

            //Nuestros exploradore intentan herir la economia enemiga
            for (int i = 0; i < MisExploradores.Count; i++)
            {
                bool Menaced = MisExploradores[i].IsMenaced();
                Transform destiny;

                if (Menaced) destiny = MiBase[0].transform;
                else destiny = EnemyExtractores[0].transform;

                MisExploradores[i].Move(this, destiny);
            }
        }

        private void myBuildings()
        {
            MiBase = GameManager.Instance.GetBaseFacilities(MyIndex);
            MiFactoria = GameManager.Instance.GetProcessingFacilities(MyIndex);
        }

        private void ScanEnemy()
        {
            EnemyDestructores = GameManager.Instance.GetDestructionUnits(FirstEnemyIndex);
            EnemyExploradores = GameManager.Instance.GetExplorationUnits(FirstEnemyIndex);
            EnemyExtractores = GameManager.Instance.GetExtractionUnits(FirstEnemyIndex);
            EnemyBase = GameManager.Instance.GetBaseFacilities(FirstEnemyIndex);
            EnemyFactoria = GameManager.Instance.GetProcessingFacilities(FirstEnemyIndex);
        }

        private void getIndexes()
        {
            MyIndex = GameManager.Instance.GetIndex(this);

            var indexList = GameManager.Instance.GetIndexes();
            //Quito mi indice de esa lista
            indexList.Remove(MyIndex);
            //Asumo que el primer indice es el de mi enemigo
            FirstEnemyIndex = indexList[0];
        }

        private void getUnits()
        {
            //Creamos el número minimo de unidades extractoras
            while (MisExtractores.Count < minDesiredExtractors)
            {

                GameManager gm = GameManager.Instance;

                ExtractionUnit nExtraction = gm.GetExtractionUnits(MyIndex)[MisExtractores.Count];

                MisExtractores.Add(new Extractor(nExtraction));
            }

            MisExploradores = GameManager.Instance.GetExplorationUnits(MyIndex);
            MisDestructores = GameManager.Instance.GetDestructionUnits(MyIndex);
        }
    }
}