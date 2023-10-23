using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Queries
{
    [ExcludeFromCodeCoverage]
    public static class ReservationQueries
    {
        public static string ListReservations =>
            @"SELECT [Id], 
                     [CreatorEmail],
                     [Date],
                     [NumberSeats],
                     [LocationPreference],
                     [Observation],
              FROM [Reservation]";

        public static string SaveReservation =>
            @"INSERT INTO [Reservation] ([Id], [CreatorEmail], [Date], [NumberSeats], [LocationPreference], [Observation]) 
			  VALUES (@Id, @CreatorEmail, @Date, @NumberSeats, @LocationPreference, @Observation)";

        public static string UpdateReservation =>
            @"UPDATE [Reservation] 
              SET [CreatorEmail] = @CreatorEmail, 
			      [Date] = @Date, 
				  [NumberSeats] = @NumberSeats, 
				  [LocationPreference] = @LocationPreference, 
				  [Observation] = @Observation
              WHERE [Id] = @Id";

        public static string DeleteReservation => "DELETE FROM [Reservation] WHERE [Id] = @Id";

        public static string CountReservationsByDateRange =>
            @"SELECT COUNT(*),
              FROM [Reservation]
              WHERE [Date] >= @StartDate AND [Date] <= @EndDate";
    }
}
