using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class Program
{
    static void Main(string[] args)
    {
        double constantRate = 0.016;
        ConstantRateDiscountCurve discount1 = new ConstantRateDiscountCurve(constantRate);
        double discount_factor = 0.25;  // time T

        double rate1 = discount1.DiscountFactor(0.25);
        Console.WriteLine($"The discount factor for T={discount_factor} is: {rate1}");

        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "mu", 0.016 },
            { "strikes", new List<double> { 1.0966 ,1.087, 1.0616 } },
            { "prices", new List<double> { 0.001555, 0.001666, 0.001766 } },
            { "sigma_atm", 0.07028 },
            { "spot", 1.0586 },
            { "pv01", 0.99 },
            { "rate", 1-rate1  },  
            { "expiry", new DateTime(2024, 1, 9) },
            { "as_of_time", new DateTime(2023, 10, 6) }
        };

        VannaVolgaPricer pricer = new VannaVolgaPricer(parameters);

        Console.WriteLine("Pricer initialized successfully.");
        Dictionary<string, object> parameters1 = new Dictionary<string, object>
        {
            { "knock_type", "down and out" },
            { "option_type", "call" },
            { "barrier", 1.0057 },
            { "strike", 1.1166 },
            { "expiry", new QuantLib.Date(9, QuantLib.Month.January, 2024) },
            { "psym", 0.0001
         }
        };
        BarrierValue barrierOption = new BarrierValue(pricer, parameters1);
        double price = barrierOption.Price();
        Console.WriteLine($"Barrier Option Price: {price}");


    }
}