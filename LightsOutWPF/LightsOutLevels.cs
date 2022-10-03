using System.Collections.Generic;

namespace LightsOutWPF
{
    public class LightsOutLevels
    {
        public string Name { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public List<int> On { get; set; }
    }
}
