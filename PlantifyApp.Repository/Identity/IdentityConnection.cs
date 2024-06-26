﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PlantifyApp.Core.Models;
using System.Xml.Linq;


namespace PlantifyApp.Repository.Identity
{
	public class IdentityConnection:IdentityDbContext<ApplicationUser>
	{
        public IdentityConnection(DbContextOptions<IdentityConnection> options):base(options)
        {
            
        }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Likes> likes { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Plants> Plants { get; set; }

    }
}
