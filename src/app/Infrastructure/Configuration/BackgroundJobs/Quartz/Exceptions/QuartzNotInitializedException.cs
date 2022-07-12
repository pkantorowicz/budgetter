using System;

namespace Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz.Exceptions;

public class QuartzNotInitializedException : ApplicationException
{
    public QuartzNotInitializedException(string message) : base(message)
    {
    }
}