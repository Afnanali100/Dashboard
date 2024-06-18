using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PlantifyApp.Core.Models;

namespace PlantifyApp.Apis.Dtos
{
    public class CommentDto
    {
        [Key]
        public int? comment_id { get; set; }

        [ForeignKey("Post")]
        public int? post_id { get; set; }

        public string? user_id { get; set; }

        public string? content { get; set; }
        [JsonIgnore]
        public Posts? Post { get; set; } // Navigation property


        public DateTime? creation_date { get; set; }
    }
}
