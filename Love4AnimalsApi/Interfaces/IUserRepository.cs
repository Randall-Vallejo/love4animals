using System;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface IUserRepository
{
    public User getUser();
}
