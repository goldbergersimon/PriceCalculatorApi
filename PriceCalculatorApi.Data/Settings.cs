using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PriceCalculatorApi.Data;

public class Settings
{
    [Key]
    [Column(TypeName = "nvarchar(25)")]
    public SettingKeys Key { get; set; }
    [MaxLength(50)]
    public string Value { get; set; }
}


