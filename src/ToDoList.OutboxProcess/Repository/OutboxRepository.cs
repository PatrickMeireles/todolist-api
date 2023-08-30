using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.OutboxProcess.Model;

namespace ToDoList.OutboxProcess.Repository;

public class OutboxRepository
{
    private const string TABLE_NAME = "outboxes";
    private readonly NpgsqlConnection connection;

    public OutboxRepository(string connection_string)
    {
        connection = new NpgsqlConnection(connection_string);
    }

    public async Task<IEnumerable<Outbox>> GetOutboxes()
    {
        connection.Open();

        var query = $"SELECT  * FROM {TABLE_NAME} WHERE \"Published\" = false AND \"PublishedAt\" IS NULL LIMIT 25";

        var result = new List<Outbox>();

        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = connection;

            cmd.CommandText = query;

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var outbox = new Outbox();

                outbox.Id = Guid.Parse(reader["Id"].ToString());
                outbox.CreatedAt = DateTime.TryParse(reader["CreatedAt"].ToString(), out DateTime createdAtResult) ? createdAtResult : default;
                outbox.Event = reader["Event"].ToString();
                outbox.Type = reader["Type"].ToString();
                outbox.UpdatedAt = DateTime.TryParse(reader["UpdatedAt"].ToString(), out DateTime updatedAtResult) ? updatedAtResult : null;
                outbox.PublishedAt = DateTime.TryParse(reader["PublishedAt"].ToString(), out DateTime PublishedAtResult) ? PublishedAtResult : null;
                result.Add(outbox);
            }
        }

        await connection.CloseAsync();

        return result;
    }

    public async Task SetPublished(List<Guid> ids)
    {
        if (ids.Count == 0)
            return;

        connection.Open();

        using (var cmd = new NpgsqlCommand())
        {

            var values = string.Join(',', ids.Select(c => $"'{c}'"));

            var query = $"UPDATE {TABLE_NAME} SET \"Published\"  = TRUE, \"PublishedAt\" = now(), \"UpdatedAt\" = now()  WHERE \"Id\" in ({values})";

            cmd.Connection = connection;
            cmd.CommandText = query;

            await cmd.ExecuteNonQueryAsync();
        }

        await connection.CloseAsync();
    }

    public async Task DisposeAsync()
    {
        await connection.DisposeAsync();
    }
}
