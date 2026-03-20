namespace usuario.API.Data;
using usuario.API.Models;
public interface IUsuarioRepository
{
    List<Usuario> GetAll();
    void Add(Usuario usuario);
}
public class InMemoryUsuarioRepository : IUsuarioRepository
{
  
  //use una lista que me sirviese de base de datos para mas flexibilidad 
    private readonly List<Usuario> _usuarios = new() {
        new Usuario(1, "1234", "manolo", "zapata", new DateTime(1999, 12, 10), "3502657845", "calle 24c #34-33"),
        new Usuario(2,"12345","martina","arango",new DateTime(2004, 07, 10),"3502657845", "calle 24c #34-33")
    };

    public List<Usuario> GetAll() => _usuarios;
    public void Add(Usuario usuario) => _usuarios.Add(usuario);
}

