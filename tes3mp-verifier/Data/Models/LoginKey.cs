﻿using System;
using System.ComponentModel.DataAnnotations;

namespace tes3mp_verifier.Data.Models
{
  public class LoginKey
  {
    [Key]
    public string Key { get; set; }
    public int UserId { get; set; }
    public DateTime Created { get; set; }

    public virtual User User { get; set; }
  }
}
