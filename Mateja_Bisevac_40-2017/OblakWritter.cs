using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DEVSsharp;

namespace Mateja_Bisevac_40_2017
{
    class OblakWritter : Atomic
    {
        public List<InputPort> in_oblaci;


        StreamWriter sw;
        OblakZaStampanje obi;
        public OblakWritter(string name, TimeUnit tu, int br_oblaka) : base(name,tu)
        {
            in_oblaci = new List<InputPort>();
            for (int i = 0; i < br_oblaka; i++)
            {
              InputPort  in_oblak = AddIP("input_oblak_"+i);
                in_oblaci.Add(in_oblak);
            }
        }

        public override bool delta_x(PortValue x)
        {
            for(int i = 0; i< in_oblaci.Count; i++)
            {
                if(in_oblaci[i] == x.port)
                {
                    obi = (OblakZaStampanje)x.value;
                    sw.WriteLine(TimeCurrent + "," + i + "," + obi.zapremina + "," + obi.koordinate[0].X + "," + obi.koordinate[0].Y + "," + obi.koordinate[1].X + "," + obi.koordinate[1].Y + "," + obi.koordinate[2].X + "," + obi.koordinate[2].Y + "," + obi.koordinate[3].X+"," + obi.koordinate[3].Y);
                    sw.Flush();
                }
            }
            return false;
        }

        public override void delta_y(ref PortValue y)
        {
           // throw new NotImplementedException();
        }

        public override void init()
        {
            if (sw != null) sw.Close();
            sw = new StreamWriter("IzlazOblak.csv");
            sw.WriteLine("Time(H),Oblak_ID,V oblaka(m^3), A.x,A.y , B.x , B.y , C.x , C.y , D.x , D.y");// A B C D su redom koordinate tacaka oblaka
            sw.Flush();
        }

        public override double tau()
        {
            return Double.MaxValue;
        }
    }
}
