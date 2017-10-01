// If there is no database, add one and seed with test data

using HealthcareNetCoreSample.Models;
using System;
using System.Linq;

namespace HealthcareNetCoreSample.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PatientContext context)
        {
            context.Database.EnsureCreated();

            // Look for any patients.
            if (context.Patients.Any())
            {
                return;   // DB has been seeded
            }

            // Else, seed new database
            var patients = new Patient[]
            {
            new Patient{FirstMidName="Carson",LastName="Alexander",CreatedDate=DateTime.Parse("2005-09-01")},
            new Patient{FirstMidName="Meredith",LastName="Alonso",CreatedDate=DateTime.Parse("2002-09-01")},
            new Patient{FirstMidName="Arturo",LastName="Anand",CreatedDate=DateTime.Parse("2003-09-01")},
            new Patient{FirstMidName="Gytis",LastName="Barzdukas",CreatedDate=DateTime.Parse("2002-09-01")},
            };
            foreach (Patient p in patients)
            {
                context.Patients.Add(p);
            }
            context.SaveChanges();

            var insProviders = new InsProvider[]
            {
            new InsProvider{InsProviderID=1050,InsProviderName="Blue Cross"},
            new InsProvider{InsProviderID=4022,InsProviderName="Cigna"},
            new InsProvider{InsProviderID=4041,InsProviderName="Aetna"}

            };
            foreach (InsProvider i in insProviders)
            {
                context.InsProviders.Add(i);
            }
            context.SaveChanges();

            var claims = new Claim[]
            {
            new Claim{PatientID=1,InsProviderID=1050,ClaimStatus=ClaimStatus.open, AmountOwed=52525},
            new Claim{PatientID=1,InsProviderID=4022,ClaimStatus=ClaimStatus.open, AmountOwed=23456},
            new Claim{PatientID=1,InsProviderID=4041,ClaimStatus=ClaimStatus.closed, AmountOwed=125},
            new Claim{PatientID=2,InsProviderID=1050,ClaimStatus=ClaimStatus.open, AmountOwed=63787},
            new Claim{PatientID=2,InsProviderID=4022,ClaimStatus=ClaimStatus.waiting, AmountOwed=528906},
            new Claim{PatientID=3,InsProviderID=1050,ClaimStatus=ClaimStatus.open, AmountOwed=43},
            new Claim{PatientID=3,InsProviderID=4022,ClaimStatus=ClaimStatus.waiting, AmountOwed=2567},
            new Claim{PatientID=4,InsProviderID=1050,ClaimStatus=ClaimStatus.open, AmountOwed=5825},
            new Claim{PatientID=4,InsProviderID=4022,ClaimStatus=ClaimStatus.waiting, AmountOwed=456},
            new Claim{PatientID=4,InsProviderID=4022,ClaimStatus=ClaimStatus.open, AmountOwed=1547453},
            new Claim{PatientID=4,InsProviderID=4022,ClaimStatus=ClaimStatus.closed, AmountOwed=4567}

            };
            foreach (Claim c in claims)
            {
                context.Claims.Add(c);
            }
            context.SaveChanges();

        }
    }
}
