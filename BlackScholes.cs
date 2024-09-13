using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BlackScholes
{
    // Black-Scholes Call Option Pricing
    public static double BsCall(double mu, double S, double K, double discount, double std)
    {
        double d1 = (Log(S / K) + mu + 0.5 * Pow(std, 2)) / std;
        double d2 = d1 - std;
        return discount * (S * Normal.CDF(0, 1, d1) - K * Normal.CDF(0, 1, d2));
    }

    // Black-Scholes Vega
    public static double BsVega(double mu, double S, double K, double discount, double sigma, double T)
    {
        double d1 = (Log(S / K) + 0.5 * Pow(sigma, 2) * T + mu) / (sigma * Sqrt(T));
        return S * discount * Exp(-0.5 * Pow(d1, 2)) / Sqrt(2 * PI) * Sqrt(T);
    }

    // Black-Scholes Delta
    public static double BsDelta(double mu, double S, double K, double discount, double sigma, double T)
    {
        double d1 = (Log(S / K) + mu + 0.5 * Pow(sigma, 2) * T) / (sigma * Sqrt(T));
        return Normal.CDF(0, 1, d1);
    }

    // Vanna calculation
    public static double BsVanna(double mu, double S, double K, double discount, double sigma, double T)
    {
        double d1 = (Log(S / K) + mu + 0.5 * Pow(sigma, 2) * T) / (sigma * Sqrt(T));
        double vega = BsVega(mu, S, K, discount, sigma, T);
        return (d1 / sigma) * vega;
    }

    // Volga calculation
    public static double BsVolga(double mu, double S, double K, double discount, double sigma, double T)
    {
        double d1 = (Log(S / K) + mu + 0.5 * Pow(sigma, 2) * T) / (sigma * Sqrt(T));
        double d2 = d1 - sigma * Sqrt(T);
        double vega = BsVega(mu, S, K, discount, sigma, T);
        return vega * d1 * d2 / sigma;
    }
}