namespace TKGMParsel.Models
{
    public class ParselDataModel
    {
        public string type { get; set; }
        public GeometryParsel geometry { get; set; }
        public PropertiesParsel properties { get; set; }
    }
    public class GeometryParsel
    {
        public string type { get; set; }
        public float[][][] coordinates { get; set; }
    }

    public class PropertiesParsel
    {

        public string ilceAd { get; set; }
        public string mevkii { get; set; }

        public int ilId { get; set; }
        public string durum { get; set; }
        public int parselId { get; set; }
        public string zeminKmdurum { get; set; }
        public int zeminId { get; set; }
        public string parselNo { get; set; }
        public string nitelik { get; set; }
        public string mahalleAd { get; set; }
        public string gittigiParselListe { get; set; }
        public string gittigiParselSebep { get; set; }
        public string alan { get; set; }
        public string adaNo { get; set; }
        public int ilceId { get; set; }
        public string ilAd { get; set; }
        public int mahalleId { get; set; }
        public string pafta { get; set; }
    }


}
