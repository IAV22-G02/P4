/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autores originales: Opsive (Behavior Designer Samples)
   Revisi�n: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts.G02
{
    /*
     * Ejemplo b�sico sobre c�mo crear un controlador basado en IA para el minijuego RTS.
     * �nicamente mandan unas �rdenes cualquiera, para probar cosas aleatorias... 
     * pero no realiza an�lisis t�ctico, ni considera puntos de ruta t�cticos, ni coordina acciones de ning�n tipo .
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

        // No necesita guardar mucha informaci�n porque puede consultar la que desee por sondeo,
        // incluida toda la informaci�n de instalaciones y unidades, tanto propias como ajenas

        // Mi ��dice de controlador y un par de instalaciones para referenciar
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
        bool workersUnderAttack = false;
        bool baseUnderAttack = false;

        int ExtractionUnitAttackedIndex;

        // N�mero de paso de pensamiento 
        private int ThinkStepNumber { get; set; } = 0;

        // �ltima unidad creada
        private UnitPurpose LastUnit { get; set; }

        // Despierta el controlado y configura toda estructura interna que sea necesaria
        private void Awake()
        {
            Name = "Example 2";

            Author = "Jose Daniel Rave Robayo | " +
                     " �ngel L�pez Benitez |" +
                     " Iv�n Prado Echegaray" +
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

            //Empieza con un rush para da�ar la economia enemiga
            if (ps == PlayStyle.Agressive) prioridad = Prioridades.HurtEnemieEconomie;
            else if (ps == PlayStyle.Pasives) prioridad = Prioridades.CreateUnits; //Si es pasivo, se encarga de defender y aumentar su economia

        }

        // El m�todo de pensar que sobreescribe e implementa el controlador, para percibir (hacer mapas de influencia, etc.) y luego actuar.
        protected override void Think()
        {
            ExtractionUnitAttackedIndex = -1;
            getIndexes();

            getUnits();

            myBuildings();

            ScanEnemy();

            //Si conseguimos da�ar suficiente la economia enemiga, atacamos
            if (EnemyExtractores.Count == 0) prioridad = Prioridades.Attack;

            checkWorkersState();

            checkBase();

            if ((MisDestructores.Count == GameManager.Instance.DestructionUnitsMax ||
                MisExploradores.Count == GameManager.Instance.ExplorationUnitsMax) && prioridad != Prioridades.DefenseBase) 
                prioridad = Prioridades.Attack;

            //Si tiene mucho dinero compra
            if (GameManager.Instance.GetMoney(MyIndex) > 30000)
                prioridad = Prioridades.CreateUnits;

            Debug.Log(MyIndex + " PRIORIZAME ESTA CRACK -> " + prioridad);

            switch (prioridad)
            {
                case Prioridades.HurtEnemieEconomie:
                    attackEconomie();

                    break;

                case Prioridades.Attack:

                    sendDestructorsToEnemyBase();

                    break;

                case Prioridades.CreateUnits:

                    compras();

                    break;

                case Prioridades.DefenseBase:

                    defenseTheBase();

                    break;

                case Prioridades.DefenseWorkers:

                    for (int i = 0; i < MisExploradores.Count; i++)
                    {
                        if(MisExploradores[i] != null && ExtractionUnitAttackedIndex > -1) 
                            MisExploradores[i].Move(this, MisExtractores[ExtractionUnitAttackedIndex].getExtractor().transform);
                    }

                    break;

            }
            ThinkStepNumber++;
        }

        private void checkWorkersState()
        {
            workersUnderAttack = false;

            for (int i = 0; i < MisExtractores.Count && !workersUnderAttack; i++)
            {
                for (int e = 0; e < EnemyExploradores.Count && !workersUnderAttack; e++)
                {
                    if (EnemyExploradores[e] != null && MisExtractores[i].getExtractor() != null)
                    {
                        Vector3 distExp = MisExtractores[i].getExtractor().transform.position - EnemyExploradores[e].transform.position;

                        if (distExp.magnitude < MisExtractores[i].getExtractor().Radius)
                        {
                            prioridad = Prioridades.DefenseWorkers;

                            ExtractionUnitAttackedIndex = i;
                            workersUnderAttack = true;
                        }
                    }
                } 
            }

            if (workersUnderAttack && MisExploradores.Count < minDesiredExplorers)
                prioridad = Prioridades.CreateUnits;

            if (ps == PlayStyle.Agressive && !workersUnderAttack)
            {
                if (EnemyDestructores.Count < MisDestructores.Count)
                    prioridad = Prioridades.Attack;
                if (MisExploradores.Count < minDesiredExplorers)
                    prioridad = Prioridades.CreateUnits;
                else if(MisExploradores.Count > GameManager.Instance.ExplorationUnitsMax / 4)
                    prioridad = Prioridades.HurtEnemieEconomie;
            }
            else if(ps == PlayStyle.Pasives && !workersUnderAttack)
            {
                if (MisExploradores.Count < GameManager.Instance.ExplorationUnitsMax / 2)
                    prioridad = Prioridades.CreateUnits;
                else if (MisExploradores.Count >= GameManager.Instance.ExplorationUnitsMax/2 || EnemyExploradores.Count < MisExploradores.Count/2)
                    prioridad = Prioridades.HurtEnemieEconomie;
            }
        }

        private void checkBase()
        {
            baseUnderAttack = false;
            for (int e = 0; e < EnemyExploradores.Count && !baseUnderAttack; e++)
            {
                if (EnemyExploradores[e] != null)
                {
                    Vector3 distExp = MiBase[0].transform.position - EnemyExploradores[e].transform.position;

                    if (distExp.magnitude < MiBase[0].Radius)
                    {
                        prioridad = Prioridades.DefenseBase;

                        baseUnderAttack = true;
                    }
                }

            }

            for (int d = 0; d < EnemyDestructores.Count && !baseUnderAttack; d++)
            {
                if (EnemyDestructores[d] != null)
                {
                    Vector3 distExp = MiBase[0].transform.position - EnemyDestructores[d].transform.position;

                    if (distExp.magnitude < MiBase[0].Radius)
                    {
                        prioridad = Prioridades.DefenseBase;

                        baseUnderAttack = true;
                    }
                }
            }
        }

        private void defenseTheBase()
        {
                int elegido = -1; bool destroyer = false;
            for (int i = 0; i < MisDestructores.Count; i++)
            {
                if (MisDestructores[i] != null)
                {
                    Transform destiny;
                    float dist = MiBase[0].Radius;
                    for (int j = 0; j < EnemyExploradores.Count; j++)
                    {
                        if (EnemyExploradores[j] != null)
                        {
                            if ((MiBase[0].transform.position - EnemyExploradores[j].transform.position).magnitude < dist)
                            {
                                dist = (MiBase[0].transform.position - EnemyExploradores[j].transform.position).magnitude;
                                elegido = j;
                            }
                        }
                    }

                    dist = MiBase[0].Radius;
                    for (int j = 0; j < EnemyDestructores.Count; j++)
                    {
                        if (EnemyDestructores[j] != null)
                        {
                            if ((MiBase[0].transform.position - EnemyDestructores[j].transform.position).magnitude < dist)
                            {
                                dist = (MiBase[0].transform.position - EnemyDestructores[j].transform.position).magnitude;
                                elegido = j;
                                destroyer = true;
                            }
                        }
                    }

                    if (elegido > -1)
                    {
                        if (!destroyer)
                            destiny = EnemyExploradores[elegido].transform;
                        else
                            destiny = EnemyDestructores[elegido].transform;

                        if ((MisDestructores[i].transform.position - destiny.position).magnitude < MisDestructores[i].Radius)
                            MisDestructores[i].Move(this, destiny);
                        else
                            MisDestructores[i].Attack(this, destiny);
                    }
                }
            }
            if (elegido > -1)
                for (int i = 0; i < MisExploradores.Count; i++)
                    if(MisExploradores[i] != null) MisExploradores[i].Move(this, MiBase[0].transform);
            else
                prioridad = Prioridades.CreateUnits;

        }

        private void compras()
        {
            //Priorizamos nuestra propia economia, creamos alguna unidad recolectora adicional para jugar m�s a Macro Game
            int money = GameManager.Instance.GetMoney(MyIndex);
            int unitMax = GameManager.Instance.ExtractionUnitsMax;
            int unitCost = GameManager.Instance.ExtractionUnitCost;

            reponerUnidades(money, ref unitMax, ref unitCost);
        }

        private void reponerUnidades(int money, ref int unitMax, ref int unitCost)
        {
            if (MisExtractores.Count < minDesiredExtractors)
            {
                if (money > unitCost && MisExtractores.Count < unitMax && !ExtractorJustCreated)
                {
                    Extractor actExtractor = new Extractor(GameManager.Instance.CreateUnit(this, MiBase[0],
                        GameManager.UnitType.EXTRACTION).GetComponent<ExtractionUnit>());

                    //Asignarle un campo de melagne, que no se como se hace
                    actExtractor.getExtractor().Move(this, getMelangeToBuy(MiFactoria[0].transform.position).transform.position);

                    ExtractorJustCreated = true;
                }
            }
            else if(MisExploradores.Count < minDesiredExplorers)
            {
                unitMax = GameManager.Instance.ExplorationUnitsMax;
                unitCost = GameManager.Instance.ExplorationUnitCost;

                if (money > unitCost && MisExploradores.Count < unitMax)
                {
                    ExplorationUnit newRecon = (ExplorationUnit)GameManager.Instance.CreateUnit(this, MiBase[0], GameManager.UnitType.EXPLORATION);

                    MisExploradores.Add(newRecon);
                }
            }
            else if(MisDestructores.Count < minDesiredDestructors)
            {
                unitMax = GameManager.Instance.DestructionUnitsMax;
                unitCost = GameManager.Instance.DestructionUnitCost;

                if (money > unitCost && MisDestructores.Count < unitMax)
                {
                    DestructionUnit newDest = (DestructionUnit)GameManager.Instance.CreateUnit(this, MiBase[0], GameManager.UnitType.DESTRUCTION);

                    MisDestructores.Add(newDest);
                }
            }
            else {
                int u = Random.Range(0, 5);
                if (u != 4)
                {
                    unitMax = GameManager.Instance.ExplorationUnitsMax;
                    unitCost = GameManager.Instance.ExplorationUnitCost;

                    if (money > unitCost && MisExploradores.Count < unitMax)
                    {
                        ExplorationUnit newRecon = (ExplorationUnit)GameManager.Instance.CreateUnit(this, MiBase[0], GameManager.UnitType.EXPLORATION);

                        MisExploradores.Add(newRecon);
                    }
                }
                else
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
        }

        private void sendDestructorsToEnemyBase()
        {
            if (MisDestructores.Count <= 0)
                prioridad = Prioridades.CreateUnits;

            for (int i = 0; i < MisDestructores.Count; i++)
            {
                bool Menaced = MisDestructores[i].IsMenaced();
                Transform destiny;

                if (Menaced) destiny = MiBase[0].transform;
                else destiny = EnemyBase[0].transform;

                Vector3 dist = EnemyBase[0].transform.position - MisDestructores[i].transform.position;

                if (dist.magnitude < MisDestructores[i].Radius) MisDestructores[i].Attack(this, EnemyBase[0].transform.position);
                else MisDestructores[i].Move(this, destiny);
            }
        }

        private void attackEconomie()
        {
            //Nuestros exploradore intentan herir la economia enemiga
            for (int i = 0; i < MisExploradores.Count; i++)
            {
                if (MisExploradores[i] != null)
                {
                    bool Menaced = MisExploradores[i].IsMenaced();
                    Transform destiny;

                    if (Menaced) destiny = MiBase[0].transform;
                    else
                    {
                        int j = 0;
                        while (j < EnemyExtractores.Count && EnemyExtractores[j] == null) j++;
                        if (j < EnemyExtractores.Count)
                            destiny = EnemyExtractores[j].transform;
                        else
                            destiny = EnemyBase[0].transform;
                    }

                    MisExploradores[i].Move(this, destiny);
                }
            }

            if (MisExploradores.Count < minDesiredExplorers)
            {
                for (int i = 0; i < MisExploradores.Count; i++)
                    MisExploradores[i].Move(this, MiBase[0].transform);
                prioridad = Prioridades.CreateUnits;
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
            //Creamos el n�mero minimo de unidades extractoras
            while (MisExtractores.Count < minDesiredExtractors)
            {

                GameManager gm = GameManager.Instance;

                ExtractionUnit nExtraction = gm.GetExtractionUnits(MyIndex)[MisExtractores.Count];

                MisExtractores.Add(new Extractor(nExtraction));
            }

            MisExploradores = GameManager.Instance.GetExplorationUnits(MyIndex);
            MisDestructores = GameManager.Instance.GetDestructionUnits(MyIndex);
        }

        private LimitedAccess getMelangeToBuy(Vector3 initPos)
        {
            LimitedAccess actMelange = null;

            float distance = 100000;
            foreach (LimitedAccess melange in RTSScenarioManager.Instance.LimitedAccesses)
            {
                float melangeDistance = (initPos - melange.transform.position).magnitude;
                if (melange.OccupiedBy == null && melangeDistance < distance)
                {
                    actMelange = melange;
                    distance = melangeDistance;
                }
            }
            return actMelange;

        }
    }
}