﻿namespace IdentityService.Core.Exceptions;

public class ConflictException(string message) : Exception(message);