﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQADemicApp
{
    public partial class PicForm : Form
    {
        /// <summary>
        /// show form for user information
        /// </summary>
        /// <param name="outbreak">true for outbreak, false for epidemic</param>
        /// <param name="cityName">city name that is displayed on form</param>
        public PicForm(bool outbreak, string cityName)
        {
            InitializeComponent();
            if(outbreak)
            {
                Text = "Outbreak";
                ClientSize = new System.Drawing.Size(667, 521);
                BackgroundImage = global::SQADemicApp.Properties.Resources.Ebola_Virus_outbreak_2;
                label1.Text = cityName;
            }
            else
            {
                Text = cityName;
                BackgroundImage = global::SQADemicApp.Properties.Resources.Epidemic;
                ClientSize = new System.Drawing.Size(276, 385);
            }   
        }
    }
}
