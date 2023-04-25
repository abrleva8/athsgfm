using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Don_tKnowHowToNameThis {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        List<double> zCoord;
        List<double> temperature;
        List<double> viscosity;
        List<double> q;
        public MainWindow() {
            InitializeComponent();
        }
        Calc calc = new Calc();
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            step.Text = calc._step.ToString();
            W.Text = calc._W.ToString();
            H.Text = calc._H.ToString();
            L.Text = calc._L.ToString();
            p.Text = calc._p.ToString();
            c.Text = calc._c.ToString();
            T0.Text = calc._T0.ToString();
            Vu.Text = calc._Vu.ToString();
            Tu.Text = calc._Tu.ToString();
            mu0.Text = calc._mu0.ToString();
            Ea.Text = calc._Ea.ToString();
            Tr.Text = calc._Tr.ToString();
            n.Text = calc._n.ToString();
            alphaU.Text = calc._alphaU.ToString();

        }


        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            if (calculateLists()) {
                Table table = new Table(zCoord, temperature, viscosity);
                table.Show();
                eff.Content = calc.Efficiency().ToString();
                T.Content = Math.Round(temperature[temperature.Count - 1], 2).ToString();
                visc.Content = Math.Round(viscosity[viscosity.Count - 1], 2).ToString();
                menuItemPlot.IsEnabled = true;
                excelSaveItem.IsEnabled = true;
            }
        }

        private bool isGood(double min, double max, TextBox text) {
            try {
                double.Parse(text.Text);
            } catch {
                text.Foreground = new SolidColorBrush(Colors.Red);
                return false;
            }
            if (min <= double.Parse(text.Text) && double.Parse(text.Text) <= max) {
                text.Foreground = new SolidColorBrush(Colors.Black);
                return true;
            }
            text.Foreground = new SolidColorBrush(Colors.Red);
            return false;
        }

        private bool calculateLists() {
            calc = new Calc(Convert.ToDouble(W.Text), Convert.ToDouble(H.Text), Convert.ToDouble(L.Text), Convert.ToDouble(step.Text), Convert.ToDouble(p.Text), Convert.ToDouble(c.Text),
                   Convert.ToDouble(T0.Text), Convert.ToDouble(Vu.Text), Convert.ToDouble(Tu.Text), Convert.ToDouble(mu0.Text), Convert.ToDouble(Ea.Text), Convert.ToDouble(Tr.Text),
                   Convert.ToDouble(n.Text), Convert.ToDouble(alphaU.Text));
            bool flag = true;
            if (!isGood(calc._W_Min, calc._W_Max, W))
                flag = false;
            if (!isGood(calc._H_Min, calc._H_Max, H))
                flag = false;
            if (!isGood(calc._L_Min, calc._L_Max, L))
                flag = false;
            if (!isGood(calc._step_Min, calc._step_Max, step))
                flag = false;
            if (!isGood(calc._p_Min, calc._p_Max, p))
                flag = false;
            if (!isGood(calc._c_Min, calc._c_Max, c))
                flag = false;
            if (!isGood(calc._T0_Min, calc._T0_Max, T0))
                flag = false;
            if (!isGood(calc._Vu_Min, calc._Vu_Max, Vu))
                flag = false;
            if (!isGood(calc._Tu_Min, calc._Tu_Max, Tu))
                flag = false;
            if (!isGood(calc._mu0_Min, calc._mu0_Max, mu0))
                flag = false;
            if (!isGood(calc._Ea_Min, calc._Ea_Max, Ea))
                flag = false;
            if (!isGood(calc._Tr_Min, calc._Tr_Max, Tr))
                flag = false;
            if (!isGood(calc._n_Min, calc._n_Max, n))
                flag = false;
            if (!isGood(calc._alphaU_Min, calc._alphaU_Max, alphaU))
                flag = false;
            if (flag) {
                zCoord = new List<double>();
                temperature = new List<double>();
                viscosity = new List<double>();
                q = new List<double>();

                calc.MaterialShearStrainRate();
                calc.SpecificHeatFluxes();
                calc.VolumeFlowRateOfMaterialFlowInTheChannel();

                for (double z = 0; z <= calc._L; z = Math.Round(z + calc._step, 1)) {
                    zCoord.Add(z);
                    double T = calc.Temperature(z);
                    temperature.Add(T);
                    double n = Math.Round(calc.Viscosity(T), 2);
                    viscosity.Add(n);
                    q.Add(calc.Efficiency());
                }
                return true;
            }
            return false;
        }

        private void CheckInputChange(object sender, System.Windows.Controls.TextChangedEventArgs e) {
            System.Windows.Controls.TextBox a = (System.Windows.Controls.TextBox)e.Source;
            //a.Foreground = Brushes.Red;
            double temp;
            if (double.TryParse(a.Text, out temp)) {
                a.Foreground = Brushes.Black;
                tableValueButton.IsEnabled = true;
            } else {
                a.Foreground = Brushes.Red;
                //tableValueButton.IsEnabled = false;
            }
        }

        private void menuItemPlot_Click(object sender, RoutedEventArgs e) {
            var windowPlot = new WindowPlot(zCoord, temperature, viscosity);
            windowPlot.Show();
        }

        private void SaveExcel_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == false) {
                return;
            };
            
            string fileName = sfd.FileName;
            ExcelWorker excelWorker;
            if (fileName.Contains(".xlsx")) {
                excelWorker = new ExcelWorker(zCoord, temperature, viscosity, q,  fileName);
            } else { excelWorker = new ExcelWorker(zCoord, temperature, viscosity, q, fileName + ".xlsx");
            }
            excelWorker.SaveToExel();
        }
    }
}
