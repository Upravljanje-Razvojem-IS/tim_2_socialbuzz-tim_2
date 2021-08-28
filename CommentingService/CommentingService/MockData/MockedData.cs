using CommentingService.Entities;
using System;
using System.Collections.Generic;

namespace CommentingService.MockData
{
    public static class MockedData
    {
        public static readonly List<User> Users = new List<User>
        {
            new User() { Id = Guid.Parse("cc0e623f-444c-4d54-9b6e-402f643d95a9"), Username = "Marijana" },
            new User() { Id = Guid.Parse("7159bb24-ab2b-4f6c-be50-2b2bc43bbded"), Username = "Andjela" },
            new User() { Id = Guid.Parse("ce90f1f1-d75e-4823-8066-b2464c7c36e3"), Username = "Kristina" },
        };

        public static readonly List<Post> Posts= new List<Post>
        {
            new Post() { Id = Guid.Parse("c45ed862-80bd-438a-a6c8-c52544e3662e"), Title = "Post1", Description = "Description of post 1" },
            new Post() { Id = Guid.Parse("cc459d68-43b6-455c-abbf-0083d48dfe89"), Title = "Post2", Description = "Description of post 2" },
            new Post() { Id = Guid.Parse("7a1414f3-203b-485a-b5b7-e5e84f9be0cb"), Title = "Post3", Description = "Description of post 3"},
        };
    }
}
