# Lost in the Dark

## 1) Introducción

- Lost in the Dark es un videojuego de estilo terror desarrollado en realidad virtual para la asignatura Interfaces Inteligentes de 4º curso de la Universidad de la Laguna. El objetivo del juego es sobrevivir el mayor tiempo posible en un entorno oscuro y con enemigos. Se dispone de un arma y munición infinita, pero esto no es suficiente para sobrevivir. La puntuación final se basa en la cantidad de monstruos que se consigue derrotar y el daño infligido total.

## 2) Mecánicas

En cuanto a las mecánicas del juego tenemos:
  - **Menús:** hay un sistema de menús que permiten configurar ciertos parámetros del juego. En el menú principal podemos, modificar el volumen, empezar una partida eligiendo una dificultad o salir del juego. En el menú final, se muestran las estadísticas de la partida jugada así como un botón para regresar al menú inicial.

  - **Dificultad**: el juego permite elegir desde el menú de inicio, la dificultad a la que se desea jugar. Esta dificultad afecta a bastantes parámetros del juego, como puede ser la vida de los enemigos, la vida del propio jugador y la cantidad de enemigos totales en partida.
  - Entorno: el entorno de juego está compuesto por
    - **Mapa**: los mapas son siempre diferentes, dado que se trata de evitar que el usuario pueda aprenderse los caminos. Se trata de un generador de laberintos desarrollado por mi y que permite una automatización del diseño del nivel bastante efectivo. Como futura actualización, se puede introducir el diseño del mapa por semilla, ya que el algoritmo posee esta cualidad actualmente, pero no se ha introducido en el juego como tal.
    - **Spawner controller**: el juego cuenta con una mecánica de aparición de enemigos constante, lo que garantiza que siempre hay enemigos que vienen a por ti y nunca un periodo de descanso / relajación. Este sistema se basa en la dificultad del juego y por tanto, cuanto mayor es, más enemigos mantiene en el juego.
    - **Sonidos**: hay variedad de sonidos ambiente para producir un mayor grado de inmersión en el juego.


  - Personaje principal:
    - **Movimiento de cámara:** el movimiento del jugador al ser en realidad virtual se produce por head tracking, es una mecánica no implementada como tal, pues ya la provee el SDK de GoogleVR, pero es importante mencionarlo.
    - **Movimiento de personaje:** por otra parte, el movimiento del personaje si se ha implementado desde cero. Este movimiento es siempre relativo a dónde esta mirando el jugador.
    - **Arma:** el jugador posee un arma con la cual se defiende. Esta tiene munición infinita y un cargador de 25 balas. El tiempo de recarga es una variable, pero está definida en 1.6 segundos.
    - **Sonido:** el jugador percibe y emite sonidos. Los sonido más importante es el de los pasos y el arma, algo importante para futuras líneas de trabajo (multijugador o detección de sonidos por parte de los enemigos)


  - Enemigos:
    - **Tipos:** tenemos tres tipos de enemigos, un esqueleto, una araña y un troll, que viene a ser el semi boss del juego. Para todos se ha implementado un script de control único, lo cual fue un reto pero que simplifica la cantidad de código.
    - **Movimiento:** los enemigos siempre irán por el camino más corto del entorno hasta el jugador. Esto lo hace la AI de Unity3D, ya que utilizan el componente **NavMeshAgent** para desplazarse. Este sistema presenta un problema, necesitan conocer que superficie pueden utilizar para moverse y el juego genera un mapa diferente en cada partida. Tras un poco de investigación, se pudo realizar un sistema que crea la superficie tras la generación del mapa y permite una correcta navegación a los enemigos.
    - **Animación:** esta mecánica esta dentro del control único de los enemigos, donde todos tienen cuatro animaciones del mismo tipo y un único script desarrollado para controlarlos a todos. A pesar de que sean iguales, cada enemigo contiene una animación propia a reproducir en cada situación. Estas son:
        - Parado / Sin desplazarse
        - Caminando
        - Atacando
        - Muerte
    - **Sistema de melee:** uno de los grandes retos de este juego fue que los enemigos pudieran atacar al personaje cuerpo a cuerpo. Tras una fase de aprendizaje, para cada enemigo se ha introducido un sistema que permite golpear exclusivamente cuando están realizando la animación de ataque. Este se basa en activar o desactivar, una cápsula que contiene un **trigger collider** en el arma del enemigo y permite detectar si se golpea o no al jugador.
    - **Sonidos:** los enemigos emiten un sonido personalizado aleatoriamente para evitar solapamiento entre diferentes enemigos del mismo tipo.


  - **Sistema de vida:** esta mecánica afecta tanto a jugador como a enemigos por partes iguales dependiendo de la dificultad. Si esta va en aumento, la vida del personaje disminuye y la de los enemigos aumenta. Por otra parte, cuanto más fácil, más vida tiene el jugador y menos los enemigos.

  - **Sistema de dropeos y habilidades:** el jugador puede obtener al matar enemigos algunos power ups que pueden aparecer bajo cierta probabilidad. Esta probabilidad se rige por:
    - Cada vez que se elimina a un enemigo tiene un 1% de probabilidad de dejar o no un item.
    - Si se decide que va a dejar algo cuando muere, entonces tenemos cuatro posibilidades. Con un 50% podemos obtener un item para recuperar la vida, con un 30% se puede obtener un incremento de 20 segundos en la velocidad, con un 15%, aumenta el daño durante 20 segundos del arma principal y con un 5% podemos matar a todos los enemigos de golpe. Este último se maneja por delegados / eventos.


  - **Ambientación:** al ser un survival de horror, los sonidos y las luces intentan asustar al jugador en todo momento. El personaje principal se encuentra en un entorno oscuro con solo una linterna para saber que hay y puede recibir sonidos que le asusten de los enemigos o el ambiente en cualquier momento.

  - **Estadísticas:** a lo largo del juego hay un sistema de recolección de varios datos importantes que luego se muestran al jugador y se calcula su puntuación en base a ellos. Estas recogen:
    - Tiempo sobrevivido
    - Cantidad de enemigos derrotados
    - Daño infligido
    - Daño recibido
    - Cantidad de balas disparadas
    - Power ups recogidos


  - **Interfaz en inglés:** pese a que no sea una mecánica como tal, destacar que toda la interfaz está en inglés, así como el código y los comentarios de este.

## 3) Assets Utilizados

* Creado por mí:
    - **Maze Generator**: crea el entorno de juego, el cual es un laberinto que tiene todos los caminos conectados y también caminos sin salida, es decir, tienes acceso a todo el laberinto siempre que encuentres el camino adecuado. Añade un punto de dificultad extra al juego. Cada vez que el juego empieza, se genera uno totalmente diferente al anterior, lo cual impide conocer el mapa de juego. Para la generación del laberinto en Unity, una vez se ha construido el diseño por el programa, se tiene que llevar al mapa y para ello se hace uso de un asset encontrado en la store, **Decrepit Dungeon LITE**, que contiene suelos, paredes, techos, pilares y marcos de puertas. Además se modificaron alguno de estos prefabs para mejorar la apariencia del juego, por ejemplo, los marcos de puerta se modificaron para añadirles antorchas y puertas rotas.
    - **Menús:** hay un menú de inicio que permite comenzar el juego (en varias dificultades), salir o ajustar el volumen y un menú post juego, donde se muestran las estadísticas de la partida. Estos se han creado desde cero añadiendo algunas fuentes para las letras encontradas por internet que dan un toque de horror al juego. Los sonidos se han encontrado [aquí](https://freesound.org/).
    - Scripts creados: para el funcionamiento del juego se han creado bastantes scripts para la coordinación de todos ellos. A destacar tenemos los siguientes:
        - **Jugador FPS:** el controlador de primera persona (movimiento y sonidos de daño) se ha creado desde cero, al igual que su cuerpo y control de daño por los monstruos.
        - **Sistema de lucha:** se ha creado un sistema de lucha para los monstruos a melee, haciendo uso de eventos en las animaciones.
        - **Sistema de spawn:** un sistema de spawn basado en la dificultad del juego elegida que mantiene siempre monstruos en el juego.
        - **Sistema de dropeo:** cuando los monstruos mueren utilizan un sistema probabilístico sencillo para decidir si deja o no un power up.


* Asset store:
    - **Sonidos:** los sonidos que contiene el juego es una mezcla de varios assets encontrados y también de sonidos encontrados navegando en Internet. Algunos nombres de estos assets son: **Horror SFX**, **Monster SFX** y **Horror ambient sound**
    - **GoogleVR:** no pertenece a la asset store como tal, pero es un plugin básico del juego para llevarlo a la realidad virtual.
    - **NavMeshComponent:** al igual que GoogleVR, no es un asset como tal, pero si un plugin externo que ayuda a los monstruos a localizar al jugador e ir hasta él.
    - **Decrepit Dungeon LITE:** como ya se mencionó antes, es un plugin que añade prefabs de dungeons. Utilizado para la construcción del laberinto.
    - **Easy Weapons:** un plugin bastante amplio, que facilita el uso de las armas y del cual solo hemos elegido una pistola. Nos facilita animaciones de disparo y recarga y modificando algunos trozos de código hemos permitido dañar a los monstruos del juego.
    - **Fantasy Monster Skeleton:** un asset que contiene un esqueleto junto a algunas animaciones. Forma parte de uno de los enemigos de nuestro juego.
    - **Animated Spider:** un asset que contiene una araña junto a algunas animaciones. Forma parte de uno de los enemigos de nuestro juego.
    - **Creature Cave Troll:** un asset que contiene un Troll junto a algunas animaciones. Forma parte de uno de los enemigos de nuestro juego y es considerado como un **boss**.
    - **Blood Damage Effect:** un plugin que facilita la animación de daño introduciendo sangre en la pantalla, como algunos juegos de la actualidad (Call of Duty, Battlefied, etc). Con algunas modificaciones básicas hemos conseguido el resultado que queríamos.
    - **Mobile power Ups full:** paquete de iconos o comúnmente conocidos como **power ups**. Contiene una gran variedad de prefabs con animación de rotación y del cual hemos escogido solamente cuatro.


## 4) Hitos logrados
- Generación de entorno automática diferente para cada partida.
- Controlador de enemigos (Movimiento y control de spawn).
- Sistema de dificultad.
- Sistema de menús.
- Sistema de sonido que permite un mayor grado de inmersión.
- Movimiento del jugador con un controlador.
- Sistema de lucha cuerpo a cuerpo para los monstruos.
- Sistema de disparo con una pistola para el jugador.
- Sistema de vida en base a la dificultad.
- Sistema de dropeos y habilidades.
- Ambientación de tipo horror.
- Recolección de estadísticas.
- El juego es capaz de ejecutarse sin apenas lag en un dispositivo android de gama media (Testeado en Mi A1. [Características del dispositivo](https://www.mi.com/es/mi-a1/)).
- Corrección de bugs importantes que aparecieron durante el testeo.

## 5) Aspectos a destacar
- Los aspectos que más se pueden destacar del juego son el generador del laberinto, un proyecto propio creado desde cero, el sistema de lucha cuerpo a cuerpo que puede llegar a ser bastante complejo, las habilidades que se pueden obtener de los dropeos de los monstruos, la combinación de diferentes sonidos y la creación del FPC (First Person Controller) desde cero.

- Se intentó implementar el resonance audio en el juego, pero por falta de tiempo y la carencia de cascos con las características necesarias para simularlo, se descartó finalmente y sería un hito pendiente.

## 6) Aspectos para la interacción con VR
- Para la interacción con la realidad virtual, se ha tenido en cuenta los consejos de distancias de paneles que puedan contener texto escrito, lo cual evita fatiga ocular a la hora de navegar entre los menús del juego.
- Se puede identificar con facilidad todo objeto interactuable con el usuario (en el caso de este juego, solo son botones). A pesar de que ya venga contemplado en el SDK de GoogleVR, la retina cambia de forma cuando se da el caso y permite al jugador detectar los diferentes botones.
- No se ha tenido en cuenta, por fallo/despiste, los consejos de giro máximo del cuello. Ocasionalmente podemos encontrarnos situaciones donde tengamos enemigos debajo de los pies (arañas) y sobrepasa los límites recomendados.

## 7) Ejecución
**Falta un gameplay**

## 8) Desarrollador
- Abel Delgado Falcon: <abeldelgadofalcon@gmail.com>
