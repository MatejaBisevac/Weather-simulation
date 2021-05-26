using System;
using System.Collections.Generic;
using System.Text;
using DEVSsharp;

namespace Mateja_Bisevac_40_2017
{
    class HRU : Atomic
    {
        //PORTS
        public List<InputPort> in_kise;
        public InputPort in_blokadaQwp;
        public OutputPort out_prelivanje;
        public OutputPort out_protok;
        public InputPort in_prelivanje;
        //




        List<Vector2D> koordinate;
        double Ph;
        double Qwp; // 
        double Hmax; // max visina vode koju prima;
        double H; //nivo vode
        double B; // povrsina;
        double V;
        double Epsh;
        bool blokiranQwp = false;
        public double nagib;
        public HRU(string name, TimeUnit tu, List<Vector2D> kord, double Ph, double Epsh, double Qwp, double nagib, int broj_oblaka) : base(name, tu)
        {
            //
            in_kise = new List<InputPort>();
            for(int i = 0; i < broj_oblaka; i++)
            {
                InputPort in_kisa = new InputPort("in_kisa");
                in_kise.Add(in_kisa);
            }


            H = 0;
            this.Qwp = Qwp;
            this.nagib = nagib;

            koordinate = kord;
            out_prelivanje = AddOP("out_prelivanje");
            out_protok = AddOP("out_protok");
            in_blokadaQwp = AddIP("in_blokadaQwp");
            
            in_prelivanje = AddIP("in_prelivanje");

            B = (koordinate[1].X - koordinate[0].X) * (koordinate[3].Y - koordinate[0].Y);
            this.Epsh = Epsh;
            V = B * H * Ph; // poroznost utice na kolicinu vode koju moze da primi 
        }


        public override bool delta_x(PortValue x)
        {
           

            double povrsinaPadavine = 0.0;
            if (x.port == in_blokadaQwp)
            {
                blokiranQwp = true;
                return true;
            }
            for (int i = 0; i < in_kise.Count; i++)
            {
                if (x.port == in_kise[i])
                {
                    Kisa padavine = (Kisa)x.value;
                    
                    if (koordinate[0].levoDole(padavine.koordinate[0]) && koordinate[3].levoGore(padavine.koordinate[3]) && koordinate[2].desnoGore(padavine.koordinate[2]) && koordinate[1].desnoDole(padavine.koordinate[1]))
                        povrsinaPadavine = B;
                    else if (padavine.koordinate[0].X > koordinate[1].X || padavine.koordinate[1].X < koordinate[0].X || padavine.koordinate[3].Y < koordinate[0].Y || padavine.koordinate[0].Y > koordinate[3].Y)
                        povrsinaPadavine = 0;
                    else if (koordinate[3].levoGore(padavine.koordinate[3]) && koordinate[3].desnoDole(padavine.koordinate[1]) && koordinate[1].levoGore(padavine.koordinate[1]))
                    {
                        povrsinaPadavine = (padavine.koordinate[1].X - koordinate[0].X) * (koordinate[3].Y - padavine.koordinate[1].Y);
                    }
                    else if (koordinate[2].levoDole(padavine.koordinate[0]) && koordinate[0].desnoGore(padavine.koordinate[0]) && koordinate[2].desnoGore(padavine.koordinate[2]))
                    {
                        povrsinaPadavine = (koordinate[1].X - padavine.koordinate[0].X) * (koordinate[2].Y - padavine.koordinate[0].Y);
                    }
                    else if (koordinate[0].desnoDole(padavine.koordinate[2]) && koordinate[0].levoDole(padavine.koordinate[0]) && koordinate[2].levoDole(padavine.koordinate[2]))
                    {
                        povrsinaPadavine = (padavine.koordinate[2].X - koordinate[0].X) * (padavine.koordinate[2].Y - koordinate[0].Y);
                    }
                    else if (padavine.koordinate[3].desnoDole(koordinate[1]) && padavine.koordinate[3].levoGore(koordinate[3]) && koordinate[1].desnoDole(padavine.koordinate[1]))
                    {
                        povrsinaPadavine = (koordinate[1].X - padavine.koordinate[3].X) * (padavine.koordinate[3].Y - koordinate[0].X);
                    }
                    else if (koordinate[3].desnoDole(padavine.koordinate[3]) && koordinate[1].levoGore(padavine.koordinate[1]))
                    {
                        povrsinaPadavine = (padavine.koordinate[1].X - padavine.koordinate[0].X) * (padavine.koordinate[3].Y - padavine.koordinate[0].Y);
                    }
                    else if (koordinate[3].levoGore(padavine.koordinate[3]) && koordinate[0].desnoDole(padavine.koordinate[1]) && koordinate[1].levoDole(padavine.koordinate[1]))
                    {
                        povrsinaPadavine = (padavine.koordinate[1].X - koordinate[0].X) * (koordinate[3].Y - koordinate[0].Y);
                    }
                    else if (koordinate[2].levoGore(padavine.koordinate[3]) && koordinate[1].desnoDole(padavine.koordinate[1]) && koordinate[3].desnoGore(padavine.koordinate[3]))
                    {
                        povrsinaPadavine = (koordinate[1].X - padavine.koordinate[3].X) * (koordinate[3].Y - koordinate[0].Y);
                    }
                    else if (koordinate[3].levoGore(padavine.koordinate[3]) && koordinate[1].desnoDole(padavine.koordinate[1]) && padavine.koordinate[3].desnoGore(koordinate[3]))
                    {
                        povrsinaPadavine = (koordinate[1].X - koordinate[0].X) * (padavine.koordinate[3].Y - koordinate[0].Y);
                    }
                       



                    //todo
                    double Hold = H;
                    H += padavine.kolicina * (povrsinaPadavine / B) / 1000; // kroz 1000 jer prelazimo iz mm u metre
                    Hold = Hold / H;
                    Qwp = H*Ph; // Qwp se proporcionalno menja u odnosu na promenu visine rezervoara
                                      // Qwp se proporcionalno menja u odnosu na promenu visine rezervoara
                    return true;
                }
            }
            if(x.port == in_prelivanje)
            {
                double Hold = H;
                H += (double)x.value;
                Hold = Hold / H;
                Qwp = Qwp * Hold;
                return true;
            }
            return false;
        }

        public override void delta_y(ref PortValue y)
        {   
            double Hold = H;
            if (blokiranQwp == false)
            {
                H -= Qwp * TimeElapsed;
                //Hold = Hold / H;
                Qwp = Ph*H;
                y = new PortValue(out_protok, Qwp);
            }
            else
            {
                if(H>Hmax)
                {
                    y = new PortValue(out_prelivanje, H - Hmax);
                    H = Hmax;
                }
            }
       }

        public override void init()
        {
         //   throw new NotImplementedException();
        }

        public override double tau()
        {
            double t = 0.0;
            if (H <= 0) // sva voda otekla i ceka kisu 
            {
                H = 0;
                return Double.MaxValue;
            }
            else if(Qwp>=0 && Qwp <= 0.0001)
            {
                return Double.MaxValue;
            }
            else
            {
                t = Epsh / Qwp;
            }
            return t;
        }
        public override string Get_s()
        {
            return "visina H : " + H;
        }
    }
}
