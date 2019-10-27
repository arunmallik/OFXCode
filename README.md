Steps to execute:
1. Clone the repo.
2. Build the repo.
3. Host on IIS.
4. Execute following REST apis using Postman.

APIs: (All the Apis uses HttpPost verb)
1. http://localhost:{port number}/api/BattleShip/GameBoard
   This Api doesn not accept any parameeters and will create a game board.
   It retuns a new game board otherise returns message saying board exist.

2. http://localhost:{port number}/api/BattleShip/AddShip
   This api takes an integer parameter that has to be passed as argument body. The parameter indicates length of the ship to be created.

3. http://localhost:{port number}/api/BattleShip/Attack
   This api takes two integer parametrs that has to be passed as argument body.
   These parameters indicated the attacked cell location (X, Y co-ordinate).
   Sample: {
	"attackx": 1,
	"attacky": 2
	   }
