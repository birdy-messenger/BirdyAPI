using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI
{
    public static class Configurations
    {
        //TODO:2 Don't commit connection string to CVS

        public static string OurEmailAddress = "birdy-noreply@birdy.com";

        public static string EmailConfirmMessage =
            "Hi! <br>You need to confirm your email to finish registration in Birdy messenger." +
            " Click on this link : ";
    }
}
