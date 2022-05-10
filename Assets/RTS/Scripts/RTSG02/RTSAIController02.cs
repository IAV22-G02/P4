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
    public class RTSAIController02: RTSAIController
    {
        //Enum para determinar que queremos hacer
        enum Prioridades {DestroyNeutralCamp, Defense, HurtEnemieEconomie, Attack }

        private GUIStyle _labelStyle;
        private GUIStyle _labelSmallStyle;

        public int minDesiredExtractors = 2;
        public int minDesiredDestructors = 2;
        public int minDesiredExplorers = 2;

        // No necesita guardar mucha información porque puede consultar la que desee por sondeo,
        // incluida toda la información de instalaciones y unidades, tanto propias como ajenas

        // Mi índice de controlador y un par de instalaciones para referenciar
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

        //Si nada más le preocupa, se encarga de dañar la economia del otro jugador
        private Prioridades prioridad = Prioridades.HurtEnemieEconomie;

        // Número de paso de pensamiento 
        private int ThinkStepNumber { get; set; } = 0;

        // Última unidad creada
        private Unit LastUnit { get; set; }

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
        }

        // El método de pensar que sobreescribe e implementa el controlador, para percibir (hacer mapas de influencia, etc.) y luego actuar.
        protected override void Think()
        {
            getIndexes();

            getUnits();

            myBuildings();

            ScanEnemy();

            if (EnemyExtractores.Count == 0) prioridad = Prioridades.Attack;

            switch (prioridad)
            {
                case Prioridades.HurtEnemieEconomie:
                    attackEconomie();

                    break;

                case Prioridades.Attack:

                    for(int i = 0; i < MisDestructores.Count; i++)
                    {
                        bool Menaced = MisDestructores[i].IsMenaced();
                        Transform destiny;

                        if (Menaced) destiny = MiBase[0].transform;
                        else destiny = EnemyBase[0].transform;

                        Vector3 dist = EnemyBase[0].transform.position - MisDestructores[i].transform.position;

                        Debug.Log("dit:" + dist.magnitude);
                        Debug.Log("radd:" + MisDestructores[i].Radius);

                        if (dist.magnitude < MisDestructores[i].Radius)
                        {
                            MisDestructores[0].Attack(this, EnemyBase[0].transform.position);

                            Debug.Log("Ataco");
                        }
                        else MisDestructores[0].Move(this, destiny);
                    }

                    break;
            }

            //if(MisDestructores.Count > 0)
            //{

            //    Vector3 dist = EnemyDestructores[0].transform.position - MisDestructores[0].transform.position;

            //    if (dist.magnitude < MisDestructores[0].Radius)
            //    {
            //        MisDestructores[0].Attack(this, EnemyDestructores[0].transform.position);

            //        Debug.Log("Ataco");
            //    }
            //    else MisDestructores[0].Move(this, EnemyDestructores[0].transform.position);
            //}


            ThinkStepNumber++;
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