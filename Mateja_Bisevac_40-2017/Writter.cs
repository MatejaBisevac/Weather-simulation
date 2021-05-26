using System;
using System.Collections.Generic;
using System.Text;
using DEVSsharp;
using System.IO;

namespace Mateja_Bisevac_40_2017
{
    class Writter : Atomic
    {

        StreamWriter swQwp;
        public List<InputPort> in_hru_protok;
        public List<InputPort> in_hru_prelivanje;
        List<double> qwpValues; // Voda koja protice tj Qwp sum
        List<double> qsValues; //Voda koja preliva
        public Writter(string name, TimeUnit tu, int broj_hru_ova, int broj_hru_preliva) : base(name,tu)
        {
            in_hru_protok = new List<InputPort>();
            in_hru_prelivanje = new List<InputPort>();
            qwpValues = new List<double>();
            qsValues = new List<double>();
            for(int i = 0; i < broj_hru_ova; i++)
            {
                InputPort in_protok = AddIP("in_hru_protok" + i);
                in_hru_protok.Add(in_protok);               
                qwpValues.Add(0);
               
            }
            for(int i = 0; i< broj_hru_preliva; i++)
            {
                qsValues.Add(0);
                InputPort in_prelivanje = AddIP("in_hru_prelivanje" + i);
                in_hru_prelivanje.Add(in_prelivanje);
            }
        }

        public override bool delta_x(PortValue x)
        {
            for(int i = 0; i < in_hru_protok.Count; i++)
            {
                if(x.port==in_hru_protok[i])
                {
                    
                    qwpValues[i] = (double)x.value;
                    double sumOfQwp =0.0;
                    foreach (var item in qwpValues)
                        sumOfQwp += item;
                    double sumOfQs = 0.0;
                    foreach (var item in qsValues)
                        sumOfQs += item;

                    swQwp.WriteLine(TimeCurrent + "," + sumOfQwp + "," + sumOfQs);
                }
            }
            for(int i = 0; i < in_hru_prelivanje.Count; i++)
            {

            }
            return false;

        }

        public override void delta_y(ref PortValue y)
        {
            throw new NotImplementedException();
        }

        public override void init()
        {
            if (swQwp != null) swQwp.Close();
            swQwp = new StreamWriter("IzlazQwpAndQs.csv");
            swQwp.WriteLine("Time(H), Sum of Qwp, Sum of Qs");
            swQwp.Flush();
        }

        public override double tau()
        {
            return Double.MaxValue;
        }
    }
}
