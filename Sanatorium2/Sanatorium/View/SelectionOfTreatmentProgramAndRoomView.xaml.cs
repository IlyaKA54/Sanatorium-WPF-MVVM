﻿using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Sanatorium.ViewModel;

namespace Sanatorium.View
{

    public partial class SelectionOfTreatmentProgramAndRoomView : Window
    {
        public SelectionOfTreatmentProgramAndRoomView()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }
        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        public void SetDataContext(SelectionOfTreatmentProgramAndRoomViewModel selectionOfTreatmentProgramAndRoomViewModel)
        {
            selectionOfTreatmentProgramAndRoomViewModel.Close += this.Close;
            DataContext = selectionOfTreatmentProgramAndRoomViewModel;
        }

    }
}
