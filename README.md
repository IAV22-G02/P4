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

## Estancias y comportamiento en estas

+ **Patio de butacas(P).** Estancia incial del público, dividido en Este y Oeste. Los espectadores huyen al vestíbulo si cae la lámpara correspondiente a su lado del patio de butacas,
Esta conectada con el escenario y el vestíbulo, y es visible desde los palcos.

+ **Vestíbulo(V).** Es la zona más externa de la ópera, donde van los bloques de público
cuando se asustan. Simplemente conecta con el patio de butacas.

+ **Escenario(E).** Estancia inicial de la cantante, que intercala con las bambalinas(donde se toma su descanso), también conecta con el patio de butacas, los palcos y es posible dejarse caer
al sótano oeste, aunque no es posible volver, el fantasma no puede pisar estas estancias si hay público mirando, aunque puede capturar a la cantante y llevársela a donde quiera, soltándola 
por voluntad propia o porque se sienta intimidado por un choque con el vizconde. Si la cantante acaba en una estancia desconocida, empieza a vagar entre estancias hasta ser encontrada por el vizconde
que la lleva hasta una estancia que ella conozca, donde pueda volver al escenario o a las bambalinas

+ **Bambalinas(B).** Estancia donde suele descansar la cantante y que conecta con el escenario, el sótano oeste y que permite deslizarse por una rampa algo oculta al sótano este, sin posibilidad de regresar. 

+ **Palco oeste(Po).** Estancia inicial del vizconde, tiene una palanca para dejar caer la lámpara oeste del patio de butacas. 
Conecta con el escenario, con el sótano oeste y permite ver el patio de butacas 

+ **Palco oeste(Pe).** Estancia similar al palco oeste, con una palanca que se puede usar para dejar caer la lámpara este del patio de butacas. 
Conecta con el escenario, con el sótano este y permite ver el patio de butacas, aunque sin visibilidad en el otro sentido.

+ **Sótano oeste(So).**  Estancia que conecta con el palco oeste, con las bambalinas y con el
sótano norte, aunque para recorrer esta conexión hace falta subirse a una barca. Solo puede subirse una persona(o una con una en brazos). La barca comienza en el sótano norte,
pero hay una palanca para acercarla a cualquier lado del sótano.

+ **Sótano este(Se).**  Estancia que conecta con el palco este, y tanto con el sótano norte
como con la sala de música donde compone su obra el fantasma, aunque para recorrer estas dos
últimas conexiones hacen falta barcas. Por defecto, la barca que lleva al sótano norte sí está en
esta orilla, pero la que lleva a la sala de música está en la orilla contraria. Aunque se puede
llegar a esta estancia desde las bambalinas, por una trampilla, desde aquí no se conecta con las
bambalinas.

+ **Celda(C).**  Estancia donde el fantasma deja a la cantante para completar su secuestro
con éxito, usando una palanca que activa unas rejas que la impiden salir (y que por supuesto el
vizconde podrá desactivar). Conecta con el sótano norte.

+ **Sótano norte(N).** Estancia que conecta con la celda, además de con la sala de música,
el sótano este y el sótano oeste a través de sus correspondientes tres barcas.

+ **Sala de música(M)** Estancia inicial del fantasma, conecta mediante una barca con el sótano este, y con otra con el sótano
norte.. El fantasma tiene el objetivo principal de secuestrar a la cantante, para lo que intentará
buscarla en las bambalinas, en el escenario o si no logra dar con ella, explorando las demás
estancias meticulosamente por si estuviera "perdida" por allí. No puede acceder al escenario si
hay público mirando, de modo que, como objetivo secundario, necesita tirar las dos lámparas
del techo para vaciar del todo el patio de butacas. Sea como sea, una vez atrapada la cantante, la
llevará consigo hasta la celda, intentando usar siempre el camino con menor coste (recordando
la última posición de las barcas y del vizconde que conoce, y eligiendo la ruta con menor coste,
la que tenga más barcas a su favor y que evite al héroe de esta historia). Cuando llega hasta la
celda la soltará allí, activará las rejas e irá hasta la sala de música, permaneciendo allí
indefinidamente. Lo único que desconcentra al fantasma cuando está componiendo es escuchar
a su musa cantar de nuevo en el escenario, reavivando sus deseos de secuestrarla y encerrarla
otra vez en su celda. Por otro lado, si el fantasma llega a percibir el ruido de los golpes del
vizconde a su piano, abandonará lo que está haciendo (soltando a la cantante) y correrá
enfurecido hasta allí para dedicar unos segundos a arreglar semejante estropicio.



Las funcionalidades mñinimas que se piden son: 

+ **A.** Mostrar el entorno virtual (la casa de la ópera), con un esquema de división de malla de
navegación proporcionado por Unity, donde se ubiquen todos los elementos descritos
anteriormente. El vizconde será controlado libremente por el jugador mediante los
cursores y una única tecla de acción para interactuar con otros elementos. Aunque haya
cámaras que sigan a cada uno de los personajes, conviene que haya una adicional que
nos dé la vista general del entorno [0,5 ptos.].<br><br>

+ **B.** Hacer que parte del público huya tras la caída de una lámpara, y regrese en cuanto está
arreglada. Será una navegación y un movimiento trivial, sin apenas decisión [0,5 ptos.].<br><br>

+ **C.** Representar a la cantante como un agente inteligente basado en una máquina de estados
que pasa del escenario a las bambalinas cuando toca, que puede ser "llevada" por los
otros dos personajes hasta otra estancia, que navega algo desorientada cuando está en
las estancias subterráneas, y que se deja llevar por el vizconde, con la esperanza de
reencontrar el escenario y continuar su rutina allí. Tiene navegación, movimiento y
percepción sencillos, y decisión mediante máquina de estados [1 ptos.].<br><br>

+ **D.** Desarrollar el árbol de comportamiento completo del fantasma, para que busque a la
cantante, tire las lámparas, la capture, la lleve a la celda, active las rejas, etc. [1 pto.]<br><br>

+ **E.** Usar un sistema de gestión sensorial para que el fantasma reaccione realmente a lo que
ve (en la propia estancia o estancias vecinas visibles) y lo que oye (el canto de su musa
y el ruido de la sala de mísica), sin tener que recurrir a información privilegiada
(únicamente recordando lo que ha visto anteriormente) [1 pto.]. <br><br>



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
