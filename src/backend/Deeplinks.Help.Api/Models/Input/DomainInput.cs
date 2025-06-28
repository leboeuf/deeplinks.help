using System.ComponentModel.DataAnnotations;

namespace Deeplinks.Help.Api.Models.Input
{
    public class DomainInput
    {
        [Required]
        public required string Domain { get; init; }
    }
}
