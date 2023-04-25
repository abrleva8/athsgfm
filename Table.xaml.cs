﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Don_tKnowHowToNameThis {
    /// <summary>
    /// Логика взаимодействия для Table.xaml
    /// </summary>
    public partial class Table : Window {
        List<double> zCoords = new List<double>();
        List<double> temperature = new List<double>();
        List<double> viscosity = new List<double>();
        BindingList<List> data = new BindingList<List>();
        public Table(List<double> z, List<double> T, List<double> n) {
            InitializeComponent();
            zCoords = z;
            temperature = T;
            viscosity = n;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            for (int i = 0; i < temperature.Count; i++) {
                data.Add(new List { _z = Math.Round(zCoords[i], 2), _T = Math.Round(temperature[i], 2), _n = Math.Round(viscosity[i], 2) });
            }
            table.ItemsSource = data;
        }
    }
}
