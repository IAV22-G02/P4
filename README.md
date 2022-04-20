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



Modelo de Vizconde             |  Modelo de Fantasma     |  Modelo de cantante
:-------------------------:|:-------------------------:|:---------------------:
<img src="https://github.com/IAV22-G02/P3/blob/main/Viscount.png" alt="drawing" width="200"/>|  <img src="https://github.com/IAV22-G02/P3/blob/main/Ghost.png" alt="drawing" width="200"/>|  <img src="https://github.com/IAV22-G02/P3/blob/main/Singer.png" alt="drawing" width="200"/>

# Descripción Punto de Partida

## [Commit](https://https://github.com/IAV22-G02/P3/commit/8f2763186f347ed4252ba3a6574eedd179715527) de Punto de Partida 

La escena inicial contiene un **Mapa** con todas las estancias antes comentadas y sus correspondientes conexiones entre ellas, modelos del público, el vizconde, el fantasma y la cantante, además de los prefabs de objetos interactivos como son el piano, las palancas y las barcas. Y en cuanto a scripts se encuentran:<br>

**Game Blackboarf**: Clase que tiene la información de las estancias del mapa y palancas<br>
**Player**: Clase encargada de gestionar las acciones del vizconde<br>
**Cantante**: Clase encargada del comportamiento y movimiento de la cantante<br>
**CameraMananager**: Clase que gestiona los diferentes puntos de vista en el escenario<br>
**Scripts de interacción**: Múltiples scripts de objetos interactivos que avisan de un contacto con ellos<br>
**NavMesh, StateManager y Behaviour Tree**: Pertenecientes a los personajes, que se encargan de la toma de decisiones y pathing por el mapa<br>

# Estructura de Clases

<br>

## Descripción de la Solución

La solución consta de la implementación de  nuevos componentes, cuyo pseudocódigo está más abajo:
+ Componente TreeDecisionMaking, que se encarga de la gestión de decisiones de los behaviour trees.
+ El componente PathFinder, que usaremos para encontrar el camino más corto a la sala del fantasma cuando secuestra la cantante y que lo siga.
+ Componente Wander o Merodeo, para que la cantante vague por las estancias
+ El componente StateMachineManager, encargado de  gestionar los states del statemachine.
+ Componente TaskSelector, que decidirá las acciones que tomará el fantasma al llegar a una sala con ayuda del behaviour tree.

Además usaremos algunos los componentes implementados en la práctica 2, pero algo modificados para este ejercicio, que se pueden ver en el enlace de más abajo.
Sobretodo los componentes para el movimiento de Teseo. Por otro lado, cambiaremos un poco el comportamiento de Merodeo de la cantante para que se mueva como si estuviera "perdida" por el mapa. Más abajo se puede ver el código de lo que se tiene en mente. La explicacion de la estructura se puede ver [aquí](https://github.com/IAV22-G02/P2)

### Opcionales

La solución también consta de funcionalidades opcionales tales como:
+ Mejora del razonamiento del fantasma sobre el estado y la posición de los distintos
elementos (las barcas, la cantante, el vizconde, etc.), de modo que pueda tomar
decisiones más inteligentes, considerando los efectos causados por otros personajes


El pseudocódigo de dichos componentes:

## TreeDecisionMaking (Fantasma y cantante)
```python
class DecisionTreeNode:
	 # Recursively walk through the tree.
	function makeDecision() -> DecisionTreeNode

class Action extends DecisionTreeNode:
	function makeDecision() -> DecisionTreeNode:
		return this

class Decision extends DecisionTreeNode:
	trueNode: DecisionTreeNode
	falseNode: DecisionTreeNode

	# Defined in subclasses, with the appropriate type.
	function testValue() -> any

	# Perform the test.
	function getBranch() -> DecisionTreeNode

	# Recursively walk through the tree.
	function makeDecision() -> DecisionTreeNode

class FloatDecision extends Decision:
	minValue: float
	maxValue: float

	function testValue() -> float

	function getBranch() -> DecisionTreeNode:
	if maxValue >= testValue() >= minValue:
		return trueNode
	else:
		return falseNode

class Decision extends DecisionTreeNode:
 function makeDecision() -> DecisionTreeNode:
 # Make the decision and recurse based on the result.
 branch: DecisionTreeNode = getBranch()
	 return branch.makeDecision()

class MultiDecision extends DecisionTreeNode:
	daughterNodes: DecisionTreeNode[]

	function testValue() -> int

	# Carries out the test and returns the node to follow.
	function getBranch() -> DecisionTreeNode:
		return daughterNodes[testValue()]
	# Recursively runs the algorithm, exactly as before.
	function makeDecision() -> DecisionTreeNode:
	branch: DecisionTreeNode = getBranch()
		return branch.makeDecision()

class RandomDecision extends Decision:
	lastFrame: int = -1
	currentDecision: bool = false

	function testValue() -> bool:
	frame = getCurrentFrame()

	# Check if our stored decision is too old.
	if frame > lastFrame + 1:
		# Make a new decision and store it.
		currentDecision = randomBoolean()

	# Either way we need to store when we were last called.
	lastFrame = frame

	return currentDecision

```

### Wander (Cantante)
```python
  # Is intersection uses navigation instead of movement
  function IsIntersection(position)-> bool:
  	coord : vector2
	coord = posToGrid(position)
	
	return haveNeighboursIntersection(coord.x, coord.y)
  
  
class KinematicWander :
  character: Static
  maxSpeed: float
  position: vector2
  # The maximum rotation speed we'd like, probably should be smaller
  # than the maximum possible, for a leisurely change in direction.
  maxRotation: float
 
  function getSteering() -> KinematicSteeringOutput:
	result = new KinematicSteeringOutput()

	newDir: vector2 
	if(isIntersection(position) || randomBinomial(0, 10) < 3)
		toDir = randomDirection(position)

		newDir = toDir - position; 

		# Get velocity from the vector form of the orientation.
		result.velocity = maxSpeed * newDir

	return newDir;
```


## StateMachine(Cantante y publico)
```python
class StateMachine:
	# We’re in one state at a time.
	initialState: State
	currentState: State = initialState

	# Checks and applies transitions, returning a list of actions.
	function update() -> Action[]:
	# Assume no transition is triggered.
	triggered: Transition = null

	# Check through each transition and store the first
	# one that triggers.
	for transition in currentState.getTransitions():
	if transition.isTriggered():
		triggered = transition
		break

	# Check if we have a transition to fire.
	if triggered:
		# Find the target state.
		targetState = triggered.getTargetState()

		# Add the exit action of the old state, the
		# transition action and the entry for the new state.
		actions = currentState.getExitActions()
		actions += triggered.getActions()
		actions += targetState.getEntryActions()

		# Complete the transition and return the action list.
		currentState = targetState
		return actions

	# Otherwise just return the current state’s actions.
	else:
		return currentState.getActions()

	

```

### Selector Task (Fantasma)
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
