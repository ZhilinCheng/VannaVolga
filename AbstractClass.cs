global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Globalization;
global using System.Linq;
global using System.Reflection.Metadata;
global using System.Threading.Tasks;
global using static System.Math;
global using MathNet.Numerics.Distributions;
global using MathNet.Numerics.LinearAlgebra;
global using MathNet.Numerics.LinearAlgebra.Double;
global using MathNet.Numerics.Optimization;
global using Microsoft.VisualBasic.FileIO;
global using static System.Net.Mime.MediaTypeNames;
global using static System.Runtime.InteropServices.JavaScript.JSType;
public class PricingEngine1
{
    protected BasePricer pricer;



    public virtual double Price(BaseInstrument instrument)
    {
        throw new NotImplementedException("base class");
    }


    public class BaseInstrument
    {
        protected Dictionary<string, object> parameters;

        public BaseInstrument(Dictionary<string, object> parameters = null)
        {
            this.parameters = new Dictionary<string, object>();
        }

        public void SetParams(Dictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                this.parameters[param.Key] = param.Value;
            }
        }

        public Dictionary<string, object> GetParams()
        {
            return this.parameters;
        }

        public virtual double Payoff(Dictionary<string, object> kwargs)
        {
            throw new NotImplementedException("base class");
        }
    }
    public class BasePricer
    {
        protected Dictionary<string, object> parameters;

        public BasePricer(Dictionary<string, object> parameters = null)
        {
            this.parameters = new Dictionary<string, object>();
        }

        public void SetParams(Dictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                this.parameters[param.Key] = param.Value;
            }
        }

        public Dictionary<string, object> GetParams()
        {
            return this.parameters;
        }

        public virtual double Price(BaseInstrument instrument)
        {
            throw new NotImplementedException("base class");
        }
    }  
}







