using ASP.NETCoreRestAPI.Models.Dto;

namespace ASP.NETCoreRestAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO { Id = 1, Name = "Public Villa 1"},
            new VillaDTO { Id = 2, Name = "Public Villa 2" },
            new VillaDTO { Id = 3, Name = "Public Villa 3"},
            new VillaDTO { Id = 4, Name = "Public Villa 4"},
            new VillaDTO { Id = 5, Name = "Public Villa 5"},
        };
    }
}
