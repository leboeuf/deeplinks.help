﻿using System.ComponentModel.DataAnnotations;

namespace Deeplinks.Help.Api.Models.Input
{
    public class HashInput
    {
        [Required]
        public required string Hash { get; init; }
    }
}
