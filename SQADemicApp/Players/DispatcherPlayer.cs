using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.Players
{
    public class DispatcherPlayer : AbstractPlayer
    {
        public override bool DispatcherMovePlayer(List<AbstractPlayer> players, City destination)
        {
            // TODO: Fix test cases, replace next line with commented blocks
            return base.DispatcherMovePlayer(players, destination);

            /**
            // TODO: Further refactoring is possible but has less priority.
            if (!(DriveOptions(currentCity).Any(p => p.Name.Equals(destination.Name)) ||
                players.Any(p => p.currentCity.Name.Equals(destination.Name))
                || ShuttleFlightOption(currentCity).Contains(destination.Name)))
            {
                return false;
            }
            currentCity = destination;
            return true;
            */
        } 
    }
}
