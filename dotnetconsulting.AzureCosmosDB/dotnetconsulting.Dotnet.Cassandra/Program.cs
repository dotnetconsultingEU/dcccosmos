﻿using Cassandra;
using Cassandra.Mapping;
using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace dotnetconsulting.Dotnet.Cassandra
{
    public class Program
    {
        // Cassandra Cluster Configs      
        private const string UserName = "<FILLME>";
        private const string Password = "<FILLME>";
        private const string CassandraContactPoint = "<FILLME>";  // DnsName  
        private static int CassandraPort = 10350;

#pragma warning disable IDE0060 // Remove unused parameter
        public static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            // Connect to cassandra cluster  (Cassandra API on Azure Cosmos DB supports only TLSv1.2)
            var options = new SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);
            options.SetHostNameResolver((ipAddress) => CassandraContactPoint);
            Cluster cluster = Cluster.Builder()
                .WithCredentials(UserName, Password)
                .WithPort(CassandraPort)
                .AddContactPoint(CassandraContactPoint)
                .WithSSL(options)
                .Build();
            ISession session = cluster.Connect();

            // Creating KeySpace and table
            session.Execute("DROP KEYSPACE IF EXISTS uprofile");
            session.Execute("CREATE KEYSPACE uprofile WITH REPLICATION = { 'class' : 'NetworkTopologyStrategy', 'datacenter1' : 1 };");
            Console.WriteLine(String.Format("created keyspace uprofile"));
            session.Execute("CREATE TABLE IF NOT EXISTS uprofile.user (user_id int PRIMARY KEY, user_name text, user_bcity text)");
            Console.WriteLine(String.Format("created table user"));

            session = cluster.Connect("uprofile");
            IMapper mapper = new Mapper(session);

            // Inserting Data into user table
            mapper.Insert(new User(1, "LyubovK", "Dubai"));
            mapper.Insert(new User(2, "JiriK", "Toronto"));
            mapper.Insert(new User(3, "IvanH", "Mumbai"));
            mapper.Insert(new User(4, "LiliyaB", "Seattle"));
            mapper.Insert(new User(5, "JindrichH", "Buenos Aires"));
            Console.WriteLine("Inserted data into user table");

            Console.WriteLine("Select ALL");
            Console.WriteLine("-------------------------------");
            foreach (User user in mapper.Fetch<User>("Select * from user"))
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("Getting by id 3");
            Console.WriteLine("-------------------------------");
            User userId3 = mapper.FirstOrDefault<User>("Select * from user where user_id = ?", 3);
            Console.WriteLine(userId3);

            // Clean up of Table and KeySpace
            session.Execute("DROP table user");
            session.Execute("DROP KEYSPACE uprofile");

            // Wait for enter key before exiting  
            Console.ReadLine();
        }

        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
    }
}