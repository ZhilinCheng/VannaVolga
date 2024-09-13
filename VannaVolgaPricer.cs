using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PricingEngine1;

using System;
using System.Collections.Generic;
using MathNet.Numerics;
using QuantLib;

public class VannaVolgaPricer : BasePricer
{
    public double Mu { get; set; }
    public List<double> Strikes { get; set; }
    public List<double> Prices { get; set; }
    public double SigmaAtm { get; set; }
    public double Spot { get; set; }
    public double Pv01 { get; set; }
    public DateTime Expiry { get; set; }
    public double Rate { get; set; }
    public DateTime AsOfTime { get; set; }

    public double T { get; set; }

    public VannaVolgaPricer(Dictionary<string, object> parameters) : base(parameters)
    {
        SetParams(parameters);
    }

    public void SetParams(Dictionary<string, object> parameters)
    {
        if (parameters != null && parameters.Count > 0)
        {
            Mu = (double)parameters["mu"];
            Strikes = (List<double>)parameters["strikes"];
            Prices = (List<double>)parameters["prices"];
            SigmaAtm = (double)parameters["sigma_atm"];
            Spot = (double)parameters["spot"];
            Pv01 = (double)parameters["pv01"];
            Expiry = (DateTime)parameters["expiry"];
            Rate = (double)parameters["rate"];
            AsOfTime = (DateTime)parameters["as_of_time"];
           

            TimeSpan timeToMat = Expiry - AsOfTime;
            T = (timeToMat.Days + timeToMat.Seconds / 86400.0) / 365.0;
        }
    }

    public double AtmVega(double K)
    {
        return BlackScholes.BsVega(Mu, Spot, K, Pv01, SigmaAtm, T);
    }

    public double AtmVanna(double K)
    {
        return BlackScholes.BsVanna(Mu, Spot, K, Pv01, SigmaAtm, T);
    }

    public double AtmVolga(double K)
    {
        return BlackScholes.BsVolga(Mu, Spot, K, Pv01, SigmaAtm, T);
    }

    public double BsCall(double K, double sigma)
    {
        return BlackScholes.BsCall(Mu, Spot, K, Pv01, sigma * Math.Sqrt(T));
    }

    public double PriceCall(double K)
    {
        var weights = CalculateWeights(K);
        double price = BsCall(K, SigmaAtm) +
                       weights[0] * (Prices[0] - BsCall(Strikes[0], SigmaAtm)) +
                       weights[1] * (Prices[1] - BsCall(Strikes[1], SigmaAtm)) +
                       weights[2] * (Prices[2] - BsCall(Strikes[2], SigmaAtm));
        return price;
    }

    public List<double> CalculateWeights(double K)
    {
        List<double> weights = new List<double> { 0, 0, 0 };

        weights[0] = AtmVega(K) / AtmVega(Strikes[0]) * Math.Log(Strikes[1] / K) *
                     Math.Log(Strikes[2] / K) / (Math.Log(Strikes[1] / Strikes[0]) * Math.Log(Strikes[2] / Strikes[0]));
        weights[1] = AtmVega(K) / AtmVega(Strikes[1]) * Math.Log(Strikes[2] / K) *
                     Math.Log(Strikes[0] / K) / (Math.Log(Strikes[0] / Strikes[1]) * Math.Log(Strikes[2] / Strikes[1]));
        weights[2] = AtmVega(K) / AtmVega(Strikes[2]) * Math.Log(Strikes[0] / K) *
                     Math.Log(Strikes[1] / K) / (Math.Log(Strikes[0] / Strikes[2]) * Math.Log(Strikes[1] / Strikes[2]));

        return weights;
    }
}


public class BarrierValue
{
    private VannaVolgaPricer pricer;
    private string knockType;
    private string optionType;
    private double barrier;
    private double strike;
    private QuantLib.Date expiry;
    private double psym;
    private List<double> strikes;

    public BarrierValue(VannaVolgaPricer pricer, Dictionary<string, object> parameters = null)
    {
        this.pricer = pricer;
        this.strikes = pricer.Strikes;
        SetParams(parameters);
    }

    public void SetParams(Dictionary<string, object> parameters)
    {
        if (parameters == null) return;
        this.knockType = (string)parameters["knock_type"];
        this.optionType = (string)parameters["option_type"];
        this.barrier = Convert.ToDouble(parameters["barrier"]);
        this.strike = Convert.ToDouble(parameters["strike"]);
        this.expiry = (QuantLib.Date)parameters["expiry"];
        this.psym = Convert.ToDouble(parameters["psym"]);
    }

    public double Price()
    {
        BarrierOptionPricer pricerEngine = new BarrierOptionPricer();
        double vanillaPrice = pricerEngine.PriceBarrierOption();
        double adjustedPrice = VannaVolgaAdjustment(vanillaPrice);

        return vanillaPrice;
    }

    private double VannaVolgaAdjustment(double vanillaPrice)
    {
        // Adjust the price using Vanna-Volga method
        var prices = pricer.Prices;
        double weight0 = 0.5 + 0.5 * psym;
        double weight1 = psym;
        double weight2 = 0.5 + 0.5 * psym;

        double adjustedPrice = vanillaPrice +
                               weight0 * (prices[0] - pricer.BsCall(strikes[0], pricer.SigmaAtm)) +
                               weight1 * (prices[1] - pricer.BsCall(strikes[1], pricer.SigmaAtm)) +
                               weight2 * (prices[2] - pricer.BsCall(strikes[2], pricer.SigmaAtm));
        return adjustedPrice;
    }
}



