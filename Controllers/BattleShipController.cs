using OFxG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OFxG.Controllers
{
    public class BattleShipController : ApiController
    {
        public static BattleShipBoard battleshipBoard = null;

        public BattleShipController()
        {
        }

        [HttpPost]
        [ActionName("GameBoard")]
        public string GameBoard()
        {
            if (battleshipBoard == null)
            {
                battleshipBoard = BattleShipBoard.BoardInstance;

            }
            else return "Board already exist.";

            return "Board created.";
        }

        [HttpPost]
        [ActionName("AddShip")]
        //[Route("api/BattleShip/AddShip")]
        public string AddShip([FromBody]int shipLength)
        {
            string result = string.Empty;
            if (battleshipBoard == null)
            {
                result = "Board not created";
            }

            int[] alreadyTraversed = new int[10];
            for (int i = 0; i < 10; i++)
            {
                alreadyTraversed[i] = -1;
            }
            bool recurse = true;

            while (recurse)
            {
                Random coordinate = new Random();
                int lookupindex = coordinate.Next(0, 9);
                if (!alreadyTraversed.Contains(-1))
                {
                    recurse = false;
                }
                else if (alreadyTraversed[lookupindex] == -1)
                {
                    alreadyTraversed[lookupindex] = 1;
                    List<BoardCell> cellsRow = battleshipBoard.cells.Where(index => index.x == lookupindex).ToList();
                    List<BoardCell> freeCells = new List<BoardCell>();
                    bool horizontalLookUp = false;
                    bool verticalLookUp = false;
                    foreach (BoardCell c in cellsRow)
                    {
                        if (freeCells.Count == shipLength)
                        {
                            horizontalLookUp = true;
                            break;
                        }
                        if (!c.occupied)
                        {
                            freeCells.Add(c);
                        }
                        else
                        {
                            if (freeCells.Count > 0) freeCells.Clear();
                            if (shipLength > (10 - (c.y + 1))) break;
                        }
                    }
                    if (freeCells.Count == shipLength)
                        horizontalLookUp = true;

                    if (!horizontalLookUp)
                    {
                        cellsRow = battleshipBoard.cells.Where(index => index.y == lookupindex).ToList();
                        foreach (BoardCell c in cellsRow)
                        {
                            if (freeCells.Count == shipLength)
                            {
                                verticalLookUp = true;
                                break;
                            }
                            if (!c.occupied)
                            {
                                freeCells.Add(c);
                            }
                            else
                            {
                                if (freeCells.Count > 0) freeCells.Clear();
                                if (shipLength > (10 - (c.x + 1))) break;
                            }

                        }
                    }
                    if (freeCells.Count == shipLength)
                        verticalLookUp = true;

                    if (horizontalLookUp || verticalLookUp)
                    {
                        foreach (BoardCell cell in freeCells)
                        {
                            cell.occupied = true;
                            cell.ship_name = "Ship_" + freeCells[0].x.ToString() + freeCells[0].y.ToString() + freeCells[shipLength - 1].y.ToString();
                            cell.cell_state = cellstate.fill;
                        }
                        result = "Ship added";
                        recurse = false;
                    }
                }
            }
            if (!recurse)
               result = "No space";
            return result;
        }

        

        [HttpPost]
        [ActionName("Attack")]
        //[Route("api/BattleShip/Attack/{attackx}/{attacky}")]
        //[Route("Ship/{shipLength}")]
        public string Attack([FromBody]Data data)
        {
            int attackx = data.attackx; int attacky = data.attacky;
            string attackresult = string.Empty;
            BoardCell cellAttacked = battleshipBoard.cells.Where(index => index.x == attackx && index.y == attacky).First();
            if (cellAttacked.cell_state != cellstate.hit)
            {
                if (cellAttacked.occupied)
                {
                    attackresult = "hit";
                    cellAttacked.cell_state = cellstate.hit;
                }
                else
                {
                    attackresult = "miss";
                }
            }

            // set the values after complete ship has sunk
            string attackedShip = cellAttacked.ship_name;
            List<BoardCell> attackedShipCells = battleshipBoard.cells
                .Where(cell_shipname => cell_shipname.ship_name == attackedShip).ToList();
            int shipCellHitCount = attackedShipCells.Where(hit => hit.cell_state == cellstate.hit).Count();
            if (attackedShipCells.Count == shipCellHitCount)
            {
                foreach (BoardCell cellItem in attackedShipCells)
                {
                    cellItem.occupied = false;
                    cellItem.ship_name = string.Empty;
                    cellItem.cell_state = cellstate.empty;
                }
            }

            return attackresult;
        }
    }

    public class Data
    {
        public int attackx { get; set; }
        public int attacky { get; set; }
    }
    public sealed class BattleShipBoard
    {
        private static BattleShipBoard boardInstance = null;
        public List<BoardCell> cells = new List<BoardCell>();
        private BattleShipBoard()
        {
        }

        public static BattleShipBoard BoardInstance
        {
            get
            {
                if (boardInstance == null)
                {
                    boardInstance = new BattleShipBoard();
                    for (int i = 0; i < 10; i++)
                        for (int j = 0; j < 10; j++)
                        {
                            {
                                BoardCell board = new BoardCell();
                                board.x = i; board.y = j;
                                board.occupied = false;
                                board.ship_name = string.Empty;
                                board.cell_state = cellstate.empty;
                                boardInstance.cells.Add(board);
                            }
                        }
                }
                return boardInstance;
            }
        }
    }
}
