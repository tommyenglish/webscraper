using System.Collections.Generic;
using System.Linq;

namespace WebScraperEngine.Models
{
    public class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Groups => Enumerable.Empty<string>();
        public IEnumerable<string> Skills => Enumerable.Empty<string>();
        public IEnumerable<string> Pathways => Enumerable.Empty<string>();
    }
}
