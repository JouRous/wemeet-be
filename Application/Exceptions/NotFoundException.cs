using System;

namespace Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object id)
            : base($"Entity {name} with Id: {id} not found!")
        { }
    }
}