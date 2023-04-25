using System;

namespace Don_tKnowHowToNameThis {
    public class Calc {
        public readonly double _R = 8.314; //ok
        public readonly double _W = 0.20; //ok 
        public readonly double _H = 0.009; //ok
        public readonly double _L = 4.5; //ok
        // TODO:make the step variable
        public readonly double _step = 0.1;
        public readonly double _p = 1060; //ok
        public readonly double _c = 1200; //ok
        public readonly double _T0 = 175; //ok
        public readonly double _Vu = 1.2; //ok
        public readonly double _Tu = 220; //ok
        public readonly double _mu0 = 9000; //ok
        public readonly double _Ea = 92000; //ok
        public readonly double _Tr = 210; //ok
        public readonly double _n = 0.3; //ok
        public readonly double _alphaU = 450; //ok

        public readonly double _W_Min = 0.0001;
        public readonly double _H_Min = 0.0001;
        public readonly double _L_Min = 0.01;
        public readonly double _step_Min = 0.05;
        public readonly double _p_Min = 100;
        public readonly double _c_Min = 100;
        public readonly double _T0_Min = 30;
        public readonly double _Vu_Min = 0.01;
        public readonly double _Tu_Min = 30;
        public readonly double _mu0_Min = 100;
        public readonly double _Ea_Min = 100;
        public readonly double _Tr_Min = 30;
        public readonly double _n_Min = 0.01;
        public readonly double _alphaU_Min = 10;

        public readonly double _W_Max = 3;
        public readonly double _H_Max = 3;
        public readonly double _L_Max = 10;
        public readonly double _step_Max = 2;
        public readonly double _p_Max = 20000;
        public readonly double _c_Max = 10000;
        public readonly double _T0_Max = 1000;
        public readonly double _Vu_Max = 10;
        public readonly double _Tu_Max = 1000;
        public readonly double _mu0_Max = 50000;
        public readonly double _Ea_Max = 1000000;
        public readonly double _Tr_Max = 1000;
        public readonly double _n_Max = 1;
        public readonly double _alphaU_Max = 5000;


        private double gammaPoit = 0;
        private double qGamma = 0;
        private double beta = 0;
        private double qAlpha = 0;
        private double F = 0;
        private double Qch = 0;
        public double z = 0;
        public double Q = 0;

        public Calc(double W, double H, double L, double step, double p, double c, double T0, double Vu, double Tu, double mu0, double Ea, double Tr, double n, double alphaU) {
            _W = W;
            _H = H;
            _L = L;
            _step = step;
            _p = p;
            _c = c;
            _T0 = T0;
            _Vu = Vu;
            _Tu = Tu;
            _mu0 = mu0;
            _Ea = Ea;
            _Tr = Tr;
            _n = n;
            _alphaU = alphaU;
        }

        public Calc(double W, double H, double L, double step) {
            _W = W;
            _H = H;
            _L = L;
            _step = step;
        }

        public Calc() { }

        public void MaterialShearStrainRate() {
            gammaPoit = _Vu / _H;
        }
        public void SpecificHeatFluxes() {
            qGamma = _H * _W * _mu0 * Math.Pow(gammaPoit, _n + 1);
            beta = _Ea / (_R * (_T0 + 20 + 273) * (_Tr + 273));
            qAlpha = _W * _alphaU * (Math.Pow(beta, -1) - _Tu + _Tr);
        }
        public void VolumeFlowRateOfMaterialFlowInTheChannel() {
            F = 0.125 * Math.Pow(_H / _W, 2) - 0.625 * (_H / _W) + 1;
            Qch = _H * _W * _Vu * F / 2;
        }

        public double Temperature(double z) {
            return _Tr + (1 / beta) * Math.Log((beta * qGamma + _W * _alphaU) / (beta * qAlpha) * (1 - Math.Exp((-beta * qAlpha * z) / (_p * _c * Qch)))
                                                + Math.Exp(beta * (_T0 - _Tr - (qAlpha * z) / (_p * _c * Qch))));
        }
        public double Viscosity(double T) {
            return _mu0 * Math.Exp(-beta * (T - _Tr)) * Math.Pow(gammaPoit, _n - 1);
        }
        public double Efficiency() {
            Q = Math.Round(_p * Qch, 2);
            return Q;
        }
    }
}
