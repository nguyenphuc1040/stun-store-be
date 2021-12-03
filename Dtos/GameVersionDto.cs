using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Dtos
{
    public class GameVersionDto
    {
        public string IdGameVersion { get; set; }
        public string IdGame { get; set; }
        public string VersionGame { get; set; }
        public DateTime? DateUpdate { get; set; }
        public string UrlDowload { get; set; }
        public string ShortDescription { get; set; }
        public string Descriptions { get; set; }
        public string Requires { get; set; }
        public string Os { get; set; }
        public string Processor { get; set; }
        public string Storage { get; set; }
        public string DirectX { get; set; }
        public string Graphics { get; set; }
        public string PrivacyPolicy { get; set; }
        public string FilePlay { get; set; }
    }
}
