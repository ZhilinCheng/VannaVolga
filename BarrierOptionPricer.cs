using QuantLib;
using static QLNet.Callability;

class BarrierOptionPricer
{
    public double PriceBarrierOption()
    {
        // 设置基础参数
        double spot = 1.0586;
        double vol = 0.07208;
        double rate = 0.016;

        // 定义日期
        QuantLib.Date today = new QuantLib.Date(6, Month.October, 2023);
        Settings.instance().setEvaluationDate(today);

        QuantLib.Calendar calendar = new NullCalendar();
        DayCounter dayCount = new Actual365Fixed();

        YieldTermStructure flatRate = new FlatForward(today, rate, dayCount);
        YieldTermStructureHandle flatRateHandle = new YieldTermStructureHandle(flatRate);

        BlackVolTermStructure flatVol = new BlackConstantVol(today, calendar, vol, dayCount);
        BlackVolTermStructureHandle flatVolHandle = new BlackVolTermStructureHandle(flatVol);

        QuantLib.Date expiryDate = new QuantLib.Date(9, Month.January, 2024);
        double strike = 1.1166;
        double barrierLevel = 1.0057;

        QuantLib.Barrier.Type barrierType = QuantLib.Barrier.Type.DownOut;
        StrikedTypePayoff payoff = new PlainVanillaPayoff(Option.Type.Call, strike);
        Exercise exercise = new EuropeanExercise(expiryDate);
        QuantLib.BarrierOption barrierOption = new BarrierOption(barrierType, barrierLevel, 0.0, payoff, exercise);

        Quote spotQuote = new SimpleQuote(spot);
        QuoteHandle spotHandle = new QuoteHandle(spotQuote);
        BlackScholesProcess process = new BlackScholesProcess(spotHandle, flatRateHandle, flatVolHandle);

        PricingEngine engine = new AnalyticBarrierEngine(process);
        barrierOption.setPricingEngine(engine);
        double price = barrierOption.NPV();

        return price;
    }
}
