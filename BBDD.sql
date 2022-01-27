DROP DATABASE IF EXISTS M3Juego;
CREATE DATABASE M3Juego;

USE M3Juego;

CREATE TABLE Jugador (
	id INT NOT NULL,
	nombre VARCHAR(60),
	contraseña VARCHAR(60),
	nivel INT NOT NULL,
	experiencia INT NOT NULL,
	conectado INT NOT NULL,
	PRIMARY KEY(id)
);

CREATE TABLE Partida (
	id INT NOT NULL,
	fecha VARCHAR(60),
	duracion INT,
	ganador VARCHAR(60),
	PRIMARY KEY(id)
);

CREATE TABLE Historial (
	idJ INT NOT NULL,
	idP INT NOT NULL,
	posicion INT,
	FOREIGN KEY(idJ) REFERENCES Jugador(id),
	FOREIGN KEY(idP) REFERENCES Partida(id)
);

CREATE TABLE Carta (
	id INT NOT NULL,
	nombre VARCHAR(60),
	vida INT NOT NULL,
	ataque INT NOT NULL,
	defensa INT NOT NULL,
	descripcion VARCHAR(500),	
	PRIMARY KEY(id)
);

CREATE TABLE Mazo (
	idJ INT NOT NULL,
	idC INT NOT NULL,
	FOREIGN KEY (idJ) REFERENCES Jugador(id),
	FOREIGN KEY (idC) REFERENCES Carta(id)
);

INSERT INTO Jugador VALUES(1,'Pau','3c',10,11000,0);

INSERT INTO Carta VALUES(1,'Kate Bishop',100,120,100,'Kate Bishop utiliza sus notables habilidades de combate para pelear por una buena causa, ya sea como Joven Vengadora, junto al mentor Clint Barton, o por su cuenta.');
INSERT INTO Carta VALUES(2,'Ant-Man',135,170,160,'Superando su pasado criminal, Scott Lang sigue los pequenos, pero poderosos, pasos de su predecesor como el heroe que cambia de tamano conocido como Ant-Man.');
INSERT INTO Carta VALUES(3,'Lizard',180,160,130,'Doctor Connors conocido como Lizard esta de vuelta en su forma reptil, listo para enfrentarse a Spider-Man.');
INSERT INTO Carta VALUES(4,'War Machine',195,200,145,'El veterano militar James Rhodes esta listo para el combate con su armadura avanzada, agregando un arsenal formidable a los disenos creados por Tony Stark.');
INSERT INTO Carta VALUES(5,'Miles Morales',210,190,140,'Emergiendo de un universo que necesitaba un nuevo Spider-Man, un adolescente de Brooklyn llamado Miles Morales acepto el desafio. Reacio al principio, rapidamente se gano el manto de un superheroe.');
INSERT INTO Carta VALUES(6,'Daredevil',170,130,110,'Cuando era nino, Matt Murdock perdio la vista y gano sentidos sobrehumanos en un accidente que le enseno a vivir sin miedo. Luego paso a estudiar derecho, lo que lo llevo a una vida de lucha contra el crimen en Nueva York; como abogado y como vigilante Daredevil.');
INSERT INTO Carta VALUES(7,'Groot',215,180,135,'Este arbol alienigena sensible se ramifica fuera de su zona de confort para ayudar a los Guardianes de la Galaxia a mantener a salvo a la gente del universo.');
INSERT INTO Carta VALUES(8,'Rocket',150,140,110,'Como experto en armas y tacticas de los Guardianes de la Galaxia, Rocket arriesga su pellejo para defender el cosmos.');
INSERT INTO Carta VALUES(9,'Black Widow',125,150,115,'Natasha Romanoff, separada de los Vengadores (ahora fracturados), enfrenta el camino oscuro que tomo para convertirse en espia y asesina, asi como los eventos que siguieron.');
INSERT INTO Carta VALUES(10,'Doctor Octopus',180,160,140,'Un experimento del cientifico Dr. Otto Octavius se vuelve inestable y fusiona un conjunto de tentaculos mecanicos inteligentes en el cuerpo y el cerebro de Otto, lo que lo lleva a convertirse en el poderoso super villano Doctor Octopus.');
INSERT INTO Carta VALUES(11,'Falcon',150,140,110,'Cuando Steve Rogers le pidio ayuda al veterano de la Fuerza Aerea Sam Wilson, Wilson acepto de inmediato. Se puso el traje de vuelo que habia usado en combate para convertirse en Falcon, lo que lo encamino a convertirse en un Vengador y, finalmente, en el Capitan America.');
INSERT INTO Carta VALUES(12,'Winter Soldier',200,165,120,'James Buchanan "Bucky" Barnes se alista para luchar en la Segunda Guerra Mundial, pero finalmente cae en la batalla. Desafortunadamente, el malvado Arnim Zola lo recupera y borra su memoria, convirtiendolo en un asesino altamente entrenado llamado Winter Soldier. Reformado por sus amigos, ahora lucha junto a los Vengadores.');
INSERT INTO Carta VALUES(13,'Mantis',120,130,125,'Mantis usa sus poderes para proteger la galaxia contra aquellos que buscan danarla. Aislada de otras formas de vida desde una edad temprana por Ego, Mantis no comprende las complejidades del compromiso social; pero esta aprendiendo al ser reclutada por los Guardianes de la Galaxia.');
INSERT INTO Carta VALUES(14,'Vulture',165,135,120,'Adrian Toomes, tambien conocido como Vulture, es uno de los enemigos mas duraderos de Peter Parker. Con un resentimiento contra Spider-Man y un talento para la invencion, Vulture puede volar por los cielos en sus propios artilugios.');
INSERT INTO Carta VALUES(15,'Hawkeye',115,140,110,'Un tirador y luchador experto, Clint Barton hace un buen uso de su talento trabajando para S.H.I.E.L.D. como agente especial. El arquero conocido como Hawkeye tambien cuenta con una fuerte brujula moral que a veces lo desvia de sus ordenes directas. Clint Barton, amigo de Black Widow desde hace mucho tiempo, es el increible arquero de los Vengadores.');
INSERT INTO Carta VALUES(16,'Shang-Chi',175,190,135,'Shang-Chi busca la paz y la armonia en un mundo cansado mientras se opone a quienes lo derribarian.');
INSERT INTO Carta VALUES(17,'Black Panther',250,200,150,'TChalla es el rey de la nacion africana secreta y altamente avanzada de Wakanda, asi como el poderoso guerrero conocido como Black Panther.');
INSERT INTO Carta VALUES(18,'Wilson Fisk',260,140,115,'Wilson Fisk, uno de los criminales mas poderosos de la ciudad de Nueva York, gobierna el inframundo con una combinacion caracteristica de crueldad y encanto.');
INSERT INTO Carta VALUES(19,'Gamora',130,160,120,'Criada por Thanos para ser un arma viviente, Gamora busca la redencion como miembro de los Guardianes de la Galaxia, haciendo un buen uso de sus extraordinarias habilidades de lucha.');
INSERT INTO Carta VALUES(20,'Captain America',200,200,150,'Recipiente del suero de Super Soldado, el heroe de la Segunda Guerra Mundial Steve Rogers lucha por los ideales estadounidenses como uno de los heroes mas poderosos del mundo y el lider de los Vengadores.');
INSERT INTO Carta VALUES(21,'Green Goblin',170,145,120,'Norman Osborn es el fundador de Oscorp Technologies. Despues de experimentar en si mismo con una sustancia quimica inestable, Norman desarrollo una personalidad malvada alternativa conocida como Green Goblin.');
INSERT INTO Carta VALUES(22,'Star-Lord',160,140,110,'El lider de los Guardianes de la Galaxia, Peter Quill, conocido como Star-Lord, aporta un atrevido sentido del humor mientras protege al universo de todas y cada una de las amenazas.');
INSERT INTO Carta VALUES(23,'Electro',210,180,125,'Despues de un accidente de proporciones epicas, Max Dillon conocido como Electro, con sus nuevos poderes, centra su atencion en la venganza hacia la compania que lo descuido, Oscorp, y las personas que siente que lo traicionaron, incluido Spider-Man.');
INSERT INTO Carta VALUES(24,'Iron Man',205,230,170,'Genio. Multimillonario. Filantropo. La confianza de Tony Stark solo se compara con sus habilidades de alto vuelo como el heroe llamado Iron Man.');
INSERT INTO Carta VALUES(25,'Loki',255,175,140,'Loki, hermano de Thor, Principe de Asgard, legitimo heredero de Jotunheim y Dios del Engano, tiene la carga de un glorioso proposito. Su deseo de ser rey lo impulsa a sembrar el caos en Asgard. En su ansia de poder, extiende su alcance a la Tierra.');
INSERT INTO Carta VALUES(26,'Drax',230,210,130,'Drax es un guerrero brutal y probado en batalla. Ex criminal intergalactico, Drax no tiene ningun interes en el dinero, solo vengar a su familia asesinada. Reacio al principio, el heroe con una rabia asesina insaciable se une a los Guardianes de la Galaxia.');
INSERT INTO Carta VALUES(27,'Spider-Man',210,200,150,'Mordido por una arana radiactiva, las habilidades aracnidas de Peter Parker le otorgan poderes asombrosos que utiliza para ayudar a los demas, mientras que su vida personal sigue presentando muchos obstaculos.');
INSERT INTO Carta VALUES(28,'Sandman',315,185,200,'Flint Marko conocido como Sandman, atrapado en forma de arena, debe seguir la corriente para regresar a casa.');
INSERT INTO Carta VALUES(29,'Hulk',340,280,200,'Expuesto a fuertes dosis de radiacion gamma, el cientifico Bruce Banner se transforma en la maquina de ira verde y mezquina llamada Hulk. Asi pues, el Dr. Bruce Banner vive una vida atrapado entre el cientifico de voz suave que siempre ha sido y el monstruo verde incontrolable impulsado por su rabia.');
INSERT INTO Carta VALUES(30,'Doctor Strange',270,190,180,'El Doctor Stephen Strange, que en su dia fue un cirujano de gran exito, pero notablemente egoista, sufrio un terrible accidente que lo llevo a evolucionar de una manera que nunca podria haber previsto. El ahora se desempena como el Hechicero Supremo, el principal protector de la Tierra contra las amenazas magicas y misticas.');
INSERT INTO Carta VALUES(31,'Vision',335,270,210,'El androide llamado Vision desafia la fisica y lucha como un Vengador con el poder de la manipulacion de la densidad y su impecable cerebro informatico. Vision, un ser completamente unico, surgio gracias a una combinacion de vibranio de Wakanda, un rayo asgardiano, una Gema del Infinito, y mas.');
INSERT INTO Carta VALUES(32,'Ultron',280,260,190,'Concebido por Tony Stark para ser el reemplazo de los Vengadores, Ultron se convierte en uno de sus mayores villanos buscando acabar con la humanidad.');
INSERT INTO Carta VALUES(33,'Thor',310,300,185,'El hijo de Odin usa sus poderosas habilidades como Dios del Trueno para proteger su hogar Asgard y el planeta Tierra por igual.');
INSERT INTO Carta VALUES(34,'Captain Marvel',305,300,190,'Despues de que un dispositivo alienigena mutara su ADN, Danvers se transformo de una piloto simplemente brillante en uno de las superheroinas mas poderosas del universo. Ahora, volando entre las estrellas, Carol Danvers es conocida como la Capitana Marvel.');
INSERT INTO Carta VALUES(35,'Scarlet Witch',300,350,220,'Notablemente poderosa, Wanda Maximoff ha luchado contra y con los Vengadores, intentando perfeccionar sus habilidades y hacer lo que cree que es correcto para ayudar al mundo.');
INSERT INTO Carta VALUES(36,'Thanos',350,325,250,'El Titan Loco Thanos recorre el universo en busca de las Gemas del Infinito, con la intencion de usar su poder ilimitado para propositos impactantes. Thanos cree que puede salvar el universo eliminando a la mitad de su poblacion.');





