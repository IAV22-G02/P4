/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform·tica de la Universidad Complutense de Madrid (EspaÒa).

   Autores originales: Opsive (Behavior Designer Samples)
   RevisiÛn: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using UnityEngine;

namespace es.ucm.fdi.iav.rts.example2
{
    /*
     * Ejemplo b·sico sobre cÛmo crear un controlador basado en IA para el minijuego RTS.
     * ⁄nicamente mandan unas Ûrdenes cualquiera, para probar cosas aleatorias... pero no realiza an·lisis t·ctico, ni considera puntos de ruta t·cticos, ni coordina acciones de ning˙n tipo .
     */ 
    public class RTSAIControllerExample2: RTSAIController
    {
        // No necesita guardar mucha informaciÛn porque puede consultar la que desee por sondeo, incluida toda la informaciÛn de instalaciones y unidades, tanto propias como ajenas

        // Mi ˙ãdice de controlador y un par de instalaciones para referenciar
        private int MyIndex { get; set; }
        private int FirstEnemyIndex { get; set; }
        private BaseFacility MyFirstBaseFacility { get; set; }
        private ProcessingFacility MyFirstProcessingFacility { get; set; }
        private BaseFacility FirstEnemyFirstBaseFacility { get; set; }
        private ProcessingFacility FirstEnemyFirstProcessingFacility { get; set; }

        // N˙mero de paso de pensamiento 
        private int ThinkStepNumber { get; set; } = 0;

        // ⁄ltima unidad creada
        private Unit LastUnit { get; set; }

        // Despierta el controlado y configura toda estructura interna que sea necesaria
        private void Awake()
        {
            Name = "Example 2";
            Author = "Federico Peinado";
        }

        // El mÈtodo de pensar que sobreescribe e implementa el controlador, para percibir (hacer mapas de influencia, etc.) y luego actuar.
        protected override void Think()
        {
            // Actualizo el mapa de influencia 
            // ...

            // Para las Ûrdenes aquÅEestoy asumiendo que tengo dinero de sobra y que se dan las condiciones de todas las cosas...
            // (Ojo: esto no deber˙} hacerse porque si me equivoco, causarÅEfallos en el juego... hay que comprobar que cada llamada tiene sentido y es posible hacerla)

            // AquÅElo suyo ser˙} elegir bien la acciÛn a realizar. 
            // En este caso como es para probar, voy dando a cada vez una orden de cada tipo, todo de seguido y muy aleatorio... 
            switch (ThinkStepNumber)
            {
                case 0:
                    // Lo primer es conocer el ˙ãdice que me ha asignado el gestor del juego
                    MyIndex = GameManager.Instance.GetIndex(this);

                    // Obtengo referencias a mis cosas
                    MyFirstBaseFacility = GameManager.Instance.GetBaseFacilities(MyIndex)[0];
                    MyFirstProcessingFacility = GameManager.Instance.GetProcessingFacilities(MyIndex)[0];

                    // Obtengo referencias a las cosas de mi enemigo
                    // ...
                    var indexList = GameManager.Instance.GetIndexes();
                    indexList.Remove(MyIndex);
                    FirstEnemyIndex = indexList[0];
                    FirstEnemyFirstBaseFacility = GameManager.Instance.GetBaseFacilities(FirstEnemyIndex)[0];
                    FirstEnemyFirstProcessingFacility = GameManager.Instance.GetProcessingFacilities(FirstEnemyIndex)[0];

                    // Construyo por primera vez el mapa de influencia (con las 'capas' que necesite)
                    // ...
                    break;

                case 1:

                    LastUnit = GameManager.Instance.CreateUnit(this, MyFirstBaseFacility, GameManager.UnitType.EXPLORATION);
                    break;

                case 2:
                    LastUnit = GameManager.Instance.CreateUnit(this, MyFirstBaseFacility, GameManager.UnitType.EXPLORATION);
                    break;

                case 3:
                    LastUnit = GameManager.Instance.CreateUnit(this, MyFirstBaseFacility, GameManager.UnitType.DESTRUCTION);
                    break;

                case 4:
                    LastUnit = GameManager.Instance.CreateUnit(this, MyFirstBaseFacility, GameManager.UnitType.DESTRUCTION);
                    break;

                case 5:
                    LastUnit = GameManager.Instance.CreateUnit(this, MyFirstBaseFacility, GameManager.UnitType.EXTRACTION);
                    break;

                case 6:
                    LastUnit = GameManager.Instance.CreateUnit(this, MyFirstBaseFacility, GameManager.UnitType.EXPLORATION);
                    break;

                case 7:
                    GameManager.Instance.MoveUnit(this, LastUnit, MyFirstProcessingFacility.transform);
                    break;

                case 8:
                    LastUnit = GameManager.Instance.CreateUnit(this, MyFirstBaseFacility, GameManager.UnitType.EXPLORATION);
                    break;

                case 9:
                    GameManager.Instance.MoveUnit(this, LastUnit, MyFirstProcessingFacility.transform);
                    break;

                case 10:
                    LastUnit = GameManager.Instance.CreateUnit(this, MyFirstBaseFacility, GameManager.UnitType.EXPLORATION);
                    break;

                case 11:
                    GameManager.Instance.MoveUnit(this, LastUnit, MyFirstProcessingFacility.transform);
                    // No lo hago... pero tambiÈn se podr˙}n crear y mover varias unidades en el mismo momento, claro...
                    break;

                case 12:
                    Stop = true;
                    break;
            }
            //Debug.Log("Controlador autom·tico " + MyIndex + " ha finalizado el paso de pensamiento " + ThinkStepNumber);
            ThinkStepNumber++;
        }
    }
}