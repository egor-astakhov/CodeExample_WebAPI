using System.ComponentModel.DataAnnotations;

namespace TitanApi.Models.Devices
{
    public class CreateDeviceDC
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
