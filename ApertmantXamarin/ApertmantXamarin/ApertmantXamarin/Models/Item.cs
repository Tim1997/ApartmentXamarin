using System;
using System.ComponentModel;

namespace ApertmantXamarin.Models
{
    public enum City
    {
        [Description("Bà rịa")]
        BaRia,
        [Description("Bạc Liêu")]
        BacLieu,
        [Description("Bắc Ninh")]
        BacNinh,
        [Description("Cà Mau")]
        CaMau,
        [Description("Đà Lạt")]
        DaLat,
        [Description("Huế")]
        Hue,
        [Description("Hà Nội")]
        HaNoi,
        [Description("Lào Cai")]
        LaoCai,
        [Description("Mỹ Tho")]
        MyTho,
        [Description("Yên Bái")]
        YenBai,
        [Description("Vũng Tàu")]
        VungTau,
        [Description("Vinh")]
        Vinh,
        [Description("Việt Trì")]
        VietTri,
        [Description("Hồ Chí Minh")]
        HoChiMinh
    }
    public class Item
    {
        public string Username { get; set; }
        public string Phone { get; set; }
        public City? City { get; set; }
        public int RoomNumber { get; set; }
        public DateTime DateHire { get; set; }
        public TimeSpan TimeHire { get; set; }
        public string Note { get; set; }
        public double Price { get; set; }
    }
}