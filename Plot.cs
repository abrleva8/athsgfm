﻿using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Helpers;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don_tKnowHowToNameThis {
    public class Plot {
        private ObservableCollection<ObservablePoint> values;
        public ObservableCollection<ISeries> Series { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        public DrawMarginFrame Frame { get; set; } = new() {
            Stroke = new SolidColorPaint {
                Color = new(0, 0, 0)
            }
        };
        public Plot(List<double> x, List<double> y, string yAxisTitle, string serieName) {
            values = new ObservableCollection<ObservablePoint>();
            Series = new ObservableCollection<ISeries>();
            LineSeries<ObservablePoint> serie = new LineSeries<ObservablePoint>();
            for (int i = 0; i < x.Count; i++) {
                values.Add(new ObservablePoint(Math.Round(x[i],2), Math.Round(y[i], 2)));
            }
            XAxes = new Axis[] {
                new Axis {
                    Name = "Координата по длине канала, м"
                }
            };

            YAxes = new Axis[] {
                new Axis {
                    Name = yAxisTitle
                }
            };
            serie.Values = values;
            serie.Fill = null;
            serie.GeometrySize = 3;
            serie.Name = serieName;
            serie.TooltipLabelFormatter = (chartPoint) => $"{YAxes[0].Name}: {chartPoint.PrimaryValue}, {XAxes[0].Name}: {chartPoint.SecondaryValue}";
            Series.Add(serie);
        }
    }
}