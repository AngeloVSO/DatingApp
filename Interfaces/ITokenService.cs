﻿using DatingApp.Entities;

namespace DatingApp.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
