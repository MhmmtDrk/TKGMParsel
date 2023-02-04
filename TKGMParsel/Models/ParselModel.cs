namespace TKGMParsel.Models
{
    public class ParselModel
    {
        public Feature[] features { get; set; }
        public string type { get; set; }
        public Crs crs { get; set; }

    }
    public class Crs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties1 properties { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public object[][][] coordinates { get; set; }
    }

    public class Properties1
    {
        public string text { get; set; }
        public int id { get; set; }
    }
}





///////////////////////////////////
///



