﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Portfolio.API.Services.GitApi.Models
{
    public class RepositoryDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("languages_url")]
        public string LanguagesUrl { get; set; }
    }
}
