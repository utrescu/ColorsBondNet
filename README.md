# Fer servir Microsoft Bond

Bond és un framework per treballar amb dades estructurades de codi obert i multiplataforma.
Ofereix serialització i una forma eficient de manipulació de dades. (Diuen que ho fan servir molt internament)

S'assembla a Google ProtocolBuffers.

Fins ara havia vist en algun projecte en el que havia col·laborat l'increment de rendiment que donava fer servir un format binari com 'protocolbuffers' sobre json.

Microsoft té el codi font de **Microsoft Bond** a [GitHub](https://github.com/Microsoft/bond)

- Més informació:
  - [Article de Toptal](https://www.toptal.com/microsoft/meet-bond-microsoft-bond)
  - [A Thorough Guide to Bond for C#](https://microsoft.github.io/bond/manual/bond_cs.html#deserializer)

Microsoft Bond permet fer servir les dades en formats binaris (compact, fast, simple) o de text (json, XML)

## Projectes

En el repositori hi ha diversos projectes (no sóc gaire original amb els noms):

| Projecte  |                                                                    |
| --------- | ------------------------------------------------------------------ |
| 007       | Llibreria que conté l'esquema de dades                             |
| basic     | Projecte senzill que implementa l'exemple bàsic de la documentació |
| james     | Servidor REST que interactua amb la base de dades                  |
| jamesbond | Client del servidor 'james' per poder veure les dades binaries     |

### 007

És un projecte que es limita a definir el tipus de dades que es transmetrà. Les dades es defineixen amb el seu propi llenguatge d'esquemes que permet definir les dades de forma precisa:

    struct AboutNothing
    {
        0: uint16 n = nothing;
        1: string name = nothing;
        2: list<float> floats = nothing;
    }

Aquests esquemes (normalment amb extensió .bond) s'han de compilar amb el compilador de Bond (gbc) que en Windows té el seu propi paquet C#, però en linux s'ha de compilar per generar les classes del llenguatge

    gbc c# fitxer.bond

Però en general els projectes ja ho faran automàticament quan es compili el projecte amb 'dotnet build' o 'dotnet run'

#### gbc

En Fedora, les instruccions d'Ubuntu ja estan en el README, s'han d'instal·lar els requeriments:

    dnf install clang cmake zlib-dev boost-dev boost-thread-dev
    curl -sSL https://get.haskellstack.org/ | sh

Clonar el repositori i generar l'executable:

    git clone https://github.com/Microsoft/bond
    cd bond/compiler
    stack build

Al acabar tindrem l'executable que, pel que he vist, s'ha de copiar a **~/.nuget/packages/bond.csharp/8.0.0/build/../tools/** com a gbc.exe perquè s'integri automàticament en la creació dels projectes.

### Basic

Es tracta de l'exemple de la pàgina web adaptat a la meva estructura de dades

    cd base
    dotnet run

### James

En el projecte 'james' que fa de servidor REST en comptes d'instal·lar els paquests de 'Bond.AspNetCore.Mvc.Formatters' me'ls he descarregat i adaptat lleugerament per evitar els errors. Simplement per veure com es fa per modificar la sortida d'un Webapi. Era innecessari perquè podia haver instal·lat el paquet ...

    dotnet add package Bond.AspNetCore.Mvc.Formatters

#### Base de dades

Per funcionar he implementat una base de dades amb els colors en català de la Wikipèdia. Aquí hi ha un backup de la base de dades en el directori 'database'

Per tant el primer és iniciar MySQL, crear-hi la base de dades, i importar-hi les dades.

    mysql -u root -p
    Password:
    MySQL [(none)]> CREATE DATABASE colors;
    MYSQL [(none)]> GRANT ALL PRIVILEGES ON colors.* TO 'colors'@localhost' IDENTIFIED BY 'colors';
    MYSQL [(none)]> exit
    Bye
    $ mysql -u colors -p colors < colors.sql
    Password:

![Comprovacio](mysql.svg)

#### El programa

Bàsicament és un servei REST que té dos mètodes GET, un per Id i un per Nom, que cerca els resultats a la base de dades, i que retorna els resultats en binari

| Mètode | URL             | Resultat                                |
| ------ | --------------- | --------------------------------------- |
| GET    | /colors/1       | Mostra el color que té el número 1 d'ID |
| GET    | /colors/vermell | Mostra el color que té de nom "vermell" |

Si s'interroga el servei el resultat no és JSON sinó que és Bond Binary Compact. Amb httpie no es poden veure els resultats:

    $ http localhost:5000/colors/10
    HTTP/1.1 200 OK
    Content-Type: application/vnd.microsoft.bond+compactbinary
    Date: Thu, 29 Aug 2018 22:18:49 GMT
    Server: Kestrel
    Transfer-Encoding: chunked

    +-----------------------------------------+
    | NOTE: binary data not shown in terminal |
    +-----------------------------------------+

Però es pot capturar el trànsit amb Wireshark...

![GET color](bond.png)

Amb Curl si que es pot treure el resultat per pantalla:

    curl localhost:5000/colors/Vermell --output -
        Vermell)#FF0000

Un altre exemple:

    curl localhost:5000/colors/1 --output -

            Aiguamarina)#7FFFD4

### Jamesbond

Com que no es pot veure el resultat retornat perquè és binari, he desenvolupat un client per interactuar-hi...

Bàsicament s'executa passant-li com a paràmetre el nom del color que es vol cercar i mostrarà per pantalla el que retorna el servei 'James'.

Primer s'inicia el servei 'james':

    cd james
    dotnet run james

I després en un altre terminal s'executa el programa:

    $ dotnet run
    S'ha d'entrar el color a cercar o l'ID.
    Ex. Programa vermell o Programa 12

    $ dotnet run vermell
    Result OK
    Vermell -> #FF0000

    $ dotnet run 12
    Result OK
    Blau cel -> #77B5FE

    $ dotnet run patata
    Result NotFound
    Error Color 'patata' no trobat

![jamesbond en marxa](jamesbond.svg)
