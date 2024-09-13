using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DiscountCurve
{
    protected Dictionary<string, object> parameters;

    public DiscountCurve(Dictionary<string, object> parameters = null)
    {

        this.parameters = new Dictionary<string, object>();
    }

    public virtual void SetParams(Dictionary<string, object> parameters)
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

    public virtual double DiscountFactor(double T)
    {
        throw new NotImplementedException("base class");
    }
}
public class PiecewiseLinearDiscountCurve : DiscountCurve
{
    private List<double> rates;
    private List<double> times;
    private int numPieces;

    public override void SetParams(Dictionary<string, object> parameters)
    {
        base.SetParams(parameters);
        this.rates = (List<double>)parameters["rates"];
        this.times = (List<double>)parameters["times"];

        //assumes times are increasing, rates[0] is constant rate between 0 and times[0], rates[1] is rate between time[1] and time[2]
        // and so on and rates[-1] is the rate beyond times[-1], so length of rates should be 1 larger than times
        this.numPieces = this.rates.Count;

        if (this.times.Count != this.numPieces - 1)
        {
            throw new Exception("times should have length 1 less than rates");
        }
    }
}

public class ConstantRateDiscountCurve : DiscountCurve
{
    private double rate;

    public ConstantRateDiscountCurve(double rate)
    {
        this.rate = rate;

    }

    public override void SetParams(Dictionary<string, object> parameters)
    {
        base.SetParams(parameters);
        this.rate = (double)parameters["rate"];
    }

    public override double DiscountFactor(double T)
    {
        return Math.Exp(-T * this.rate);
    }
}
