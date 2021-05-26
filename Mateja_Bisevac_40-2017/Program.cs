using System;
using DEVSsharp;
namespace Mateja_Bisevac_40_2017
{
    class Program
    {
        static void Main(string[] args)
        {
            SRTEngine engine = new SRTEngine(new Sistem("sistemOblaka",10.0),10000, ulazTemperatura);
            engine.RunConsoleMenu();
        }
        static PortValue ulazTemperatura(Devs model)
        {
            Sistem s = (Sistem)model;
            if(s!=null)
            {
                string line = Console.ReadLine();
                if (line != null)
                {
                    int lineParse = int.Parse(line); //Tk vrednost
                    if (s.Tk > lineParse) // pala temp ispod 10 salji kisu 
                        return new PortValue(s.out_keyboardTemp, true);
                    else   // nije pala temp ispod 10, ne salji kisu
                        return new PortValue(s.out_keyboardTemp, false);
                }
                
                }
            return new PortValue(null, null);
        }
    }
}
