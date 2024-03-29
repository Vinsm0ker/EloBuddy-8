namespace Kindred.SpellDB
{
    public enum SpellType
    {
        Line,
        Circular,
        Cone
    }

    public class SpellData
    {
        public string charName;
        public string spellKey;
        public int dangerlevel = 1;
        public string spellName;
        public string name;
        public float range;
        public float radius;
        public float projectileSpeed = float.MaxValue;
        public string missileName = "";
        public SpellType spellType;
        public float spellDelay = 250;
        public bool fixedRange = false;
        public float angle;
        public float sideRadius;
        public int splits;
        public bool usePackets = false;
        public float extraDelay = 0;
        public float extraDistance = 0;
        public bool isThreeWay = false;
        public bool defaultOff = false;
        public bool noProcess = false;
        public bool isWall = false;
        public float extraEndTime = 0;
        public bool hasEndExplosion = false;
        public bool isSpecial = false;
        public string[] extraSpellNames;
        public string[] extraMissileNames;

        public SpellData()
        {

        }

        public SpellData(
            string charName,
            string spellName,
            string name,
            int range,
            int radius,
            int dangerlevel,
            SpellType spellType
            )
        {
            this.charName = charName;
            this.spellName = spellName;
            this.name = name;
            this.range = range;
            this.radius = radius;
            this.dangerlevel = dangerlevel;
            this.spellType = spellType;
        }
    }
}
