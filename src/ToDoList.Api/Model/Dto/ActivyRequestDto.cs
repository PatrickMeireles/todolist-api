using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Model.Dto;

public class ActivyRequestDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [DataType(DataType.Date)]
    [CurrentDate(ErrorMessage = "The Date Estimated Finish must be more that today.")]
    public DateTime? DateEstimatedFinish { get; set; }

    [Required]
    [EnumDataType(typeof(Priority))]
    public int Priority { get; set; }


    public static explicit operator Activity(ActivyRequestDto model)
    {
        var activity = Activity.Create(model.Name, model.Description, model.DateEstimatedFinish, (Priority)model.Priority);

        return activity;
    }
}

public class CurrentDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        DateTime dateTime = Convert.ToDateTime(value);
        return dateTime.Date >= DateTime.Now.Date;
    }
}
