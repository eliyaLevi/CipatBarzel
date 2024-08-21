using Microsoft.AspNetCore.SignalR;
namespace CipatBarzel.Hubs
{
	public class RealTime : Hub
	{
		//פונקציה לשליחת התראה על התקפה בזמן אמת לכל הלקוחות המחוברים
		public async Task AttackAlert(int id, int rt, string name)
		{
			//שליחת הודעה לכל הלקוחות דרך המתודה SendAsync עם הפרמטרים האלו
			await Clients.All.SendAsync("RedAlert", id, rt, name);
		}
	}
}
