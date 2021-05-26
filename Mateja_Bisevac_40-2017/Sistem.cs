using System;
using System.Collections.Generic;
using System.Text;
using DEVSsharp;

namespace Mateja_Bisevac_40_2017
{

    class Sistem : Coupled
    {
        public double Tk;
        public OutputPort out_keyboardTemp;
        public Sistem(string name, double Tk) : base(name)
        {
            Vector2D Sistem_vetar = new Vector2D(1, 1);
            this.Tk = Tk;// ako padne temp ispod 10 pocinje da pada kisa
            out_keyboardTemp = AddOP("output_keyboard");    


            List<Vector2D> koordinateHru = new List<Vector2D>();
            koordinateHru.Add(new Vector2D(5, 10));
            koordinateHru.Add(new Vector2D(10, 10));
            koordinateHru.Add(new Vector2D(10, 15));
            koordinateHru.Add(new Vector2D(5, 15));

            HRU h1 = new HRU("hru1", TimeUnit.Sec, koordinateHru, 0.925, 1, 5, 3,3);
            koordinateHru.Clear();


            koordinateHru.Add(new Vector2D(10, 10));
            koordinateHru.Add(new Vector2D(15, 10));
            koordinateHru.Add(new Vector2D(15, 15));
            koordinateHru.Add(new Vector2D(10, 15));

            HRU h2 = new HRU("hru2", TimeUnit.Sec, koordinateHru, 1.325, 1, 5, 2,3);
            koordinateHru.Clear();
            
            koordinateHru.Add(new Vector2D(5, 5));
            koordinateHru.Add(new Vector2D(10, 5));
            koordinateHru.Add(new Vector2D(10, 10));
            koordinateHru.Add(new Vector2D(5, 10));

            HRU h3 = new HRU("hru3", TimeUnit.Sec, koordinateHru, 0.125, 1, 5, 2,3);
            koordinateHru.Clear();

            koordinateHru.Add(new Vector2D(10, 5));
            koordinateHru.Add(new Vector2D(15, 5));
            koordinateHru.Add(new Vector2D(15, 10));
            koordinateHru.Add(new Vector2D(10, 10));

            HRU h4 = new HRU("hru4", TimeUnit.Sec, koordinateHru, 0.325, 1, 5, 1,3);
            koordinateHru.Clear();

            this.AddModel(h1);
            this.AddModel(h2);
            this.AddModel(h3);
            this.AddModel(h4);

            List<List<HRU>> hrus;
            hrus = new List<List<HRU>>();
            hrus.Add(new List<HRU>());
            hrus.Add(new List<HRU>());

            hrus[0].Add(h1);
            hrus[0].Add(h2);
            hrus[1].Add(h3);
            hrus[1].Add(h4);
            Teren t = new Teren(hrus);
        

            //CP prvaljenje CP gde je najveci nagib izmedju 2 bloka
            for (int i = 0; i < t.HRUs.Count; i++)
            {
                for(int j = 0; j < t.HRUs[i].Count; j++)
                {
                    if(i==0 && j==0)
                    {
                        if (t.HRUs[i + 1][j].nagib >= t.HRUs[i][j + 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i + 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else
                                this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i + 1][j].in_prelivanje);
                        else
                        {
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j + 1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else
                                this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j + 1].in_prelivanje);
                        }
                    }
                    else if(i==0 && j!=0 && j!= t.HRUs[i].Count-1)
                    {
                        if (t.HRUs[i + 1][j].nagib >= t.HRUs[i][j + 1].nagib && t.HRUs[i + 1][j].nagib >= t.HRUs[i][j - 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i + 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i + 1][j].in_prelivanje);
                        else if (t.HRUs[i][j + 1].nagib >= t.HRUs[i + 1][j].nagib && t.HRUs[i][j + 1].nagib >= t.HRUs[i][j - 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j+1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j + 1].in_prelivanje);
                        else
                        {
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j-1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else
                            this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j - 1].in_prelivanje);
                        }
                    }
                    else if(i==0 && j == t.HRUs[i].Count - 1)
                    {
                        if (t.HRUs[i + 1][j].nagib >= t.HRUs[i][j - 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i+1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i + 1][j].in_prelivanje);
                        else
                        {
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j-1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else
                            this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j - 1].in_prelivanje);

                        }
                        }
                    else if(i!=t.HRUs.Count-1 && i!=0 && j== t.HRUs[i].Count - 1)
                    {
                        if (t.HRUs[i + 1][j].nagib >= t.HRUs[i - 1][j].nagib && t.HRUs[i + 1][j].nagib >= t.HRUs[i][j - 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i + 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i + 1][j].in_prelivanje);

                        else if (t.HRUs[i - 1][j].nagib >= t.HRUs[i + 1][j].nagib && t.HRUs[i][j + 1].nagib >= t.HRUs[i][j - 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i - 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i - 1][j].in_prelivanje);
                        else
                        {
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j-1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j - 1].in_prelivanje);
                        }
                        }
                    else if(i == t.HRUs.Count - 1 && j == t.HRUs[i].Count - 1)
                    {
                        if (t.HRUs[i - 1][j].nagib >= t.HRUs[i][j - 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i - 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i - 1][j].in_prelivanje);
                        else
                        {
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j-1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j - 1].in_prelivanje);
                        }
                        }
                    else if(i == t.HRUs.Count - 1 && j!=0 && j!= t.HRUs[i].Count - 1)
                    {
                        if (t.HRUs[i - 1][j].nagib >= t.HRUs[i][j+1].nagib && t.HRUs[i - 1][j].nagib >= t.HRUs[i][j - 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i - 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                        else  this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i - 1][j].in_prelivanje);
                        else if (t.HRUs[i][j+1].nagib >= t.HRUs[i + 1][j].nagib && t.HRUs[i-1][j].nagib >= t.HRUs[i][j - 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j+1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                        else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j+1].in_prelivanje);
                        else
                        {
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j-1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                           else  this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j - 1].in_prelivanje);
                        }
                        }
                    else if(i==t.HRUs.Count-1 && j==0)
                    {
                        if (t.HRUs[i - 1][j].nagib >= t.HRUs[i][j + 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i - 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i - 1][j].in_prelivanje);
                        else
                        {
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j+1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j + 1].in_prelivanje);
                        }
                        }
                    else if(j==0 && i!=0 & i!=t.HRUs.Count-1)
                    {
                        if (t.HRUs[i - 1][j].nagib >= t.HRUs[i][j + 1].nagib && t.HRUs[i - 1][j].nagib >= t.HRUs[i + 1][j].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i - 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i - 1][j].in_prelivanje);
                        else if (t.HRUs[i + 1][j].nagib >= t.HRUs[i - 1][j].nagib && t.HRUs[i + 1][j].nagib >= t.HRUs[i][j + 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i + 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i + 1][j].in_prelivanje);
                        else
                        {
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j+1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                           else  this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j + 1].in_prelivanje);
                        }
                      }
                    else
                    {
                        if (t.HRUs[i - 1][j].nagib >= t.HRUs[i][j + 1].nagib && t.HRUs[i - 1][j].nagib >= t.HRUs[i + 1][j].nagib && t.HRUs[i - 1][j].nagib >= t.HRUs[i][j - 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i - 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i - 1][j].in_prelivanje);
                        else if (t.HRUs[i + 1][j].nagib >= t.HRUs[i - 1][j].nagib && t.HRUs[i + 1][j].nagib >= t.HRUs[i][j + 1].nagib && t.HRUs[i + 1][j].nagib >= t.HRUs[i][j - 1].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i + 1][j].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i + 1][j].in_prelivanje);
                        else if (t.HRUs[i][j + 1].nagib >= t.HRUs[i - 1][j].nagib && t.HRUs[i][j + 1].nagib >= t.HRUs[i][j - 1].nagib && t.HRUs[i][j + 1].nagib >= t.HRUs[i + 1][j].nagib)
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j + 1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                            else this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j + 1].in_prelivanje);
                        else
                        {
                            if (t.HRUs[i][j].nagib > t.HRUs[i][j-1].nagib)
                                new PortValue(t.HRUs[i][j].in_blokadaQwp, true);
                           else  this.AddCP(t.HRUs[i][j].out_prelivanje, t.HRUs[i][j - 1].in_prelivanje);
                        }
                        }
                }
            }

            List<Vector2D> koordOblaka1 = new List<Vector2D>();
            koordOblaka1.Add(new Vector2D(0,0));
            koordOblaka1.Add(new Vector2D(2, 0));
            koordOblaka1.Add(new Vector2D(2, 2));
            koordOblaka1.Add(new Vector2D(0, 2));
            Oblak ob1 = new Oblak("oblak_1", TimeUnit.Sec,new Vector2D(1,1),10,koordOblaka1,10000,1,1,1);


            List<Vector2D> koordOblaka2 = new List<Vector2D>();

            koordOblaka2.Add(new Vector2D(1, 1));
            koordOblaka2.Add(new Vector2D(2, 1));
            koordOblaka2.Add(new Vector2D(2, 2));
            koordOblaka2.Add(new Vector2D(1, 2));

            Oblak ob2 = new Oblak("oblak_2", TimeUnit.Sec, new Vector2D(1,1), 10, koordOblaka2, 10000, 1, 1, 1);



            List<Vector2D> koordOblaka3 = new List<Vector2D>();

            koordOblaka3.Add(new Vector2D(4, 4));
            koordOblaka3.Add(new Vector2D(5, 4));
            koordOblaka3.Add(new Vector2D(5, 5));
            koordOblaka3.Add(new Vector2D(4, 5));

            Oblak ob3 = new Oblak("oblak_3", TimeUnit.Sec, new Vector2D(1,1), 10, koordOblaka3, 100000, 1, 1, 1);
            

            this.AddModel(ob1);
            this.AddModel(ob2);
            this.AddModel(ob3);

            this.AddCP(this.out_keyboardTemp, ob1.in_temperature);
            this.AddCP(this.out_keyboardTemp, ob2.in_temperature);
            this.AddCP(this.out_keyboardTemp, ob3.in_temperature);


            for (int i = 0; i < t.HRUs.Count; i++)
            {
                for(int j = 0; j < t.HRUs[i].Count; j++)
                {
                    this.AddCP(ob1.out_kisa, t.HRUs[i][j].in_kise[0]);
                    this.AddCP(ob2.out_kisa, t.HRUs[i][j].in_kise[1]);
                    this.AddCP(ob3.out_kisa, t.HRUs[i][j].in_kise[2]);
                }
            }
            int brojGranicnihHRU = 0;
            int brojHRU = 0;
            for(int i = 0; i < t.HRUs.Count; i++)
            {
                for (int j = 0; j < t.HRUs[i].Count; j++)
                {
                    if (i == 0 || j == 0 || i == t.HRUs.Count-1 || j == t.HRUs[i].Count-1)
                        brojGranicnihHRU++;

                    brojHRU++;
                }
            }


            Writter w = new Writter("writterQwsAndQs",TimeUnit.Sec ,brojHRU,brojGranicnihHRU);
            this.AddModel(w);
            int count = 0;
            int countPrelivanje=0;
            for(int i = 0; i < t.HRUs.Count;i++)
            {
                for (int j = 0; j < t.HRUs[i].Count; j++)
                {
                    if (i == 0 || j == 0 || i == t.HRUs.Count - 1 || j == t.HRUs[i].Count - 1)
                    {
                        this.AddCP(t.HRUs[i][j].out_prelivanje, w.in_hru_prelivanje[count]);
                        countPrelivanje++;
                    }
                        this.AddCP(t.HRUs[i][j].out_protok, w.in_hru_protok[count]);
                    count++;
                }
            }


            OblakWritter ow = new OblakWritter("oblakWritter", TimeUnit.Sec,3);
            this.AddModel(ow);
            this.AddCP(ob1.out_stampanje, ow.in_oblaci[0]);
            this.AddCP(ob2.out_stampanje, ow.in_oblaci[1]);
            this.AddCP(ob3.out_stampanje, ow.in_oblaci[2]);
        
        }
    }
}
