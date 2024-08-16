using System.ComponentModel.DataAnnotations;

namespace Kasbo.Models;

public class BaseEntity<T>
{
    [Key]
    public T Id { get; set; }
    public DateTime CreateDate { get; set; }=DateTime.Now;
}