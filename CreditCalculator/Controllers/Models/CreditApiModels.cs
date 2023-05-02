using System.ComponentModel.DataAnnotations;
using CreditCalculator.Domain;

namespace CreditCalculator.Controllers.Models;

public class CalculateCreditApiModel
{
    /// <summary>
    /// Общая сумма кредита
    /// </summary>
    [Required]
    public decimal? CreditAmount { get; set; }
    
    /// <summary>
    /// Дата выдачи кредита
    /// </summary>
    [Required]
    public DateTime? IssueDate { get; set; }
    
    /// <summary>
    /// Дата закрытия кредита
    /// </summary>
    [Required]
    public DateTime? ClosingDate { get; set; }
    
    /// <summary>
    /// Процент под который выдается кредит
    /// </summary>
    [Required]
    public decimal? InterestRate { get; set; }
    
    /// <summary>
    /// Тип кредита
    /// </summary>
    [Required]
    public СhartType? СhartType { get; set; }
}