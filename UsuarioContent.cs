using reservas_vuelos.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace reservas_vuelos.DAL
{
    public class UsuarioContext : DbContext
    {

        public UsuarioContext() : base("UsuarioContext")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}