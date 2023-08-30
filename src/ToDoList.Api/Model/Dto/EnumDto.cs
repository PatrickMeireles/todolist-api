namespace ToDoList.Api.Model.Dto;

public class EnumDto
{
    public int Key { get; set; }
    public string Description { get; set; } = string.Empty;

    public EnumDto(Enum @enum)
    {
        Key = Convert.ToInt32(@enum);
        Description = @enum.ToString();
    }
}
