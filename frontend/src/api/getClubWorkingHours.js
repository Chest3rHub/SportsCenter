import API_URL from "../appConfig";

export default async function getClubWorkingHours(weekOffset = 0) {
    try {
        const response = await fetch(`${API_URL}/SportsCenterManagement/get-sports-club-working-hours-for-week?weekOffset=${weekOffset}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include'
        });
        
        if (!response.ok) {
          //  throw new Error('Network problem');
        }
        
        return response;
    } catch (error) {
       // console.error('Error fetching club working hours:', error);
       // throw error;
    }
}