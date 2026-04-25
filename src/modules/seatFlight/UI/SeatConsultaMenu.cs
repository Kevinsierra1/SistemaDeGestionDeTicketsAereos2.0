using SistemaDeGestionDeTicketsAereos.src.modules.flight.Application.UseCases;
using SistemaDeGestionDeTicketsAereos.src.modules.flight.Infrastructure.Repositories;
using SistemaDeGestionDeTicketsAereos.src.modules.seat.Application.UseCases;
using SistemaDeGestionDeTicketsAereos.src.modules.seat.Infrastructure.Repositories;
using SistemaDeGestionDeTicketsAereos.src.modules.seatClass.Application.UseCases;
using SistemaDeGestionDeTicketsAereos.src.modules.seatClass.Infrastructure.Repositories;
using SistemaDeGestionDeTicketsAereos.src.modules.seatFlight.Application.UseCases;
using SistemaDeGestionDeTicketsAereos.src.modules.seatFlight.Domain.valueObject;
using SistemaDeGestionDeTicketsAereos.src.modules.seatFlight.Infrastructure.Repositories;
using SistemaDeGestionDeTicketsAereos.src.shared.helpers;
using Spectre.Console;

namespace SistemaDeGestionDeTicketsAereos.src.modules.seatFlight.UI;

public sealed class SeatConsultaMenu
{
    public async Task RunAsync(CancellationToken ct = default)
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[cyan]GESTIÓN DE ASIENTOS POR VUELO[/]").Centered());
            AnsiConsole.MarkupLine("[grey]Consulta y administración del mapa de asientos de cada vuelo.[/]\n");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Seleccione una opción:")
                    .PageSize(8)
                    .HighlightStyle(new Style(foreground: Color.Cyan1, background: Color.Black))
                    .AddChoices(new[]
                    {
                        "1. Ver asientos por vuelo",
                        "2. Consultar disponibilidad por clase",
                        "3. Ver asientos ocupados",
                        "4. Cambiar estado de un asiento",
                        "0. Volver"
                    }));

            switch (option)
            {
                case "1. Ver asientos por vuelo":
                    await VerAsientosPorVueloAsync(ct);
                    break;
                case "2. Consultar disponibilidad por clase":
                    await ConsultarDisponibilidadPorClaseAsync(ct);
                    break;
                case "3. Ver asientos ocupados":
                    await VerAsientosOcupadosAsync(ct);
                    break;
                case "4. Cambiar estado de un asiento":
                    await CambiarEstadoAsientoAsync(ct);
                    break;
                case "0. Volver":
                    return;
            }

            AnsiConsole.MarkupLine("\n[grey]Presione cualquier tecla para continuar...[/]");
            Console.ReadKey(true);
        }
    }

    private static async Task VerAsientosPorVueloAsync(CancellationToken ct)
    {
        Console.Clear();
        AnsiConsole.Write(new Rule("[cyan]Ver asientos por vuelo[/]").Centered());
        using var context = DbContextFactory.Create();

        var idFlight = AnsiConsole.Prompt(
            new TextPrompt<int>("Ingrese el [bold]ID del vuelo[/]:")
                .Validate(v => v > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]El ID debe ser mayor a 0.[/]")));

        var flight = await new GetFlightByIdUseCase(new FlightRepository(context)).ExecuteAsync(idFlight, ct);
        if (flight is null) { AnsiConsole.MarkupLine("[red]Vuelo no encontrado.[/]"); return; }

        AnsiConsole.MarkupLine($"\n[bold]Vuelo:[/] {Markup.Escape(flight.Number.Value)} — {flight.Date.Value:yyyy-MM-dd}\n");

        var allSeats    = await new GetAllSeatsUseCase(new SeatRepository(context)).ExecuteAsync(ct);
        var seatFlights = await new GetAllSeatFlightsUseCase(new SeatFlightRepository(context)).ExecuteAsync(ct);
        var seatClasses = await new GetAllSeatClassesUseCase(new SeatClassRepository(context)).ExecuteAsync(ct);

        var classNames = seatClasses.ToDictionary(sc => sc.Id.Value, sc => sc.Name.Value);
        var sfByFlight = seatFlights.Where(sf => sf.IdFlight == idFlight).ToList();

        if (!sfByFlight.Any()) { AnsiConsole.MarkupLine("[yellow]Este vuelo no tiene asientos generados.[/]"); return; }

        var seatById = allSeats.ToDictionary(s => s.Id.Value);

        foreach (var group in sfByFlight.Where(sf => seatById.ContainsKey(sf.IdSeat))
                     .GroupBy(sf => seatById[sf.IdSeat].IdClase).OrderBy(g => g.Key))
        {
            var className = classNames.TryGetValue(group.Key, out var cn) ? cn : $"Clase {group.Key}";
            AnsiConsole.Write(new Rule($"[bold]{Markup.Escape(className)}[/]").LeftJustified());

            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("Asiento").AddColumn("Estado").AddColumn("Disponible");

            foreach (var sf in group.OrderBy(sf => seatById[sf.IdSeat].Number.Value))
            {
                var seat = seatById[sf.IdSeat];
                var estadoLabel = sf.Status switch
                {
                    SeatFlightStatus.Disponible => "[green]Disponible[/]",
                    SeatFlightStatus.Reservado  => "[yellow]Reservado[/]",
                    SeatFlightStatus.Ocupado    => "[red]Ocupado[/]",
                    SeatFlightStatus.Bloqueado  => "[grey]Bloqueado[/]",
                    _                          => sf.Status.ToString()
                };
                table.AddRow(Markup.Escape(seat.Number.Value), estadoLabel, sf.Available ? "[green]Sí[/]" : "[red]No[/]");
            }
            AnsiConsole.Write(table);
        }

        var total       = sfByFlight.Count;
        var disponibles = sfByFlight.Count(sf => sf.Status == SeatFlightStatus.Disponible);
        var reservados  = sfByFlight.Count(sf => sf.Status == SeatFlightStatus.Reservado);
        var ocupados    = sfByFlight.Count(sf => sf.Status == SeatFlightStatus.Ocupado);
        var bloqueados  = sfByFlight.Count(sf => sf.Status == SeatFlightStatus.Bloqueado);
        AnsiConsole.MarkupLine($"\n[bold]Resumen:[/] Total: {total} | [green]Disponibles: {disponibles}[/] | [yellow]Reservados: {reservados}[/] | [red]Ocupados: {ocupados}[/] | [grey]Bloqueados: {bloqueados}[/]");
    }

    private static async Task ConsultarDisponibilidadPorClaseAsync(CancellationToken ct)
    {
        Console.Clear();
        AnsiConsole.Write(new Rule("[cyan]Disponibilidad por clase[/]").Centered());
        using var context = DbContextFactory.Create();

        var idFlight = AnsiConsole.Prompt(
            new TextPrompt<int>("Ingrese el [bold]ID del vuelo[/]:")
                .Validate(v => v > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]El ID debe ser mayor a 0.[/]")));

        var flight = await new GetFlightByIdUseCase(new FlightRepository(context)).ExecuteAsync(idFlight, ct);
        if (flight is null) { AnsiConsole.MarkupLine("[red]Vuelo no encontrado.[/]"); return; }

        AnsiConsole.MarkupLine($"\n[bold]Vuelo:[/] {Markup.Escape(flight.Number.Value)} — {flight.Date.Value:yyyy-MM-dd}\n");

        var allSeats    = await new GetAllSeatsUseCase(new SeatRepository(context)).ExecuteAsync(ct);
        var seatFlights = await new GetAllSeatFlightsUseCase(new SeatFlightRepository(context)).ExecuteAsync(ct);
        var seatClasses = await new GetAllSeatClassesUseCase(new SeatClassRepository(context)).ExecuteAsync(ct);

        var classNames = seatClasses.ToDictionary(sc => sc.Id.Value, sc => sc.Name.Value);
        var seatById   = allSeats.ToDictionary(s => s.Id.Value);
        var sfByFlight = seatFlights.Where(sf => sf.IdFlight == idFlight && seatById.ContainsKey(sf.IdSeat)).ToList();

        if (!sfByFlight.Any()) { AnsiConsole.MarkupLine("[yellow]Este vuelo no tiene asientos generados.[/]"); return; }

        var table = new Table().Border(TableBorder.Rounded)
            .Title("[bold]Disponibilidad por clase[/]")
            .AddColumn("Clase").AddColumn("Total").AddColumn("Disponibles")
            .AddColumn("Reservados").AddColumn("Ocupados").AddColumn("Bloqueados").AddColumn("% Ocupación");

        foreach (var group in sfByFlight.GroupBy(sf => seatById[sf.IdSeat].IdClase).OrderBy(g => g.Key))
        {
            var className = classNames.TryGetValue(group.Key, out var cn) ? cn : $"Clase {group.Key}";
            var total  = group.Count();
            var disp   = group.Count(sf => sf.Status == SeatFlightStatus.Disponible);
            var reserv = group.Count(sf => sf.Status == SeatFlightStatus.Reservado);
            var ocup   = group.Count(sf => sf.Status == SeatFlightStatus.Ocupado);
            var bloq   = group.Count(sf => sf.Status == SeatFlightStatus.Bloqueado);
            var pct    = total > 0 ? (double)(reserv + ocup) / total * 100 : 0;
            table.AddRow(Markup.Escape(className), total.ToString(),
                $"[green]{disp}[/]", $"[yellow]{reserv}[/]",
                $"[red]{ocup}[/]", $"[grey]{bloq}[/]", $"{pct:F1}%");
        }
        AnsiConsole.Write(table);

        var totalAll = sfByFlight.Count;
        var ocupAll  = sfByFlight.Count(sf => sf.Status == SeatFlightStatus.Ocupado || sf.Status == SeatFlightStatus.Reservado);
        var pctGen   = totalAll > 0 ? (double)ocupAll / totalAll * 100 : 0;
        AnsiConsole.MarkupLine($"\n[bold]Total general:[/] {totalAll} | {ocupAll} reservados/ocupados | [bold]{pctGen:F1}% de ocupación[/]");
    }

    private static async Task VerAsientosOcupadosAsync(CancellationToken ct)
    {
        Console.Clear();
        AnsiConsole.Write(new Rule("[cyan]Asientos ocupados / reservados[/]").Centered());
        using var context = DbContextFactory.Create();

        var idFlight = AnsiConsole.Prompt(
            new TextPrompt<int>("Ingrese el [bold]ID del vuelo[/]:")
                .Validate(v => v > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]El ID debe ser mayor a 0.[/]")));

        var flight = await new GetFlightByIdUseCase(new FlightRepository(context)).ExecuteAsync(idFlight, ct);
        if (flight is null) { AnsiConsole.MarkupLine("[red]Vuelo no encontrado.[/]"); return; }

        AnsiConsole.MarkupLine($"\n[bold]Vuelo:[/] {Markup.Escape(flight.Number.Value)} — {flight.Date.Value:yyyy-MM-dd}\n");

        var allSeats    = await new GetAllSeatsUseCase(new SeatRepository(context)).ExecuteAsync(ct);
        var seatFlights = await new GetAllSeatFlightsUseCase(new SeatFlightRepository(context)).ExecuteAsync(ct);
        var seatClasses = await new GetAllSeatClassesUseCase(new SeatClassRepository(context)).ExecuteAsync(ct);

        var classNames = seatClasses.ToDictionary(sc => sc.Id.Value, sc => sc.Name.Value);
        var seatById   = allSeats.ToDictionary(s => s.Id.Value);

        var noDisponibles = seatFlights
            .Where(sf => sf.IdFlight == idFlight && sf.Status != SeatFlightStatus.Disponible && seatById.ContainsKey(sf.IdSeat))
            .OrderBy(sf => seatById[sf.IdSeat].IdClase)
            .ThenBy(sf => seatById[sf.IdSeat].Number.Value)
            .ToList();

        if (!noDisponibles.Any()) { AnsiConsole.MarkupLine("[green]No hay asientos ocupados ni reservados en este vuelo.[/]"); return; }

        var table = new Table().Border(TableBorder.Rounded)
            .Title("[bold]Asientos no disponibles[/]")
            .AddColumn("Asiento").AddColumn("Clase").AddColumn("Estado");

        foreach (var sf in noDisponibles)
        {
            var seat      = seatById[sf.IdSeat];
            var className = classNames.TryGetValue(seat.IdClase, out var cn) ? cn : $"Clase {seat.IdClase}";
            var estadoLabel = sf.Status switch
            {
                SeatFlightStatus.Reservado => "[yellow]Reservado[/]",
                SeatFlightStatus.Ocupado   => "[red]Ocupado[/]",
                SeatFlightStatus.Bloqueado => "[grey]Bloqueado[/]",
                _                         => sf.Status.ToString()
            };
            table.AddRow(Markup.Escape(seat.Number.Value), Markup.Escape(className), estadoLabel);
        }
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"\n[bold]Total:[/] {noDisponibles.Count} asiento(s) no disponible(s)");
    }

    private static async Task CambiarEstadoAsientoAsync(CancellationToken ct)
    {
        Console.Clear();
        AnsiConsole.Write(new Rule("[cyan]Cambiar estado de asiento[/]").Centered());
        AnsiConsole.MarkupLine("[grey]Permite al administrador cambiar manualmente el estado de un asiento.[/]\n");
        using var context = DbContextFactory.Create();

        var idFlight = AnsiConsole.Prompt(
            new TextPrompt<int>("Ingrese el [bold]ID del vuelo[/]:")
                .Validate(v => v > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]El ID debe ser mayor a 0.[/]")));

        var flight = await new GetFlightByIdUseCase(new FlightRepository(context)).ExecuteAsync(idFlight, ct);
        if (flight is null) { AnsiConsole.MarkupLine("[red]Vuelo no encontrado.[/]"); return; }

        var allSeats    = await new GetAllSeatsUseCase(new SeatRepository(context)).ExecuteAsync(ct);
        var seatFlights = await new GetAllSeatFlightsUseCase(new SeatFlightRepository(context)).ExecuteAsync(ct);
        var seatClasses = await new GetAllSeatClassesUseCase(new SeatClassRepository(context)).ExecuteAsync(ct);

        var classNames = seatClasses.ToDictionary(sc => sc.Id.Value, sc => sc.Name.Value);
        var seatById   = allSeats.ToDictionary(s => s.Id.Value);
        var sfByFlight = seatFlights
            .Where(sf => sf.IdFlight == idFlight && seatById.ContainsKey(sf.IdSeat))
            .OrderBy(sf => seatById[sf.IdSeat].Number.Value)
            .ToList();

        if (!sfByFlight.Any()) { AnsiConsole.MarkupLine("[yellow]Este vuelo no tiene asientos generados.[/]"); return; }

        var seatOptions = sfByFlight.Select(sf =>
        {
            var seat      = seatById[sf.IdSeat];
            var className = classNames.TryGetValue(seat.IdClase, out var cn) ? cn : $"Clase {seat.IdClase}";
            return $"{seat.Number.Value} | {className} | {sf.Status}";
        }).ToList();

        var selectedLabel = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione el [bold]asiento[/]:")
                .PageSize(12)
                .AddChoices(seatOptions));

        var idx        = seatOptions.IndexOf(selectedLabel);
        var selectedSf = sfByFlight[idx];

        var nuevoEstado = AnsiConsole.Prompt(
            new SelectionPrompt<SeatFlightStatus>()
                .Title("Seleccione el [bold]nuevo estado[/]:")
                .AddChoices(Enum.GetValues<SeatFlightStatus>()));

        var repo = new SeatFlightRepository(context);
        await new UpdateSeatFlightUseCase(repo).ExecuteAsync(
            selectedSf.Id.Value, selectedSf.IdSeat, selectedSf.IdFlight, nuevoEstado, ct);

        await context.SaveChangesAsync(ct);

        var seatNumber = seatById[selectedSf.IdSeat].Number.Value;
        AnsiConsole.MarkupLine($"\n[green]Asiento [bold]{Markup.Escape(seatNumber)}[/] actualizado a [bold]{nuevoEstado}[/] correctamente.[/]");
    }
}