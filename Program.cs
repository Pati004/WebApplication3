using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json.Serialization;

namespace SmucarskiKlub
{
    public class Smucar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(100)]
        public string Priimek { get; set; }

        [Required]
        public int LetoRojstva { get; set; }

        [Required]
        [MaxLength(100)]
        public string Drzava { get; set; }

        [JsonIgnore]
        public ICollection<SmucarVKlubu> ClanstvaVKlubih { get; set; } = new List<SmucarVKlubu>();

        public Smucar() { }

        public Smucar(string ime, string priimek, int letoRojstva, string drzava)
        {
            Ime = ime;
            Priimek = priimek;
            LetoRojstva = letoRojstva;
            Drzava = drzava;
        }
    }

    public class Klub
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(100)]
        public string Mesto { get; set; }

        [Required]
        public int SteviloSmucarjev { get; set; }

        [JsonIgnore]
        public ICollection<SmucarVKlubu> Clanstva { get; set; } = new List<SmucarVKlubu>();

        public Klub() { }

        public Klub(string ime, string mesto, int steviloSmucarjev)
        {
            Ime = ime;
            Mesto = mesto;
            SteviloSmucarjev = steviloSmucarjev;
        }
    }

    public class SmucarVKlubu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SmucarId { get; set; }

        [Required]
        public int KlubId { get; set; }

        [Required]
        public int OdLeta { get; set; }

        [Required]
        public int DoLeta { get; set; }

        [Required]
        public int Tekmovanja { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Smucar Smucar { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Klub Klub { get; set; }

        public SmucarVKlubu() { }

        public SmucarVKlubu(int smucarId, int klubId, int odLeta, int doLeta, int tekmovanja)
        {
            SmucarId = smucarId;
            KlubId = klubId;
            OdLeta = odLeta;
            DoLeta = doLeta;
            Tekmovanja = tekmovanja;
        }
    }

    public class SmucarskiKlubDbContext : DbContext
    {
        public DbSet<Smucar> Smucarji { get; set; }
        public DbSet<Klub> Klubi { get; set; }
        public DbSet<SmucarVKlubu> SmucarjiVKlubih { get; set; }

        public SmucarskiKlubDbContext(DbContextOptions<SmucarskiKlubDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmucarVKlubu>()
                .HasOne(s => s.Smucar)
                .WithMany(s => s.ClanstvaVKlubih)
                .HasForeignKey(s => s.SmucarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SmucarVKlubu>()
                .HasOne(s => s.Klub)
                .WithMany(k => k.Clanstva)
                .HasForeignKey(s => s.KlubId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Smucar>().HasData(
                new Smucar { Id = 1, Ime = "Blaz", Priimek = "Jurkovic", LetoRojstva = 2010, Drzava = "Nemcija" },
                new Smucar { Id = 2, Ime = "Leo", Priimek = "Puncec", LetoRojstva = 2000, Drzava = "Austrija" },
                new Smucar { Id = 3, Ime = "Ivan", Priimek = "Borovickic", LetoRojstva = 1918, Drzava = "Hrvaska" },
                new Smucar { Id = 4, Ime = "Denis", Priimek = "Krajnc", LetoRojstva = 1960, Drzava = "Slovenija" }
            );

            modelBuilder.Entity<Klub>().HasData(
                new Klub { Id = 1, Ime = "Triglav", Mesto = "Krajn", SteviloSmucarjev = 14 },
                new Klub { Id = 2, Ime = "Everest", Mesto = "Zagreb", SteviloSmucarjev = 3 },
                new Klub { Id = 3, Ime = "Mountain", Mesto = "Rim", SteviloSmucarjev = 26 }
            );

            modelBuilder.Entity<SmucarVKlubu>().HasData(
                new SmucarVKlubu { Id = 1, SmucarId = 1, KlubId = 1, OdLeta = 2021, DoLeta = 2024, Tekmovanja = 1 },
                new SmucarVKlubu { Id = 2, SmucarId = 1, KlubId = 2, OdLeta = 2018, DoLeta = 2020, Tekmovanja = 42 },
                new SmucarVKlubu { Id = 3, SmucarId = 2, KlubId = 1, OdLeta = 2019, DoLeta = 2024, Tekmovanja = 24 },
                new SmucarVKlubu { Id = 4, SmucarId = 3, KlubId = 1, OdLeta = 2023, DoLeta = 2024, Tekmovanja = 24 },
                new SmucarVKlubu { Id = 5, SmucarId = 3, KlubId = 3, OdLeta = 2007, DoLeta = 2021, Tekmovanja = 6350 },
                new SmucarVKlubu { Id = 6, SmucarId = 4, KlubId = 3, OdLeta = 2020, DoLeta = 2024, Tekmovanja = 23 }
            );
        }
    }


    public interface ISmucarskiKlubService
    {

        Task<List<Smucar>> GetSmucarjiAsync(string? ime = null, string? priimek = null);
        Task<Smucar?> GetSmucarAsync(int id);
        Task<Smucar> AddSmucarAsync(Smucar smucar);
        Task<bool> UpdateSmucarAsync(int id, Smucar smucar);
        Task<bool> DeleteSmucarAsync(int id);


        Task<List<Klub>> GetKlubiAsync(string? ime = null);
        Task<Klub?> GetKlubAsync(int id);
        Task<Klub> AddKlubAsync(Klub klub);
        Task<bool> UpdateKlubAsync(int id, Klub klub);
        Task<bool> DeleteKlubAsync(int id);


        Task<List<SmucarVKlubu>> GetSmucarjiVKlubuAsync();
        Task<SmucarVKlubu?> GetSmucarVKlubuAsync(int id);
        Task<SmucarVKlubu> AddSmucarVKlubuAsync(SmucarVKlubu smucarVKlubu);
        Task<bool> UpdateSmucarVKlubuAsync(int id, SmucarVKlubu smucarVKlubu);
        Task<bool> DeleteSmucarVKlubuAsync(int id);

        Task<List<Smucar>> GetSmucarjiByKlubAsync(string imeKluba);
        Task<List<Klub>> GetKlubiBySmucarAsync(string ime, string priimek);
        Task<Klub> GetKlubWithMostSkiersAsync();
        Task<Smucar> GetOldestSkierAsync();
        Task<double> GetAverageAgeAsync();
    }

    public class SmucarskiKlubService : ISmucarskiKlubService
    {
        private readonly SmucarskiKlubDbContext _context;

        public SmucarskiKlubService(SmucarskiKlubDbContext context)
        {
            _context = context;
        }
        public async Task<List<Smucar>> GetSmucarjiAsync(string? ime = null, string? priimek = null)
        {
            var query = _context.Smucarji.AsQueryable();

            if (!string.IsNullOrEmpty(ime))
                query = query.Where(s => s.Ime.Contains(ime));

            if (!string.IsNullOrEmpty(priimek))
                query = query.Where(s => s.Priimek.Contains(priimek));

            return await query.ToListAsync();
        }

        public async Task<Smucar?> GetSmucarAsync(int id)
        {
            return await _context.Smucarji.FindAsync(id);
        }

        public async Task<Smucar> AddSmucarAsync(Smucar smucar)
        {
            _context.Smucarji.Add(smucar);
            await _context.SaveChangesAsync();
            return smucar;
        }

        public async Task<bool> UpdateSmucarAsync(int id, Smucar smucar)
        {
            if (id != smucar.Id) return false;

            var existing = await _context.Smucarji.FindAsync(id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(smucar);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSmucarAsync(int id)
        {
            var smucar = await _context.Smucarji.FindAsync(id);
            if (smucar == null) return false;

            _context.Smucarji.Remove(smucar);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Klub>> GetKlubiAsync(string? ime = null)
        {
            var query = _context.Klubi.AsQueryable();

            if (!string.IsNullOrEmpty(ime))
                query = query.Where(k => k.Ime.Contains(ime));

            return await query.ToListAsync();
        }

        public async Task<Klub?> GetKlubAsync(int id)
        {
            return await _context.Klubi.FindAsync(id);
        }

        public async Task<Klub> AddKlubAsync(Klub klub)
        {
            _context.Klubi.Add(klub);
            await _context.SaveChangesAsync();
            return klub;
        }

        public async Task<bool> UpdateKlubAsync(int id, Klub klub)
        {
            if (id != klub.Id) return false;

            var existing = await _context.Klubi.FindAsync(id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(klub);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteKlubAsync(int id)
        {
            var klub = await _context.Klubi.FindAsync(id);
            if (klub == null) return false;

            _context.Klubi.Remove(klub);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<SmucarVKlubu>> GetSmucarjiVKlubuAsync()
        {
            return await _context.SmucarjiVKlubih
                .Include(s => s.Smucar)
                .Include(s => s.Klub)
                .ToListAsync();
        }
        public async Task<SmucarVKlubu?> GetSmucarVKlubuAsync(int id)
        {
            return await _context.SmucarjiVKlubih
                .Include(s => s.Smucar)
                .Include(s => s.Klub)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SmucarVKlubu> AddSmucarVKlubuAsync(SmucarVKlubu smucarVKlubu)
        {
            _context.SmucarjiVKlubih.Add(smucarVKlubu);
            await _context.SaveChangesAsync();
            return smucarVKlubu;
        }

        public async Task<bool> UpdateSmucarVKlubuAsync(int id, SmucarVKlubu smucarVKlubu)
        {
            if (id != smucarVKlubu.Id) return false;

            var existing = await _context.SmucarjiVKlubih.FindAsync(id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(smucarVKlubu);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSmucarVKlubuAsync(int id)
        {
            var smucarVKlubu = await _context.SmucarjiVKlubih.FindAsync(id);
            if (smucarVKlubu == null) return false;

            _context.SmucarjiVKlubih.Remove(smucarVKlubu);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Smucar>> GetSmucarjiByKlubAsync(string imeKluba)
        {
            return await _context.SmucarjiVKlubih
                .Include(s => s.Smucar)
                .Where(s => s.Klub.Ime == imeKluba)
                .Select(s => s.Smucar)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Klub>> GetKlubiBySmucarAsync(string ime, string priimek)
        {
            return await _context.SmucarjiVKlubih
                .Include(s => s.Klub)
                .Where(s => s.Smucar.Ime == ime && s.Smucar.Priimek == priimek)
                .Select(s => s.Klub)
                .Distinct()
                .ToListAsync();
        }

        public async Task<Klub> GetKlubWithMostSkiersAsync()
        {
            return await _context.SmucarjiVKlubih
                .GroupBy(s => s.Klub)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstAsync();
        }

        public async Task<Smucar> GetOldestSkierAsync()
        {
            return await _context.Smucarji
                .OrderBy(s => s.LetoRojstva)
                .FirstAsync();
        }

        public async Task<double> GetAverageAgeAsync()
        {
            int currentYear = DateTime.Now.Year;
            return await _context.Smucarji
                .AverageAsync(s => currentYear - s.LetoRojstva);
        }
    }

    public static class EndpointExtensions
    {
        public static void MapSmucarskiKlubEndpoints(this WebApplication app)
        {
            app.MapGet("/api/smucarji", async ([FromQuery] string? ime, [FromQuery] string? priimek, ISmucarskiKlubService service) =>
            {
                var smucarji = await service.GetSmucarjiAsync(ime, priimek);
                return Results.Ok(smucarji);
            })
            .WithName("GetSmucarji")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi smucarja",
                Description = "Dobi listo smucarjev po imenu ali priimku",
                Tags = new List<OpenApiTag> { new() { Name = "Smucarji" } }
            })
            .Produces<List<Smucar>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapGet("/api/smucarji/{id}", async (int id, ISmucarskiKlubService service) =>
            {
                var smucar = await service.GetSmucarAsync(id);
                if (smucar == null)
                    return Results.NotFound();
                return Results.Ok(smucar);
            })
            .WithName("GetSmucar")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi smucarja po id",
                Description = "Dobi smucarja po njegovem id ",
                Tags = new List<OpenApiTag> { new() { Name = "Smucarji" } }
            })
            .Produces<Smucar>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/api/smucarji", async (Smucar smucar, ISmucarskiKlubService service) =>
            {
                var newSmucar = await service.AddSmucarAsync(smucar);
                return Results.Created($"/api/smucarji/{newSmucar.Id}", newSmucar);
            })
            .WithName("CreateSmucar")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Vnesi novega smucarja",
                Description = "Vnese novega smucarja v bazo",
                Tags = new List<OpenApiTag> { new() { Name = "Smucarji" } }
            })
            .Produces<Smucar>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Accepts<Smucar>(MediaTypeNames.Application.Json);

            app.MapPut("/api/smucarji/{id}", async (int id, Smucar smucar, ISmucarskiKlubService service) =>
            {
                if (await service.UpdateSmucarAsync(id, smucar))
                    return Results.NoContent();
                return Results.NotFound();
            })
            .WithName("UpdateSmucar")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update smucar",
                Description = "Posodobi informacije ze obstojecega smucarja",
                Tags = new List<OpenApiTag> { new() { Name = "Smucarji" } }
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<Smucar>(MediaTypeNames.Application.Json);

            app.MapDelete("/api/smucarji/{id}", async (int id, ISmucarskiKlubService service) =>
            {
                if (await service.DeleteSmucarAsync(id))
                    return Results.NoContent();
                return Results.NotFound();
            })
            .WithName("DeleteSmucar")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Izbrisi smucarja",
                Description = "Zbrisi smucarja is baze",
                Tags = new List<OpenApiTag> { new() { Name = "Smucarji" } }
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
            app.MapGet("/api/klubi", async ([FromQuery] string? ime, ISmucarskiKlubService service) =>
            {
                var klubi = await service.GetKlubiAsync(ime);
                return Results.Ok(klubi);
            })
            .WithName("GetKlubi")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi klubi",
                Description = "Dobi listo klubov po imenu",
                Tags = new List<OpenApiTag> { new() { Name = "Klubi" } }
            })
            .Produces<List<Klub>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapGet("/api/klubi/{id}", async (int id, ISmucarskiKlubService service) => {
                var klub = await service.GetKlubAsync(id);
                if (klub == null)
                    return Results.NotFound();
                return Results.Ok(klub);
            })
            .WithName("GetKlub")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi klub po id",
                Description = "Dobi klub po njegovem id",
                Tags = new List<OpenApiTag> { new() { Name = "Klubi" } }
            })
            .Produces<Klub>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/api/klubi", async (Klub klub, ISmucarskiKlubService service) => {
                var newKlub = await service.AddKlubAsync(klub);
                return Results.Created($"/api/klubi/{newKlub.Id}", newKlub);
            })
            .WithName("CreateKlub")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Vnesi nov klub",
                Description = "Vnese nov klub v bazo",
                Tags = new List<OpenApiTag> { new() { Name = "Klubi" } }
            })
            .Produces<Klub>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Accepts<Klub>(MediaTypeNames.Application.Json);

            app.MapPut("/api/klubi/{id}", async (int id, Klub klub, ISmucarskiKlubService service) => {
                if (await service.UpdateKlubAsync(id, klub))
                    return Results.NoContent();
                return Results.NotFound();
            })
            .WithName("UpdateKlub")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update klub",
                Description = "Posodobi podatke kluba",
                Tags = new List<OpenApiTag> { new() { Name = "Klubi" } }
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<Klub>(MediaTypeNames.Application.Json);

            app.MapDelete("/api/klubi/{id}", async (int id, ISmucarskiKlubService service) => {
                if (await service.DeleteKlubAsync(id))
                    return Results.NoContent();
                return Results.NotFound();
            })
            .WithName("DeleteKlub")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Izbrisi klub",
                Description = "Izbrise klub is baze podatkov",
                Tags = new List<OpenApiTag> { new() { Name = "Klubi" } }
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            app.MapGet("/api/smucarji-v-klubu", async (ISmucarskiKlubService service) =>
                Results.Ok(await service.GetSmucarjiVKlubuAsync()))
            .WithName("GetSmucarjiVKlubu")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi clane kluba",
                Description = "Dobi listo vseh smucarjev ,ki so v klubih",
                Tags = new List<OpenApiTag> { new() { Name = "Clani kluba" } }
            })
            .Produces<List<SmucarVKlubu>>(StatusCodes.Status200OK);

            app.MapGet("/api/smucarji-v-klubu/{id}", async (int id, ISmucarskiKlubService service) => {
                var smucarVKlubu = await service.GetSmucarVKlubuAsync(id);
                if (smucarVKlubu == null)
                    return Results.NotFound();
                return Results.Ok(smucarVKlubu);
            })
            .WithName("GetSmucarVKlubu")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi clana kluba po id",
                Description = "Dobi smucarja v klubu po id",
                Tags = new List<OpenApiTag> { new() { Name = "Clani kluba" } }
            })
            .Produces<SmucarVKlubu>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapGet("/api/klubi/{imeKluba}/smucarji", async (string imeKluba, ISmucarskiKlubService service) =>
                Results.Ok(await service.GetSmucarjiByKlubAsync(imeKluba)))
            .WithName("GetSmucarjiByKlub")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi smucarje po klubu",
                Description = "Dobi listo vseh clanov nekega kluba",
                Tags = new List<OpenApiTag> { new() { Name = "Reports" } }
            })
            .Produces<List<Smucar>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapGet("/api/smucarji/{ime}/{priimek}/klubi", async (string ime, string priimek, ISmucarskiKlubService service) =>
                Results.Ok(await service.GetKlubiBySmucarAsync(ime, priimek)))
            .WithName("GetKlubiBySmucar")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi klube po smucarju",
                Description = "Dobi vse klube v katerih je bil dolocen smucar",
                Tags = new List<OpenApiTag> { new() { Name = "" } }
            })
            .Produces<List<Klub>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


            app.MapPost("/api/smucarji-v-klubu", async (SmucarVKlubu smucarVKlubu, ISmucarskiKlubService service) =>
            {
                try
                {
                    var newClanstvo = await service.AddSmucarVKlubuAsync(smucarVKlubu);
                    return Results.Created($"/api/smucarji-v-klubu/{newClanstvo.Id}", newClanstvo);
                }
                catch (Exception)
                {
                    return Results.BadRequest("Invalid skier or club ID");
                }
            })
            .WithName("CreateSmucarVKlubu")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Dodaj smucarja v klub",
                Description = "Doda novega smucarja v klub",
                Tags = new List<OpenApiTag> { new() { Name = "Clani kluba" } }
            })
            .Produces<SmucarVKlubu>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Accepts<SmucarVKlubu>(MediaTypeNames.Application.Json);

            app.MapPut("/api/smucarji-v-klubu/{id}", async (int id, SmucarVKlubu smucarVKlubu, ISmucarskiKlubService service) =>
            {
                if (await service.UpdateSmucarVKlubuAsync(id, smucarVKlubu))
                    return Results.NoContent();
                return Results.NotFound();
            })
            .WithName("UpdateSmucarVKlubu")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Posodobi clanstvo v klubu",
                Description = "Posodobi podatke o clanstvu smucarja v klubu",
                Tags = new List<OpenApiTag> { new() { Name = "Clani kluba" } }
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<SmucarVKlubu>(MediaTypeNames.Application.Json);

            app.MapDelete("/api/smucarji-v-klubu/{id}", async (int id, ISmucarskiKlubService service) =>
            {
                if (await service.DeleteSmucarVKlubuAsync(id))
                    return Results.NoContent();
                return Results.NotFound();
            })
            .WithName("DeleteSmucarVKlubu")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Izbrisi clanstvo",
                Description = "Izbrisi clanstvo smucarja v klubu",
                Tags = new List<OpenApiTag> { new() { Name = "Clani kluba" } }
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            app.MapGet("/api/klubi/most-smucarji", async (ISmucarskiKlubService service) =>
                Results.Ok(await service.GetKlubWithMostSkiersAsync()))
            .WithName("GetKlubWithMostSkiers")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi klub z najvec smucarji",
                Description = "Dobi klub z najvecjim stevilom smucarojev",
                Tags = new List<OpenApiTag> { new() { Name = "Statistika klubov" } }
            })
            .Produces<Klub>(StatusCodes.Status200OK);

            app.MapGet("/api/smucarji/oldest", async (ISmucarskiKlubService service) =>
                Results.Ok(await service.GetOldestSkierAsync()))
            .WithName("GetOldestSkier")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi najstarejsega smucarja",
                Description = "Dobi najstarejsega smucarja v bazi",
                Tags = new List<OpenApiTag> { new() { Name = "Statistika smucarjev" } }
            })
            .Produces<Smucar>(StatusCodes.Status200OK);

            app.MapGet("/api/smucarji/average-age", async (ISmucarskiKlubService service) =>
                Results.Ok(await service.GetAverageAgeAsync()))
            .WithName("GetAverageAge")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Pridobi povprecno  starost",
                Description = "Izracuna povprecno starost smucarjev",
                Tags = new List<OpenApiTag> { new() { Name = "Statistika Smucarjev" } }
            })
            .Produces<double>(StatusCodes.Status200OK);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<SmucarskiKlubDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ISmucarskiKlubService, SmucarskiKlubService>();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Smucarski Klub API",
                    Version = "v1",
                    Description = "An API za smuèarje in smuèarske klube",
                    Contact = new OpenApiContact
                    {
                        Name = "API Support",
                        Email = "support@smucarskiklub.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddCors();
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<SmucarskiKlubDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options => {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Smucarski Klub API V1");
                    options.RoutePrefix = "swagger";
                    options.DocumentTitle = "Smucarski Klub API Documentation";
                });
            }
            app.UseCors("AllowBlazorClient");
            app.UseHttpsRedirection();
            app.UseCors();
            app.MapSmucarskiKlubEndpoints();

            app.Run();
        }
    }
}