using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TemBot
{
    public static class Temtems
    {
        static Dictionary<Int64, TemtemInfo> _allTemtems;
        public static Dictionary<Int64, TemtemInfo> AllTemtems
        {
            get
            {
                if (_allTemtems == null)
                {
                    var temtems = JsonSerializer.Deserialize<List<TemtemInfo>>(Properties.Resources.Temtems);
                    _allTemtems = new Dictionary<Int64, TemtemInfo>();
                    foreach (var temtem in temtems)
                    {
                        _allTemtems.Add(temtem.id, temtem);
                    }
                }
                return _allTemtems;
            }
        }
    }
    public class Passive
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class TemtemInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int baseAttack { get; set; }
        public int baseDefense { get; set; }
        public int baseHp { get; set; }
        public int baseSpAttack { get; set; }
        public int baseSpDefense { get; set; }
        public int baseSpeed { get; set; }
        public int baseStamina { get; set; }
        public string elementalType1 { get; set; }
        public string elementalType2 { get; set; }
        public double weight { get; set; }
        public int height { get; set; }
        public int catchRate { get; set; }
        public int defeatedExperienceGroup { get; set; }
        public bool hasGender { get; set; }
        public int maleRate { get; set; }
        public bool publicTempedia { get; set; }
        public bool lumaAvailable { get; set; }
        public int tvAttack { get; set; }
        public int tvDefense { get; set; }
        public int tvHp { get; set; }
        public int tvSpAttack { get; set; }
        public int tvSpDefense { get; set; }
        public int tvSpeed { get; set; }
        public int tvStamina { get; set; }
        public List<Passive> passives { get; set; }
        public List<object> techniques { get; set; }
        public List<object> coursesTechniques { get; set; }
        public List<object> eggTechniques { get; set; }
        public List<object> evolutions { get; set; }
    }
}
