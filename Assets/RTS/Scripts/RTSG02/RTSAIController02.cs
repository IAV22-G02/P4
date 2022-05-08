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
     * �nicamente mandan unas �rdenes cualquiera, para probar cosas aleatorias... pero no realiza an�lisis t�ctico, ni considera puntos de ruta t�cticos, ni coordina acciones de ning�n tipo .
     */ 
    public class RTSAIController02: RTSAIController
    {
        private GUIStyle _labelStyle;
        private GUIStyle _labelSmallStyle;

        public int minDesiredExtractors = 2;
        public int minDesiredDestructors = 2;
        public int minDesiredExplorers = 2;

        // No necesita guardar mucha informaci�n porque puede consultar la que desee por sondeo, incluida toda la informaci�n de instalaciones y unidades, tanto propias como ajenas

        // Mi �ndice de controlador y un par de instalaciones para referenciar
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

        // N�mero de paso de pensamiento 
        private int ThinkStepNumber { get; set; } = 0;

        // �ltima unidad creada
        private Unit LastUnit { get; set; }

        // Despierta el controlado y configura toda estructura interna que sea necesaria
        private void Awake()
        {
            Name = "Example 2";
            Author = "Jose Daniel Rave Robayo | " +
                     " �ngel L�pez Benitez |" + 
                     " Iv�n Prado" + 
                     "Juan Diego Mendoza Reyes";

            _labelStyle = new GUIStyle();
            _labelStyle.fontSize = 16;
            _labelStyle.normal.textColor = Color.white;

            _labelSmallStyle = new GUIStyle();
            _labelSmallStyle.fontSize = 11;
            _labelSmallStyle.normal.textColor = Color.white;
        }

        // El m�todo de pensar que sobreescribe e implementa el controlador, para percibir (hacer mapas de influencia, etc.) y luego actuar.
        protected override void Think() {

            ThinkStepNumber++;
        }
    }
}