using System;
using System.Collections.Generic;
using System.Text;
using DEVSsharp;

namespace Mateja_Bisevac_40_2017
{
    class Oblak : Atomic
    {
        //PORTS
        public InputPort in_temperature;
        public OutputPort out_kisa;
        public OutputPort out_stampanje;

       //



        Vector2D vetar;
        double h; // visina oblaka
        List<Vector2D> koordinate;
        bool padaKisa=false;
        double P; // mm/h
        double Kv; // ??
        double Kz; // ??
        double B; // metrima^2
        double Eps0; // u metrima
        double V; // zapremina u metrima^3
        double Speed; // brzina oblaka metri po satu;

        public Oblak(string name, TimeUnit tu,Vector2D vetar, double h , List<Vector2D> koords, double P, double Kv, double Kz, double Eps0) : base(name, tu)
        {
            koordinate = new List<Vector2D>();
            in_temperature = AddIP("in_temperature");
            out_stampanje = AddOP("out_stampanje");
            out_kisa = AddOP("out_kisa");
            foreach(var item in koords)
            {
                koordinate.Add(new Vector2D(item.X, item.Y));
            }


            koordinate = koords;
            this.h = h;
            this.vetar = vetar;
            this.P = P; // kolko padne kise za jednu sekundu
            this.Kv = Kv;
            this.Kz = Kz;
            this.Eps0 = Eps0;
            B = (koordinate[1].X - koordinate[0].X) * (koordinate[3].Y - koordinate[0].Y);
            V = B * h;
            Speed = Kv / V;

        }
        public override bool delta_x(PortValue x)
        {
            if(x.port == in_temperature)
            {
                padaKisa = (bool)x.value;
                return true;
            }
            return false; 

        }

        public override void delta_y(ref PortValue y)
        {
            // pomeranje oblaka sa preciznoscu Eps0
            for (int i = 0; i < koordinate.Count; i++)
            {
                koordinate[i] += vetar;
            }
            if (padaKisa == true)
            {
                h -= Kz * P * TimeElapsed;
                Kisa padavine = new Kisa(koordinate, P * TimeElapsed);
                y = new PortValue(out_kisa, padavine);

                if (h <= 0.001)
                {
                    padaKisa = false; // ispraznjen oblak 
                }
                h = 0.00001;  
            }
            V = B * h;
            Speed = Kv / V;
            OblakZaStampanje ob = new OblakZaStampanje(this.koordinate, this.V);
            y = new PortValue(out_stampanje, ob);


         }

        public override void init()
        {
           // throw new NotImplementedException();
        }

        public override double tau()
        {
            if (h < 0.01)
                return double.MaxValue;
            double t = 3.0;
           t = Eps0 / Speed;
            //t = 5;
            return t;
        }
        public override string Get_s()
        {
            return padaKisa.ToString();
        }
    }
}
