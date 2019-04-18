using System;
using System.Collections.Generic;

namespace Landfall.TABS
{
    class patch_LandfallUnitDatabase : LandfallUnitDatabase
    {
        private List<UnitBlueprint> Units; // Expose private field
        public List<UnitBlueprint> GetAllUnits() // Make accessing it easier
        {
            return this.Units;
        }

        private List<MapAsset> Maps;
        public List<MapAsset> GetAllMaps() // The same as above
        {
            return this.Maps;
        }
    }
}
