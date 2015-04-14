﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SQADemicApp.BL
{
    public static class HelperBL
    {
        //The following shuffiling code refrences comon array shuffling techniques and was gotten from: http://stackoverflow.com/questions/108819/best-way-to-randomize-a-string-array-with-net 
        //This code is not my own, and combinied with the randomness, is not generated with TDD

        /// <summary>
        /// Suffles a string[]
        /// </summary>
        /// <param name="unshuffledArry"></param>
        /// <returns>a shuffled Arry</returns>
        public static string[] shuffleArray(string[] unshuffledArry)
        {
            RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
            string[] shuffledarray = unshuffledArry.OrderBy(x => GetNextInt32(rnd)).ToArray();
            return shuffledarray;
        }

        private static int GetNextInt32(RNGCryptoServiceProvider rnd)
        {
            byte[] randomInt = new byte[4];
            rnd.GetBytes(randomInt);
            return Convert.ToInt32(randomInt[0]);
        }
    }
}
