using System;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;

namespace MVCBlog.Business.Test;

public class InMemoryDatabaseFactory
{
    private readonly string databaseName;

    public InMemoryDatabaseFactory()
    {
        this.databaseName = Guid.NewGuid().ToString();
    }

    public InMemoryDatabaseFactory(string databaseName)
    {
        this.databaseName = databaseName;
    }

    public EFUnitOfWork CreateContext()
    {
        var options = new DbContextOptionsBuilder<EFUnitOfWork>()
            .UseInMemoryDatabase(databaseName: this.databaseName)
            .Options;

        return new EFUnitOfWork(options);
    }
}