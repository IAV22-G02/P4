/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform·tica de la Universidad Complutense de Madrid (EspaÒa).

   Autores originales: Opsive (Behavior Designer Samples)
   RevisiÛn: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace es.ucm.fdi.iav.rts
{
    /* 
     * El gestor del escenario es responsable de identificar y mantener la lista de todos los recursos y otros elementos 'neutrales' del escenario que estÈn activos, como poblados y torretas.
     * Sirve para que los bots t·cticos puedan percibir y actuar en respuesta a situaciones t·cticas como estar cerca de peligrosas torretas o de interesantes recursos.
     * 
     * Posibles mejoras:
     * - Administrar desde aquÅEalgunas cuestiones como localizar el recurso m·s cercano que tiene una unidad extractora o cosas asÅE ofrecer el servicio de poder iterar por ellos como con las c·maras, por ejemplo.
     * - Crear un Script Scenario y darle algunas propiedades como el nombre, autor, nivel de dificultad, nivel de justicia o simetr˙}, n˙mero de jugadores, o algo asÅE
     * - Podr˙} introducir m·s tiempos que controlar, para congelar la acciÛn en algunos momentos como al reiniciar
     */
    public class RTSScenarioManager : MonoBehaviour
    {
        // El escenario es un objeto de tipo terreno (puede consultarse su tamaÒo con terrainData.size)
        [SerializeField] private Terrain _scenario = null;
        public Terrain Scenario { get { return _scenario; } }

        // Velocidad del juego. Por defecto estÅEpuesta al doble de lo normal.
        [SerializeField] private float _timeScale = 2f;
        public float TimeScale { get { return _timeScale; } private set { _timeScale = value; } }

        // Lista de toda las c·maras con las que poder visualizar el escenario
        [SerializeField] private List<Camera> _cameras = new List<Camera>();
        public List<Camera> Cameras { get { return _cameras; } }

        // Õndice de la c·mara activa 
        [SerializeField] private int _cameraIndex = 0;
        public int CameraIndex { get { return _cameraIndex; } private set { _cameraIndex = value; } }

        // Los recursos del escenario (todos ser·n de acceso limitado... como lo son las instalaciones de procesamiento, por ejemplo)
        public List<LimitedAccess> LimitedAccesses { get; private set; } = new List<LimitedAccess>();

        // Las torretas (neutrales) del escenario
        public List<Tower> Towers { get; private set; } = new List<Tower>();

        // Los poblados (neutrales) del escenario
        public List<Village> Villages { get; private set; } = new List<Village>();
 
        // Utiliza un sencillo patrÛn Singleton para dar acceso global y eliminar duplicados, aunque no crea un objeto si no estamos en una escena ni se mantiene si cambiamos de escena
        private static RTSScenarioManager _instance;
        public static RTSScenarioManager Instance { get { return _instance; } }

        /********************************************************/

        // El zoom de la c·mara
        // Posibles mejoras: Externalizarlo como valores que se puedan cambiar desde el inspector
        private float minFov = 20f;
        private float maxFov = 80f;
        private float sensitivity = 20f;

        // El estilo para las etiquetas de la interfaz
        private GUIStyle _labelStyle { get; set; }

        // Despierta el Singleton (lo crea) y elimina duplicados de la misma clase que pueda haber en la escena.
        // Inicializa las estructuras internas del escenario, como la escala temporal, las c·maras...
        // Posibles mejoras: 
        // - Por seguridad podr˙}n tambiÈn destruirse torretas, poblados, obst·culos... o el escenario al completo...  y recrearlo todo de alguna manera, por ejemplo desde fichero en el Start. 
        // - Se podr˙}n buscar las c·maras autom·ticamente, debajo del objecto Scenario
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            // Aumenta el tamaÒo y cambia el color de fuente de letra para OnGUI
            _labelStyle = new GUIStyle();
            _labelStyle.fontSize = 20;
            _labelStyle.normal.textColor = Color.white;

            // Se guardan todos los recursos, torretas y poblados en listas
            LimitedAccesses = new List<LimitedAccess>(Scenario.GetComponentsInChildren<LimitedAccess>()); // No se puede buscar fuera del escenario, porque est·n las instalaciones de procesamiento, que tambiÈn son LimitedAccess
            Towers = new List<Tower>(Scenario.GetComponentsInChildren<Tower>());
            Villages = new List<Village>(Scenario.GetComponentsInChildren<Village>());

            // Cambia la velocidad del juego al valor inicial que hayamos dado
            Time.timeScale = TimeScale;

            // Todas las c·maras estar·n desactivadas y activo la primera
            Cameras[CameraIndex].enabled = true;
        }

        // Cambia el punto de vista, ciclando por todas las c·maras que tiene el escenario
        private void SwitchViewpoint()
        {
            // Desactivar la c·mara actual y activar la siguiente
            Cameras[CameraIndex].enabled = false;
            CameraIndex = (CameraIndex + 1) % Cameras.Count;
            Cameras[CameraIndex].enabled = true;
        }

        // Actualiza el gestor del escenario ajustando el zoom, seg˙n la entrada de la rueda del ratÛn.
        private void Update()
        {
            float fov = Cameras[CameraIndex].fieldOfView;
            fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Cameras[CameraIndex].fieldOfView = fov; 
        }

        // Cambia la velocidad del juego, ciclando por 0.5x, 1x, 2x, 5x, 10x
        private void SwitchScenarioSpeed()
        {
            // Tal vez se pueda usar Switch, pero hay que tener cuidado con la precisiÛn de los floats
            if (TimeScale > 9.9f)
                TimeScale = 0.5f;
            else
                if (TimeScale > 4.9f)
                    TimeScale = 10f;
                else
                    if (TimeScale > 1.9f)
                        TimeScale = 5f;
                    else
                        if (TimeScale > 0.9f)
                            TimeScale = 2f;
                        else
                            if (TimeScale > 0.49f)
                                TimeScale = 1f;

            Time.timeScale = TimeScale;
        }

        // Dibuja la interfaz gr·fica de usuario para que el administrador humano pueda trabajar mientras estudia la batalla entre dos controladores cualesquiera.
        public void OnGUI()
        {
            // Abrimos un ·rea de distribuciÛn centrada y abajo, con contenido en horizontal
            float halfWidth = Screen.width / 2;
            float halfAreaWidth = 100;
            float halfHeight = Screen.height / 2;
            float areaHeight = 50;
            GUILayout.BeginArea(new Rect(halfWidth - halfAreaWidth, Screen.height - areaHeight, halfWidth + halfAreaWidth, Screen.height));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace(); // Realmente no lo centra... estar˙} bien centrarlo

            if (GUILayout.Button("Switch Viewpoint", GUILayout.ExpandWidth(false)))
            {
                // Se va ciclando entre las c·maras disponibles
                SwitchViewpoint();
            }

            if (GUILayout.Button("Switch Scenario Speed", GUILayout.ExpandWidth(false)))
            {
                // Se va ciclando entre las distintas proporciones temporales
                SwitchScenarioSpeed();
            }

            if (GUILayout.Button("Pause", GUILayout.ExpandWidth(false)))
            {
                // Se va ciclando entre estar parado y al ritmo normal de juego
                // Se podr˙} hacer que hubiese un ritmo que fuese EL DOBLE de r·pido
                if (Time.timeScale == 0)
                    Time.timeScale = TimeScale;
                else
                    Time.timeScale = 0;
            }

            if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
            {
                // El reinicio es simplemente recargar la escena actual
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (GUILayout.Button("Quit", GUILayout.ExpandWidth(false)))
            {
                Application.Quit();
            }

            // Cerramos el ·rea de distribuciÛn con contenido en horizontal
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        // Comprueba si una posiciÛn estÅEsobre la superficie de un terreno
        // Posible mejora: Comprobar que la altura tambiÈn se corresponde con la altura del terreno en ese punto
        public bool InsideScenario(Vector3 position)
        {
            // Creo que no hace falta buscar el terreno activo con Terrain.activeTerrain
            // Adem·s la posiciÛn de un terreno siempre es su esquina con la X y la Z m·s pequeÒas
            Vector3 terrainPosition = Scenario.transform.position;

            if (position.x < terrainPosition.x)
                return false;
            if (position.x > terrainPosition.x + Scenario.terrainData.size.x)
                return false;
            if (position.z < terrainPosition.z)
                return false;
            if (position.z > terrainPosition.z + Scenario.terrainData.size.z)
                return false;
            return true;
        }

        // Cuando un poblado va a ser destruido, avisa antes de autodestruirse para que se le elimine de las listas del gestor del juego.
        // Posibles mejoras: Acabar con los poblados neutrales podr˙} suponer alg˙n tipo de penalizaciÛn para los controladores que ejecutan esas Ûrdenes...
        public void VillageDestroyed(Village village)
        {
            if (village == null)
                throw new ArgumentNullException("No se ha pasado un poblado.");

            Villages.Remove(village);
            // Podr˙} sacar un mensaje en consola
        }

        // Cuando una torreta va a ser destruida, avisa antes de autodestruirse para que se le elimine de las listas del gestor del juego. 
        public void TowerDestroyed(Tower tower)
        {
            if (tower == null)
                throw new ArgumentNullException("No se ha pasado una torreta.");

            Towers.Remove(tower);
            // Podr˙} sacar un mensaje en consola
        }
    }
}
