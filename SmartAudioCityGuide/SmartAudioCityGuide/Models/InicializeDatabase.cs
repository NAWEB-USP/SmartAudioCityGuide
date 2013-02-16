using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SmartAudioCityGuide.Controllers;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace SmartAudioCityGuide.Models
{
    public class InicializeDatabase : DropCreateDatabaseAlways<SmartAudioCityGuideEntities>
    {
        private static CryptographyController criptographyController = new CryptographyController();


        protected override void Seed(SmartAudioCityGuideEntities context)
        {

            /*new MyContext();*/
            base.Seed(context);

            Users userTest = new Users();
            userTest.name = "Wonders";
            userTest.password = criptographyController.getMD5Hash("123456");
            userTest.userName = "smartaudiocityguide@gmail.com";
            userTest.authenticate = 1;

            context.users.Add(userTest);

            Codes code = new Codes();
            code.code = "wonders";

            context.codes.Add(code);
            context.SaveChanges();




        }
    }
    /*
    class MyContext : DbContext
    {
        static MyContext()
    {
        Database.SetInitializer(
            new DropCreateDatabaseAlways<MyContext>());
    }

        public MyContext()
            : base()
        {
        }

        // Add DbSet properties here
    }



    class CreateMySqlDatabaseIfNotExists<TContext>
    : IDatabaseInitializer<TContext>
        where TContext : DbContext
    {
        public void InitializeDatabase(TContext context)
        {
            if (context.Database.Exists())
            {
                if (!context.Database.CompatibleWithModel(false))
                {
                    throw new InvalidOperationException(
                        "The model has changed!");
                }
            }
            else
            {
                CreateMySqlDatabase(context);
            }
        }

        private void CreateMySqlDatabase(TContext context)
        {
            try
            {
                // Create as much of the database as we can
                context.Database.Create();

                // No exception? Don't need a workaround
                return;
            }
            catch (MySqlException ex)
            {
                // Ignore the parse exception
                if (ex.Number != 1064)
                {
                    throw;
                }
            }

            // Manually create the metadata table
            using (var connection = ((MySqlConnection)context
                .Database.Connection).Clone())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
    @"
CREATE TABLE __MigrationHistory (
    MigrationId mediumtext NOT NULL,
    Model mediumblob NOT NULL,
    ProductVersion mediumtext NOT NULL);
 
ALTER TABLE __MigrationHistory
ADD PRIMARY KEY (MigrationId(255));
 
INSERT INTO __MigrationHistory (
    MigrationId,
    Model,
    ProductVersion)
VALUES (
    'InitialCreate',
    @Model,
    @ProductVersion);
";
                command.Parameters.AddWithValue(
                    "@Model",
                    GetModel(context));
                command.Parameters.AddWithValue(
                    "@ProductVersion",
                    GetProductVersion());

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private byte[] GetModel(TContext context)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(
                    memoryStream,
                    CompressionMode.Compress))
                using (var xmlWriter = XmlWriter.Create(
                    gzipStream,
                    new XmlWriterSettings { Indent = true }))
                {
                    EdmxWriter.WriteEdmx(context, xmlWriter);
                }

                return memoryStream.ToArray();
            }
        }

        private string GetProductVersion()
        {
            return typeof(DbContext).Assembly
                .GetCustomAttributes(false)
                .OfType<AssemblyInformationalVersionAttribute>()
                .Single()
                .InformationalVersion;
        }
    }*/

}