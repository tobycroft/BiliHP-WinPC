using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace BiliHP2020.login
{
    public partial class eula : Form
    {
        public eula()
        {
            InitializeComponent();
        }

        private void eula_Load(object sender, EventArgs e)
        {
            WebClient wb = new WebClient();
            wb.Headers.Add(HttpRequestHeader.Cookie, "");
        }
    }
}
