using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Utility
{
    public class City
    {
        public Byte ID { get; set; }
        public String Name { get; set; }


        private static List<City> _cities;

        public static List<City> All()
        {
            if(_cities == null)
            {
                _cities = new List<City>();
                _cities.Add(new City() { ID = 1, Name = "基隆市" });
                _cities.Add(new City() { ID = 2, Name = "台北市" });
                _cities.Add(new City() { ID = 3, Name = "台北縣" });
                _cities.Add(new City() { ID = 4, Name = "桃園縣" });
                _cities.Add(new City() { ID = 5, Name = "新竹市" });
                _cities.Add(new City() { ID = 6, Name = "新竹縣" });
                _cities.Add(new City() { ID = 7, Name = "苗栗縣" });
                _cities.Add(new City() { ID = 8, Name = "台中市" });
                _cities.Add(new City() { ID = 9, Name = "台中縣" });
                _cities.Add(new City() { ID = 10, Name = "彰化縣" });
                _cities.Add(new City() { ID = 11, Name = "南投縣" });
                _cities.Add(new City() { ID = 12, Name = "雲林縣" });
                _cities.Add(new City() { ID = 13, Name = "嘉義市" });
                _cities.Add(new City() { ID = 14, Name = "嘉義縣" });
                _cities.Add(new City() { ID = 15, Name = "台南市" });
                _cities.Add(new City() { ID = 16, Name = "台南縣" });
                _cities.Add(new City() { ID = 17, Name = "高雄市" });
                _cities.Add(new City() { ID = 18, Name = "高雄縣" });
                _cities.Add(new City() { ID = 19, Name = "屏東縣" });
                _cities.Add(new City() { ID = 20, Name = "台東縣" });
                _cities.Add(new City() { ID = 21, Name = "花蓮縣" });
                _cities.Add(new City() { ID = 22, Name = "宜蘭縣" });
                _cities.Add(new City() { ID = 23, Name = "澎湖縣" });
                _cities.Add(new City() { ID = 23, Name = "金門縣" });
                _cities.Add(new City() { ID = 23, Name = "連江縣" });
            }

            return _cities;
        }
    }
}
