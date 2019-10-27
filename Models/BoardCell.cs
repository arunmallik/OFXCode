using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OFxG.Models
{
    public class BoardCell
    {
        public int x { get; set; }
        public int y { get; set; }
        public bool occupied { get; set; }
        public string ship_name { get; set; }
        public cellstate cell_state { get; set; }
    }
    public enum cellstate
    {
        hit = 0,
        miss = 1,
        sunk,
        empty,
        fill
    }

    public class Ship
    {
        public string shipname { get; set; }
        public int origLength { get; set; }
        public int hitCount { get; set; }
        List<BoardCell> coordinates { get; set; }
    }
}