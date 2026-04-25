using SistemaDeGestionDeTicketsAereos.src.modules.seatFlight.Domain.aggregate;
using SistemaDeGestionDeTicketsAereos.src.modules.seatFlight.Domain.Repositories;
using SistemaDeGestionDeTicketsAereos.src.modules.seatFlight.Domain.valueObject;

namespace SistemaDeGestionDeTicketsAereos.src.modules.seatFlight.Application.UseCases;

public sealed class CreateSeatFlightUseCase
{
    private readonly ISeatFlightRepository _repo;
    public CreateSeatFlightUseCase(ISeatFlightRepository repo) => _repo = repo;

    public async Task<SeatFlight> ExecuteAsync(int idSeat, int idFlight, bool available, CancellationToken ct = default)
    {
        var status = available ? SeatFlightStatus.Disponible : SeatFlightStatus.Reservado;
        return await ExecuteAsync(idSeat, idFlight, status, ct);
    }

    public async Task<SeatFlight> ExecuteAsync(int idSeat, int idFlight, SeatFlightStatus status, CancellationToken ct = default)
    {
        var existing = await _repo.GetBySeatAndFlightAsync(idSeat, idFlight, ct);
        if (existing is not null) throw new InvalidOperationException($"SeatFlight for seat '{idSeat}' and flight '{idFlight}' already exists.");
        var entity = SeatFlight.CreateNew(idSeat, idFlight, status);
        await _repo.AddAsync(entity, ct);
        return entity;
    }
}
