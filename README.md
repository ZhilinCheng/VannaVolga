to be honest, the model I implement is wrong， several issues:
1.I should use USD and EUR discount term structure with proper interpolation, 
(or even better USD curve + fx forward implied EUR rate ), instead of one single flat yield term structure.
2. for barrier, my implementation formula is wrong
3.Delta strike conversion with proper ATM type and delta type to get the ATM, 25DC, 25DP strike,
I should use quantlib BlackDeltaCalculator or use analytical formula to transform it
4.25DC vol = atmVol + bfVol + 0.5 * rrVol

25DP vol = atmVol + bfVol - 0.5 * rrVol
