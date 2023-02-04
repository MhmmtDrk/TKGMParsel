using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKGMParsel.Data.Entities
{
    public  class Parcel
    {
        [Key]
        public int Id { get; set; }
        public string? ilceAd { get; set; }
        public string? mevkii { get; set; }

        public int? ilId { get; set; }
        public string? durum { get; set; }
        public int? parselId { get; set; }
        public string? zeminKmdurum { get; set; }
        public int? zeminId { get; set; }
        public string? parselNo { get; set; }
        public string? nitelik { get; set; }
        public string? mahalleAd { get; set; }
        public string? gittigiParselListe { get; set; }
        public string? gittigiParselSebep { get; set; }
        public string? alan { get; set; }
        public string? adaNo { get; set; }
        public int? ilceId { get; set; }
        public string? ilAd { get; set; }
        public int? mahalleId { get; set; }
        public string? pafta { get; set; }
    }
}
