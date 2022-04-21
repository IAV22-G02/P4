# P4
Repository for the Artificial Intelligence signature of the UCM. Practice 4
___________________________________________________________________________


<!-- #Video de pruebas [aquí](https://youtu.be/kdz3KuYpfRA)-->


# Autores

## Grupo 02
López Benítez, Ángel   -   angelo06@ucm.es <br>
Rave Robayo, Jose Daniel   -   jrave@ucm.es <br>
Prado Echegaray, Iván   -   ivprado@ucm.es <br>
Mendoza Reyes, Juan Diego   -   juandiem@ucm.es <br>


## Resumen

La práctica consiste en implementar un prototipo de una simulación de combate entre dos facciones basado en la novela **Dune**, 
La batalla tiene lugar entre las fuerzas de los Fremen y las fuerzas del Emperador y el Barón Harkonnen en el planera Arrakis.
El cuadro temporal toma lugar diez mil años en el futuro, donde la sociedad se aprovecha económicamente de la extracción de una especia conocida como malogne,
que es conocida por alargar la vida e incluso despierta poderes psíquicos en algunas personas.
Paul Atrides lidera la rebelión del pueblo nativo de Arrakis contra El imperio y las Grandes Casas feudales que existen en la galaxia, como la liderada por el  Barón Vladimir Harkonnen. <br>

<br>
La práctica consta de un entorno 3D que representa un campo de batalla donde se desarrolla la simulación (controlado por **IA**) cada bando tiene dos construcciones, la instalación base y la instalación de procesamiento.<br>
El bando enemigo se centrará en la recolecíón de recursos y en la producción de unidades para destruir la la base del jugador	. <br>
El jugador tiene el mismo objetivo que la IA, y tendrá que competir contra ella en un entorno en el que ambos pueden recolectar los mismos recursos.
Ambos bandos pueden contruir las mismas unidades y la misma cantidad, estas unidades son la unidad extractora (máximo 5), la unidad exploradora(máximo 30). y la unidad destructora(máximo 10).<br>

## Unidades y contrucciones

+ **Instalación base.** Es un edificio que sirve para crear unidades a modo de barracón, puede crear cualquiera de las tres unidades posibles: extractora, exploradora o destructora
Los requisitos para la creación son tener el dinero necesario y no superar el límite de esas unidades, Puede recibir 100 puntos de daño, si se pierde, se pierde la partida.

+ **Instalación de procesamiento.** Almacena el solaris(dinero) recogido, no se le puede solicitar ninguna acción. Puede recibir 50 puntos de daño.

+ **Unidad Extractora.** Responsable de extraer la especia melange. Se le puede solicitar moverse para que cuando se encuentre con los campos de melagne empiece a extraer.
Tras extraerlo se dirige a una instalación de procesamiento, cuando entrega la especia, conseguimos los solaris correspondientes. Salvo nueva orden, repite la extracción en bucle.
En cada viaje obtiene 1000 solaris.
HP: 10
Precio: 10000
Máximo de unidades: 5

+ **Unidad exploradora.** Unidad más ágil y con capacidad de combate. Cuando llega al lugar al que se le he mandado se queda quieta si el lugar es tranquilo. Sin embargo, si se topa con un enemigo, le ataca y le presigue,
ya sea del bando enemigo o un poblado graben, si la atacan, esta contesta al agresor.
DPS: 2
HP: 5
Precio: 15000
Máximo número de unidades: 30

+ **Unidad destructora.** Más poderosa y resistente que la exploradora, pero también más lenta. No persigue a los enemigos ni se enfrenta a los agresores, si no que se centra en su objetivo actual.
DPS: 5
HP: 20
Precio: 30000
Máximo número de unidades: 10

El controlador automático: 
Funciona a forma de capitán gneeral del ejercicto, da órdenes a la unidades de su propio ejército.
Estas órdenes se razonan a través de sondeos del escenario, que se realizan cada frame o cada cierto tiempo.
Estos razonamientos se realizan teniendo en cuenta el tiempo que tardan en construirse las unidades y que las órdenes muy seguidas
y contradictorias pueden ser contraproducentes y provocar bloqueos.
Se realizan sondeos de del escenario, intalaciones y distintas unidades, utilizando mapas de unfluencia con división de baldosas.
Se reservan las teclas F, G y H para mostrar y ocultar las influencias de los Fremen, Graben o Harkonen, respectivamente

Apartados:

+ **A.** Mostrar en una primera ejecución, sin necesidad de tocar nada, el controlador
automático enfrentándose a sí mismo en el escenario de ejemplo del juego de
estrategia en tiempo real proporcionado por el profesor [0,5 ptos.].<br><br>

+ **B.** Incluir la posibilidad de probar al menos una nueva variante del escenario
desarrollada por vuestro grupo para probar el controlador automático, permitiendo
enfrentar al controlador de IA contra un jugador humano, además de contra sí mismo
(con los mismos o diferentes parámetros); siempre en batallas 1 contra 1 [0,5 ptos.].<br><br>

+ **C.** Visualizar a voluntad el mapa de influencia utilizado, y cómo se va actualizando según
cambia la situación táctica del juego [1 ptos.]..<br><br>

+ **D.** Demostrar cierta robustez en el funcionamiento del controlador, tratando de evitar la
inactividad o los bloqueos de las unidades, así como cierta capacidad de reacción a la
hora de defenderse del enemigo y cierta proactividad a la hora de atacarlo para ganar la
partida [1 pto.]
<br><br>

+ **E.** Demostrar mediante pruebas diseñadas a tal efecto un nivel de competencia razonable
en el juego, aprovechando el tiempo y los recursos disponibles, venciendo sin
problemas a controladores incompetentes, sean humanos o no. Demostrar capacidad de
adaptación tanto a diversas situaciones iniciales (empezar con o sin muchas unidades,
con o sin mucho dinero, etc.) como a cambios bruscos en la situación táctica de la
batalla (pérdida de las unidades, carencia total de solaris, etc.) . <br><br>


# Descripción Punto de Partida

## [Commit](https://github.com/IAV22-G02/P4/commit/e4ea84e275632e71d3d0992caaceb848a236ca00) de Punto de Partida 

La escena inicial contiene un **Escenario de batalla** con todas las instalaciones antes comentadas, con un estandarte representando al bando que pertenece,y unidades extractoras, exploradoras y destructoras de cada bando, cada una a su vez, representada por un color amarillo o azul.<br>
A su vez, también se encuentran implementadas mallas de navegación y herramientas de percepción y movimiento dinámico de Unity.<br>
 Y en cuanto a scripts se encuentran:<br>

**Health**: Se explica por si solo, perteneciente a la gran mayoría de los objetos<br>
**Proyectile**: También bastante autoexplicativo, afecta a Health.<br>
**Village**: Clase encargada de gestionar los comportamientos de los poblados<br>
**Facilities**: Clase encargada de gestionar los comportamientos de las intalaciones encargadas de la logística del ejército.<br>
**Scripts de instalaciones**: Clases de comportamiento de las diferentes instalaciones(Bases, Procesamiento...) 
**Tower**: Clase encargada del comportamiento agresivo y neutral frente a amenazas<br>
**Unit**: Clase que gestiona los diferentes comportamientos inteligentes y movilidad del ejército<br>
**Scripts de unidad**: Clases exclusivas a cada tipo de unidad de juego(destructora, exploradora y extractora)
**RTSManagers**: Clases usadas para la gestión de funcionalidades y/o comportamientos, utilizados por el escenario, responsable de identificar y mantener la lista de todos los recursos y demás elementos, y un gamemanager, responsable de poner en marcha el juego, iniciar su estado y llevar un seguimiento de todos sus cambios.
**RTSContollers**: Clases que usan un mapa de influencia y/o corrutinas para controlar las decisiones que necesitará cada bando para su correcto uso, entre ellos se encuentran los automáticos(AI y Random), que permite implementar bots tácticos para jugar automáicamente al juego, usando una corrutina,y del jugador(Player y PlayerRandom), que ofrecen una interfaz al jugador humano para que mande órdenes a uno de los ejécitos.
**Scripts de tasks**: Múltiples scripts de tareas que se asignan a su correspondiente unidad en tiempo de ejecución<br>
**Componentes NavMesh**: Componente usado en las unidades para facilitar la navegación<br>

# Estructura de Clases

<br>

## Descripción de la Solución

La solución no consta de la implementación de  nuevos componentes, sino del correcto funcionamiento de los ya existentes, y las posibles la mejora de estos, entre ellas:
+ Crear un Script Scenario y darle algunas propiedades como el nombre, autor, nivel de dificultad, nivel de justicia o simetría, número de jugadores(RTSSceneraioManager).
+ Añadir unas constantes que proporcionen valores por defecto para atributos de las instalaciones como su coste, velocidad, etc(Facilities).
+ Añadir un método de Reset que permita volver a los valores iniciales de la instalación (Facilities).
+ Cargar distintos árboles de comportamiento, según se esté modo de ataque (como las exploradoras cuando persiguen) o más tranquilo(Unit).
+ Los poblados una vez destruidos puedan regenerarse y volver a aparecer(Village y Tower).
+ confirmar que los misiles no colisionan ni interfieren entre sí y tener prefabs distintos para cada tipo de proyectil(Proyectil).

### Opcionales

La solución también consta de funcionalidades opcionales tales como:
+ Mejora aspectos del escenario, sonidos o efectos visuales para mejorar su
estética.
+  Mejora aspectos de la interfaz y el control para el jugador humano.
+ Mejora aspectos del movimiento y la navegación de las distintas unidades,manteniendo las condiciones normales básicas.
+ Mejora el controlador para jugadores humanos, de manera que reciba feedback visual y
sea posible precisar la posición a la que ordenar moverse a las unidades seleccionadas.


El pseudocódigo de dichos componentes:
### Selector Task (Units)
```python
class Task:
	# Return on success (true) or failure (false).
	function run() -> bool

class EnemyNear extends Task:
	function run() -> bool:
	# Task fails if there is no enemy nearby.
	return distanceToEnemy < 10

class PlayAnimation extends Task:
	animationId: int
	speed: float = 1.0
	function run() -> bool:
	if animationEngine.ready():
		animationEngine.play(animationId, speed)
		return true
	else:
		# Task failure, the animation could not be played.
		return false

class Selector extends Task:
	children: Task[]

	function run() -> bool:
	for c in children:
		if c.run():
			return true
	return false

class Sequence extends Task:
	children: Task[]
	
	function run() -> bool:
	for c in children:
		if not c.run():
			return false
	return true


```


# Referencias Usadas:
+ AI for GAMES Third Edition, Ian Millintong
+ Unity 5.x Game AI Programming Cookbook, Jorge Palacios
