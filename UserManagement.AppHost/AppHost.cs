var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.UserManagement>("usermanagement");

builder.Build().Run();
