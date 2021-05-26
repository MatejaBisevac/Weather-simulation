using System;
using System.Collections.Generic;
using System.Text;
using DEVSsharp;

namespace Mateja_Bisevac_40_2017
{
    class Teren
    {
        public List<List<HRU>> HRUs;
        public Teren(List<List<HRU>> hruovi)
        {
            this.HRUs = hruovi;
        }
    }
}
