using Haiku.API.Dtos;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class CustomSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(CreatorDto))
        {
            if (schema.Properties.ContainsKey("name"))
            {
                schema.Properties["name"].Example = new OpenApiString("John Doe");
                schema.Properties["name"].Description = "The Author name.";
            }
            if (schema.Properties.ContainsKey("bio"))
            {
                schema.Properties["bio"].Default = new OpenApiString("No Bio.");
                schema.Properties["bio"].Example = new OpenApiString("An accomplished poet.");
                schema.Properties["bio"].Description = "The bio of the Author. Defaults to 'No Bio'.";
            }
            if (schema.Properties.ContainsKey("id"))
            {
                  schema.Properties["id"].ReadOnly = true;
            }
        }

        if (context.Type == typeof(HaikuDto))
        {
            if (schema.Properties.ContainsKey("title"))
            {
                schema.Properties["title"].Example = new OpenApiString("An Old Silent Pond");
                schema.Properties["title"].Description = "The title of the Haiku.";
            }
            if (schema.Properties.ContainsKey("lineOne"))
            {
                schema.Properties["lineOne"].Example = new OpenApiString("An old silent pond...");
                schema.Properties["lineOne"].Description = "The first line is 5 Syllables.";
            }
            if (schema.Properties.ContainsKey("lineTwo"))
            {
                schema.Properties["lineTwo"].Example = new OpenApiString("A frog jumps into the pond,");
                schema.Properties["lineTwo"].Description = "The Second line is 7 Syllables.";
            }
            if (schema.Properties.ContainsKey("lineThree"))
            {
                schema.Properties["lineThree"].Example = new OpenApiString("splash! Silence again.");
                schema.Properties["lineThree"].Description = "The Third line is 5 Syllables.";
            }
            if (schema.Properties.ContainsKey("creatorId"))
            {
                schema.Properties["creatorId"].Default = new OpenApiLong(1);
                schema.Properties["creatorId"].Example = new OpenApiLong(1);
                schema.Properties["creatorId"].Description = "The ID of the Haiku's author. Defaults to 1, which represents an 'Unknown Author'.";
            }
            if (schema.Properties.ContainsKey("id"))
            {
                schema.Properties["id"].ReadOnly = true;
            }
        }

        if (context.Type == typeof(UserDto))
        {
            if (schema.Properties.ContainsKey("username"))
            {
                schema.Properties["username"].Example = new OpenApiString("user23211");
                schema.Properties["username"].Description = "The users username.";
            }
            if (schema.Properties.ContainsKey("password"))
            {
                schema.Properties["password"].Example = new OpenApiString("a2vfa2f");
                schema.Properties["password"].Description = "The users password.";
            }
            if (schema.Properties.ContainsKey("id"))
            {
                schema.Properties["id"].ReadOnly = true;
            }
        }
    }
}