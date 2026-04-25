// La asignación asiento-vuelo controla qué asientos están disponibles para un vuelo en particular
using SistemaDeGestionDeTicketsAereos.src.modules.seatFlight.Domain.valueObject;

namespace SistemaDeGestionDeTicketsAereos.src.modules.seatFlight.Domain.aggregate;

// Agregado SeatFlight: encapsula la relación entre un asiento y un vuelo específico
public class SeatFlight
{
    // ID de la asignación (Value Object)
    public SeatFlightId Id { get; private set; }

    // FK al asiento de la aeronave
    public int IdSeat { get; private set; }

    // FK al vuelo en que opera ese asiento
    public int IdFlight { get; private set; }

    // Estado actual del asiento en el vuelo
    public SeatFlightStatus Status { get; private set; }

    // Compatibilidad con la lógica previa basada en bool.
    public bool Available => Status == SeatFlightStatus.Disponible;

    // Constructor privado: solo se crea a través del método Create
    private SeatFlight(SeatFlightId id, int idSeat, int idFlight, SeatFlightStatus status)
    {
        Id = id;
        IdSeat = idSeat;
        IdFlight = idFlight;
        Status = status;
    }

    // Método de fábrica para crear o reconstruir una asignación asiento-vuelo desde la base de datos
    public static SeatFlight Create(int id, int idSeat, int idFlight, SeatFlightStatus status)
    {
        // Regla: el asiento asociado debe ser una referencia válida
        if (idSeat <= 0)
            throw new ArgumentException("IdSeat must be greater than 0.", nameof(idSeat));

        // Regla: el vuelo asociado debe ser una referencia válida
        if (idFlight <= 0)
            throw new ArgumentException("IdFlight must be greater than 0.", nameof(idFlight));

        return new SeatFlight(
            SeatFlightId.Create(id),
            idSeat,
            idFlight,
            status
        );
    }

    // Sobrecargas para mantener compatibilidad con el flujo existente.
    public static SeatFlight Create(int id, int idSeat, int idFlight, bool available) =>
        Create(id, idSeat, idFlight, available ? SeatFlightStatus.Disponible : SeatFlightStatus.Reservado);

    // Método de fábrica para crear una asignación nueva (ID = 0, la BD lo asigna después)
    public static SeatFlight CreateNew(int idSeat, int idFlight, SeatFlightStatus status) => Create(0, idSeat, idFlight, status);
    public static SeatFlight CreateNew(int idSeat, int idFlight, bool available) => Create(0, idSeat, idFlight, available);
}
