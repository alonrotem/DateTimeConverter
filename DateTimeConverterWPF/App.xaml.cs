using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DateTimeConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App ()
	    {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("he-IL");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("he-IL");
    	}
    }
}
