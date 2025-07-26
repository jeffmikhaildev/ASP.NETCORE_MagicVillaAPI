using ASP.NETCoreRestAPI.Models.Dto;

namespace ASP.NETCoreRestAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO { Id = 1, Name = "Public Villa 1", Occupancy= 4, Sqft = 100},
            new VillaDTO { Id = 2, Name = "Public Villa 2", Occupancy= 6 , Sqft = 200},
          
        };
    }
}
