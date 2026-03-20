
namespace usuario.API.Models;

public record Usuario(
    int Id,
    string Cedula,
    string Nombre,
    string Apellidos,
    DateTime FechaNacimiento,
    string Telefono,
    string Direccion
)
{
        public int Edad => DateTime.Today.Year - FechaNacimiento.Year -
            (DateTime.Today < FechaNacimiento.AddYears(DateTime.Today.Year - FechaNacimiento.Year) ? 1 : 0);
}
public record Login(string Nombre, string Cedula);