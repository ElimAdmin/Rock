﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CheckinClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int closeClickBuffer = 0;
        DispatcherTimer closeButtonRestartTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void frmMain_Loaded( object sender, RoutedEventArgs e )
        {
            closeButtonRestartTimer.Tick += new EventHandler( closeButtonRestartTimer_Tick );
            closeButtonRestartTimer.Interval = new TimeSpan( 0, 0, 10 );

            RockCheckinScriptManager scriptManager = new RockCheckinScriptManager( this );
            wbMain.ObjectForScripting = scriptManager;
            wbMain.AllowDrop = false;

            //this.Topmost = true; // remove before flight
            puOverlay.IsOpen = true;
        }

        private void btnClose_Click( object sender, RoutedEventArgs e )
        {
            // start a timer to clear the close buffer if the user releases the button
            if ( closeClickBuffer == 0 )
                closeButtonRestartTimer.Start();

            closeClickBuffer++;

            btnClose.Opacity = closeClickBuffer / 300;

            if ( closeClickBuffer > 300 )
                Application.Current.Shutdown();
        }

        // resets the close counter
        private void closeButtonRestartTimer_Tick( object sender, EventArgs e )
        {
            closeClickBuffer = 0;
            closeButtonRestartTimer.Stop();
        }
    }
}