using System;

namespace BirdyAPI.Dto
{
    public class UserSessionDto
    {
        public UserSessionDto(int id, Guid token)
        {
            Id = id;
            Token = token;  
        }
        public UserSessionDto() { }
        public int Id { get; set; }
        public Guid Token { get; set; }
    }
}
