﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Core.Objects
{
    public class DomainException : Exception
    {
        public DomainException()
        {
        }

        public DomainException(string message)
        {

        }

        public DomainException(string message, Exception innerException)
        {

        }

    }
}
