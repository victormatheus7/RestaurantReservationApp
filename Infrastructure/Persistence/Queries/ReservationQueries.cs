using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Queries
{
    [ExcludeFromCodeCoverage]
    public static class ReservationQueries
    {
        public static string ListReservations =>
            @"SELECT id AS ""Id"", 
                     creator_email AS ""CreatorEmail"",
                     date AS ""Date"",
                     number_seats AS ""NumberSeats"",
                     location_preference_id AS ""LocationPreference"",
                     observation AS ""Observation""
              FROM public.reservation";

        public static string SaveReservation =>
            @"INSERT INTO public.reservation (id, creator_email, date, number_seats, location_preference_id, observation) 
			  VALUES (@Id, @CreatorEmail, @Date, @NumberSeats, @LocationPreference, @Observation)";

        public static string UpdateReservation =>
            @"UPDATE public.reservation 
              SET creator_email = @CreatorEmail, 
			      date = @Date, 
				  number_seats = @NumberSeats, 
				  location_preference_id = @LocationPreference, 
				  observation = @Observation
              WHERE id = @Id";

        public static string DeleteReservation => "DELETE FROM public.reservation WHERE id = @Id";

        public static string CountReservationsByDateRange =>
            @"SELECT COUNT(*)
              FROM public.reservation
              WHERE date >= @StartDate AND date <= @EndDate";
    }
}
