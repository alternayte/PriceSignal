using System.Text.Json;
using Domain.Models.NotificationChannel;

namespace Domain.Models.PriceRule;

public record PriceRuleSnapshot(string Name, 
    string Description, string NotificationChannel, 
    string Symbol, ICollection<ConditionSnapshot> Conditions);

public record ConditionSnapshot(string ConditionType, 
    decimal Value, JsonDocument AdditionalValues);