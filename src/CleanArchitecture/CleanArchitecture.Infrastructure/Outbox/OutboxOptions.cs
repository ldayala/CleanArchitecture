﻿
namespace CleanArchitecture.Infrastructure.Outbox
{
    public  class OutboxOptions
    {
        public int IntervalInSeconds { get; init; }
        public int BatchSize { get; init; }  //para obtener solo x record
    }
}
