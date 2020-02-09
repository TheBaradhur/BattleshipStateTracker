# BattleshipStateTracker
----
## Description

This micro service is in charge of handling the state of a battleship game, from start to completion, for a single player. 
It does not include persistence, so everything is reset when you relaunch the app or start a new game.

The game finishes when an attack hits the last surviving ship and sink it.

----
## Getting Started

Clone the repo, open the project in visual studio and run it.

----
## Available endpoints
### GameState 
    GET api/gamestate/

    POST api/gamestate/new-game

    POST api/gamestate/add-ship

    POST api/gamestate/attack

----
## Manual Tests with Postman
For ease of testing, a postman collection is available for import in folder "ManualTests". It contains all the endpoints in a collection with valid examples. 

To use it, open Postman and import the collection. Then update the localhost port based on how the selected launch port.