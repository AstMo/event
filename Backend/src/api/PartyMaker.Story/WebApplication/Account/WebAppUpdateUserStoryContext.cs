using PartyMaker.Common.Request;
using PartyMaker.Story.CommonStories;
using System;

namespace PartyMaker.Story.WebApplication.Account
{
    public class WebAppUpdateUserStoryContext : UpdateEntityStoryContext
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public Guid ImageId { get; set; }

        public string Phone { get; set; }

        public string Birthday { get; set; }
    }
}
