namespace Adani.Solution.DTO
{
    public class ImportAddressDto
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public bool CityIsActive { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public bool DistrictIsActive { get; set; }
        public int TerritoryId { get; set; }
        public string TerritoryName { get; set; }
        public bool TerritoryIsActive { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public bool StateIsActive { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyName { get; set; }
        public bool CountryIsActive { get; set; }
    }
}
