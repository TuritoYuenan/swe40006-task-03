var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
	config.DocumentName = "CalculatorAPI";
	config.Title = "CalculatorAPI v1";
	config.Version = "v1";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseOpenApi();
	app.UseSwaggerUi(config =>
	{
		config.DocumentTitle = "CalculatorAPI";
		config.Path = "/swagger";
		config.DocumentPath = "/swagger/{documentName}/swagger.json";
		config.DocExpansion = "list";
	});
}

app.UseHttpsRedirection();

// Home endpoint
app.MapGet("/", () => "Welcome to the Calculator API! Use the endpoints to perform various calculations.").WithName("Home");

// Basic arithmetic operations
app.MapGet("/add", (double a, double b) => new CalculationResult($"{a} + {b}", a + b)).WithName("Add");
app.MapGet("/subtract", (double a, double b) => new CalculationResult($"{a} - {b}", a - b)).WithName("Subtract");
app.MapGet("/multiply", (double a, double b) => new CalculationResult($"{a} * {b}", a * b)).WithName("Multiply");
app.MapGet("/divide", (double a, double b) => b != 0 ? new CalculationResult($"{a} / {b}", a / b) : throw new ArgumentException("Division by zero is not allowed.")).WithName("Divide");
app.MapGet("/power", (double a, double b) => new CalculationResult($"{a} ^ {b}", Math.Pow(a, b))).WithName("Power");

// Random number generator
app.MapGet("/random", () => new CalculationResult("Random", new Random().NextDouble())).WithName("Random");

// Trigonometric functions
app.MapGet("/trig/sin", (double angle) => new CalculationResult($"sin({angle})", Math.Sin(angle))).WithName("Sine");
app.MapGet("/trig/cos", (double angle) => new CalculationResult($"cos({angle})", Math.Cos(angle))).WithName("Cosine");
app.MapGet("/trig/tan", (double angle) => new CalculationResult($"tan({angle})", Math.Tan(angle))).WithName("Tangent");

// Given n, sum of all natural numbers up to n
app.MapGet("/sum", (int n) => new CalculationResult($"Sum of numbers from 1 to {n}", n * (n + 1) / 2)).WithName("Sum");

app.Run();

record CalculationResult(string Operation, double Result);
