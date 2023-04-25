﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace Don_tKnowHowToNameThis {
    internal class ExcelWorker {
        private string _path { get; set; }
        List<double> zCoords;
        List<double> temperature;
        List<double> viscosity;
        List<double> q;
        Calc _calc = new Calc();

        public ExcelWorker(Calc calc, string fileName) {
            _path = fileName;
            _calc = calc;
        }

        public ExcelWorker(List<double> zCoord, List<double> temperature, List<double> viscosity, List<double> q, string fileName) {
            zCoords = zCoord;
            this.temperature = temperature;
            this.viscosity = viscosity;
            this.q = q;
            _path = fileName;
        }

        public void SaveToExel() {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage()) {
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                FileInfo file = new FileInfo(_path);
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(file.Name);

                int rowAndColumn = 1;
                worksheet.Cells[rowAndColumn, rowAndColumn].Value = "Координата по длине канала, м";
                worksheet.Cells[rowAndColumn, rowAndColumn + 1].Value = "Температура, °С";
                worksheet.Cells[rowAndColumn, rowAndColumn + 2].Value = "Вязкость, Па*с";
                for (int i = 0; i < zCoords.Count; ++i) {
                    worksheet.Cells[rowAndColumn + i + 1, rowAndColumn].Value = zCoords[i];
                    worksheet.Cells[rowAndColumn + i + 1, rowAndColumn + 1].Value = temperature[i];
                    worksheet.Cells[rowAndColumn + i + 1, rowAndColumn + 2].Value = viscosity[i];
                    worksheet.Cells[rowAndColumn + i + 1, rowAndColumn + 3].Value = q[i];
                }

                int r = rowAndColumn;
                int c = rowAndColumn + 5;
                Dictionary<string, object> inputDatas = new Dictionary<string, object>()
                {
                    { "Входные данные",                                                     "" },
                    { "Тип материала",                                                      "" },
                    { "", ""},
                    { "Геометрические параметры канала:",                                   "" },
                    { "Ширина, м",                                                          _calc._W },
                    { "Глубина, м",                                                         _calc._H },
                    { "Длина, м",                                                           _calc._L },
                    { " ", ""},
                    { "Параметры свойств материала:",                                       ""},
                    { "Плотность, кг/м^3",                                                  _calc._R },
                    { "Удельная теплоёмкость, Дж/(кг*°С)",                                  _calc._c },
                    { "Температура плавления, °С",                                          _calc._T0 },
                    { "  ", ""},
                    { "Режимные параметры процесса:",                                       "" },
                    { "Скорость крышки, м/с",                                               _calc._Vu },
                    { "Температура крышки, °С",                                             _calc._Tu },
                    { "   ", ""},
                    { "Параметры метода решения уравнений модели:",                         "" },
                    { "Шаг расчета по длине канала, м",                                     _calc._step },
                    { "    ", ""},
                    { "Эмпирические коэффициенты математической модели:",                   "" },
                    { "Коэффициент консистенции при температуре приведения, Па*с^n",        _calc._mu0 },
                    { "Энергия активации вязкого течения материала, Дж/моль",                  _calc._Ea },
                    { "Температура приведения, °С",                                         _calc._Tr },
                    { "Индекс течения",                                                     _calc._n },
                    { "Коэффициент теплоотдачи от крышки канала к материалу, Вт /(м^2*°С)", _calc._alphaU },
                    { "     ", ""},
                    { "      ", ""},
                    { "Критериальные показатели процесса:",                                 "" },
                    { "Производительность, кг/ч",                                           _calc.Q },
                    { "Температура продукта, °С",                                           temperature[temperature.Count-1] },
                    { "Вязкость продукта, Па*с",                                            viscosity[viscosity.Count -1] },
                };

                foreach (var str in inputDatas) {
                    worksheet.Cells[r, c].Value = str.Key;
                    worksheet.Cells[r++, c + 1].Value = str.Value;
                }

                try {
                    excelPackage.SaveAs(file);
                } catch (Exception ex) {
                   MessageBox.Show(ex.Message);
                }
                MessageBox.Show("Сохранение произошло успешно!");
            }
        }
    }
}