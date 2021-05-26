using System;
using System.Collections.Generic;
using System.Text;

namespace Mateja_Bisevac_40_2017
{
    class OblakZaStampanje
    {
        public List<Vector2D> koordinate;
        public double zapremina;
        public OblakZaStampanje(List<Vector2D> koord, double zapremina)
        {
            this.koordinate = koord;
            this.zapremina = zapremina;
        }


    }
}
