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
using System.Collections.Generic;

namespace es.ucm.fdi.iav.rts
{
    /* 
    * El controlador del jugador permite ofrecer una interfaz al jugador humano para que mande Ûrdenes a uno de los ejÈrcitos del juego, aunque de manera aleatoria (sin poder elegir origen ni destino).
    * Se trata de una clase abstracta para todos los controladores de bots espec˙Éicos que programemos.
    * 
    * Posibles mejoras:
    * - AÒadir un botÛn para realizar un movimiento masivo (ej. ataque con todas las unidades)
    * - En lugar de usar OnGUI, crear una interfaz de usuario m·s moderna.
    * - Mantener y mostrar en formato de porcentaje o barra la cantidad de salud que le queda al jugador, n˙mero de unidades, etc. Antes se mostraba asÅE GUILayout.Label(string.Format("Enemy Health: {0}%", Mathf.RoundToInt((currentHealth / startEnemyHealth) * 100)), labelStyle);
    * - Permitir que se pueda cambiar desde el inspector el tamaÒo de fuente y los colores, poniÈndolos diferentes para cada tipo de jugador.
    * - Adaptar la interfaz para N jugadores (mantener botones y sÛlo ir cambiando el ˙ãdice de a quien le toca).
    */
    public class RTSPlayerRandomController : RTSController
    { 
        // El estilo para las etiquetas de la interfaz
        private GUIStyle _labelStyle;

        // Mi propio ˙ãdice (parecido al n˙mero de jugador que soy)
        private int _index;

        // Despertar el controlador del jugador, las estructuras internas que necesita
        private void Awake()
        {
            // Aumenta el tamaÒo y cambia el color de fuente de letra para OnGUI
            _labelStyle = new GUIStyle();
            _labelStyle.fontSize = 16;
            _labelStyle.normal.textColor = Color.white;
        }

        // Iniciar el controlador del jugador, llamando a los otros objetos que sean necesarios
        private void Start()
        {
            _index = GameManager.Instance.GetIndex(this);
        }

        // Dibuja la interfaz gr·fica de usuario para que la utilice el jugador humano
        private void OnGUI()
        {
            // Abrimos un ·rea de distribuciÛn arriba y a la izquierda (si el ˙ãdice del controlador es par) o a la derecha (si el ˙ãdice es impar), con contenido en vertical
            float areaWidth = 150; 
            float areaHeight = 250;
            if (_index % 2 == 0)
                GUILayout.BeginArea(new Rect(0, 0, areaWidth, areaHeight));
            else
                GUILayout.BeginArea(new Rect(Screen.width - areaWidth, 0, Screen.width, areaHeight));
            GUILayout.BeginVertical(); 

            // Lista las variables importantes como el ˙ãdice del jugador y su cantidad de dinero
            GUILayout.Label("[Random C" + _index + " ] " + GameManager.Instance.GetMoney(_index) + " solaris", _labelStyle); 

            // Botones que permite al jugador humano hacer ciertas acciones con su ejÈrcito

            if (GUILayout.Button("Create Extractor", GUILayout.ExpandWidth(false))) {
                // SÛlo si tengo al menos una instalaciÛn base, esto va a funcionar, si no no harÅEnada
                List<BaseFacility> facilities = GameManager.Instance.GetBaseFacilities(_index);
                if (facilities.Count > 0)
                    // Se pasa una instalaciÛn base cualquiera como par·metro (aleatoria, obviamente nuestra) 
                    GameManager.Instance.CreateUnit(this, facilities[Random.Range(0, facilities.Count - 1)], GameManager.UnitType.EXTRACTION); 
            }
            if (GUILayout.Button("Create Explorator", GUILayout.ExpandWidth(false)))
            {
                // SÛlo si tengo al menos una instalaciÛn base, esto va a funcionar, si no no harÅEnada
                List<BaseFacility> facilities = GameManager.Instance.GetBaseFacilities(_index);
                if (facilities.Count > 0)
                    // Se pasa una instalaciÛn base cualquiera como par·metro (aleatoria, obviamente nuestra) 
                    GameManager.Instance.CreateUnit(this, facilities[Random.Range(0, facilities.Count - 1)], GameManager.UnitType.EXPLORATION);
            }
            if (GUILayout.Button("Create Destructor", GUILayout.ExpandWidth(false)))
            {
                // SÛlo si tengo al menos una instalaciÛn base, esto va a funcionar, si no no harÅEnada
                List<BaseFacility> facilities = GameManager.Instance.GetBaseFacilities(_index);
                if (facilities.Count > 0)
                    // Se pasa una instalaciÛn base cualquiera como par·metro (aleatoria, obviamente nuestra) 
                    GameManager.Instance.CreateUnit(this, facilities[Random.Range(0, facilities.Count - 1)], GameManager.UnitType.DESTRUCTION);
            }
            if (GUILayout.Button("Move Extractor", GUILayout.ExpandWidth(false))) 
            {
                // Mueve una unidad extractora cualquiera (aleatoria)
                List<ExtractionUnit> unitsList = GameManager.Instance.GetExtractionUnits(GameManager.Instance.GetIndex(this));
                ExtractionUnit unit = unitsList[Random.Range(0, unitsList.Count - 1)];

                // A un punto del mapa cualquiera (tomo la posiciÛn de un recurso aleatorio) 
                List<LimitedAccess> resourcesList = RTSScenarioManager.Instance.LimitedAccesses;
                LimitedAccess resource = resourcesList[Random.Range(0, resourcesList.Count - 1)];

                GameManager.Instance.MoveUnit(this, unit, resource.transform);
            }
            if (GUILayout.Button("Move Explorator", GUILayout.ExpandWidth(false)))
            {
                // Mueve una unidad exploradora cualquiera (aleatoria)
                List<ExplorationUnit> unitsList = GameManager.Instance.GetExplorationUnits(GameManager.Instance.GetIndex(this));
                if (unitsList.Count > 0)
                {
                    ExplorationUnit unit = unitsList[Random.Range(0, unitsList.Count - 1)];

                    // A un punto del mapa cualquiera (tomo la posiciÛn de una torreta aleatoria) 
                    List<Tower> towersList = RTSScenarioManager.Instance.Towers;
                    if (towersList.Count > 0)
                    {
                        Tower tower = towersList[Random.Range(0, towersList.Count - 1)];

                        GameManager.Instance.MoveUnit(this, unit, tower.transform);
                    }
                }
            }
            if (GUILayout.Button("Move Destructor", GUILayout.ExpandWidth(false)))
            {
                // Mueve una unidad destructora cualquiera (aleatoria)
                List<DestructionUnit> unitsList = GameManager.Instance.GetDestructionUnits(GameManager.Instance.GetIndex(this));
                if (unitsList.Count > 0)
                {
                    DestructionUnit unit = unitsList[Random.Range(0, unitsList.Count - 1)];

                    // Elijo un enemigo (un ˙ãdice cualquiera que no sea el m˙å... comprobar tambiÈn que no estoy yo solo)
                    List<int> list = GameManager.Instance.GetIndexes();
                    list.Remove(GameManager.Instance.GetIndex(this));
                    int enemyIndex = list[Random.Range(0, list.Count - 1)];

                    // A un punto del mapa cualquiera (tomo la posiciÛn de una instalaciÛn enemiga -base o de procesamiento- aleatoria) 
                    List<Facility> enemyFacilitiesList = new List<Facility>();
                    foreach (var facility in GameManager.Instance.GetBaseFacilities(enemyIndex)) // No se pueden asignar directamente las listas
                        enemyFacilitiesList.Add(facility);
                    foreach (var facility in GameManager.Instance.GetProcessingFacilities(enemyIndex)) // No se pueden asignar directamente las listas
                        enemyFacilitiesList.Add(facility);
                    if (enemyFacilitiesList.Count > 0)
                    {
                        Facility enemyFacility = enemyFacilitiesList[Random.Range(0, enemyFacilitiesList.Count - 1)];

                        GameManager.Instance.MoveUnit(this, unit, enemyFacility.transform);
                    }
                }
            }

            // Cerramos el ·rea de distribuciÛn con contenido en vertical
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}