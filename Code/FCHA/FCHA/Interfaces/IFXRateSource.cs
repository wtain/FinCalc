namespace FCHA.Interfaces
{
    public interface IFXRateSource
    {
        FxRate GetFXRate(string CCY);
    }
}