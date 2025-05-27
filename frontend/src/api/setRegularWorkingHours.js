import API_URL from "../appConfig";

export default async function setRegularWorkingHours(dayOfWeek, openHour, closeHour) {
    try {
        const polishDayMapping = {
            'monday': 'poniedzialek',
            'tuesday': 'wtorek',
            'wednesday': 'sroda',
            'thursday': 'czwartek',
            'friday': 'piatek',
            'saturday': 'sobota',
            'sunday': 'niedziela'
        };
        const polishDayOfWeek = polishDayMapping[dayOfWeek.toLowerCase()] || dayOfWeek; 
        const response = await fetch(`${API_URL}/SportsCenterManagement/Set-sports-center-working-hours`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify({
                dayOfWeek: polishDayOfWeek,
                openHour,
                closeHour
            })
        });
        if (!response.ok) {
        const errorData = await response.json();
        const error = new Error(errorData.message || 'Failed to set regular hours');
        error.response = response;
        error.data = errorData;
        throw error;
        }
        
        return response;
    } catch (error) {
        console.error('Error setting regular working hours: ', error);
        throw error;
    }
}