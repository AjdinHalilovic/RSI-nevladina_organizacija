using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Base.DTO
{
    public class SponsorDTO
    {
        public int Id { get; set; }
        public int SponsorId { get; set; }
        public string Name { get; set; }
        public string SponsorType { get; set; }
        public string WebUrl { get; set; }
        public string LogoBase64 { get; set; }
        public string Description { get; set; }

    }
}
