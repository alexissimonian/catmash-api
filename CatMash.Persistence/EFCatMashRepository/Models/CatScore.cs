using System.ComponentModel.DataAnnotations;

namespace CatMash.Persistence.EFCatMashRepository.Models;

public class CatScore
{
    [Key]
    public string Id { get; set; }
    public string Url { get; set; }
    public int Score { get; set; }
}