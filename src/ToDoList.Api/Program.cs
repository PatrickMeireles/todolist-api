using Microsoft.EntityFrameworkCore;
using Quartz;
using ToDoList.Api.Data;
using ToDoList.Api.Repository;
using ToDoList.Api.Services;
using ToDoList.Api.UseCase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IActivityUseCase, ActivityUseCase>();
builder.Services.AddScoped<IOutboxUseCase, OutboxUseCase>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();

var connectionString = builder.Configuration.GetConnectionString("DB");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new ArgumentException("String Connection to use Postgre was not found.");

builder.Services.AddDbContext<EntityFrameworkContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddQuartz(options =>
{
    var tasksDelaysMinutes = int.TryParse(builder.Configuration["Operations:TasksDelaysMinutes"], out int resultTaskDelay) ? resultTaskDelay : 10;
    var outboxProcessMinutes = int.TryParse(builder.Configuration["Operations:outboxProcessMinutes"], out int resultOutboxProcess) ? resultOutboxProcess : 10;

    var jobTaskDelays = JobKey.Create(nameof(TasksDelaysBackgroundServices));

    options.AddJob<TasksDelaysBackgroundServices>(jobTaskDelays)
        .AddTrigger(trigger => trigger.ForJob(jobTaskDelays)
                    .WithSimpleSchedule(schedules => schedules.WithIntervalInMinutes(tasksDelaysMinutes).RepeatForever()));

    var jobOutboxProcess = JobKey.Create(nameof(OutboxProcessBackgroundServices));

    options.AddJob<OutboxProcessBackgroundServices>(jobOutboxProcess)
        .AddTrigger(trigger => trigger.ForJob(jobOutboxProcess)
                    .WithSimpleSchedule(schedules => schedules.WithIntervalInMinutes(outboxProcessMinutes).RepeatForever()));
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.UseHttpsRedirection();

app.Run();
