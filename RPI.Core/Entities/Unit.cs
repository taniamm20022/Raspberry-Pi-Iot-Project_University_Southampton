using System;
using System.Collections.Generic;


namespace RPI.Core.Entities
{
    public class Unit
    {
        public Unit()
        {
            this.Sensors = new HashSet<Sensor>();
        }

        public int UnitId { get; set; }
        public string Name { get; set; }

        public ICollection<Sensor> Sensors { get; set; }
    }
}

