
var builder = DistributedApplication.CreateBuilder(args);

// The following line requires that the 'UserManagement.Api' project is referenced by this project.
// Make sure that:
// 1. The 'UserManagement.Api' project exists in your solution.
// 2. The 'UserManagement.Api' project is referenced by the 'UserManagement.AppHost' project.
// 3. The namespace 'UserManagement.Api' is correct and matches the actual namespace in the referenced project.

builder.AddProject<Projects.UserManagement>("usermanagement");

builder.Build().Run();
