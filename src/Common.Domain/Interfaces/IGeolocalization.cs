using Common.Domain.Geolocalization;

namespace Common.Domain.Interfaces
{
    public interface IGeolocalization 
    {
        string Key { get; set; }
        Localization FindByAddress(string address);
    }
}
