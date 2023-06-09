using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyPoezd.Models;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле 'Фамилия' обязательно для заполнения.")]
    [RegularExpression(@"^[А-Яа-я]+$", ErrorMessage = "Поле 'Фамилия' должно содержать только русские буквы.")]
    public string? Surname { get; set; }
    [Required(ErrorMessage = "Поле 'Имя' обязательно для заполнения.")]
    [RegularExpression(@"^[А-Яа-я]+$", ErrorMessage = "Поле 'Имя' должно содержать только русские буквы.")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Поле 'Отчество' обязательно для заполнения.")]
    [RegularExpression(@"^[А-Яа-я]+$", ErrorMessage = "Поле 'Отчество' должно содержать только русские буквы.")]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Поле 'Номер телефона' обязательно для заполнения.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Поле 'Номер телефона' должно содержать 11 символов.")]
    [RegularExpression(@"^[^\d]*$", ErrorMessage = "Поле 'Номер телефона' не должно содержать цифры.")]
    public string? NumberPhone { get; set; }

    [Required(ErrorMessage = "Поле 'Паспорт' обязательно для заполнения.")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Поле 'Паспорт' должно содержать 10 символов.")]
    public string? PassportData { get; set; }
    [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения.")]
    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
