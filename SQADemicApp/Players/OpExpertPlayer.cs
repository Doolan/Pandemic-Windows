﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.Players
{
    public class OpExpertPlayer : AbstractPlayer
    {
        public override bool BuildAResearchStationOption()
        {
            CurrentCity.ResearchStation = true;
            return true;
        }
    }
}
