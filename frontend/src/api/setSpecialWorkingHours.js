import API_URL from "../appConfig";

export default async function setSpecialWorkingHours(date, openHour, closeHour) {
    try {
        const response = await fetch(`${API_URL}/SportsCenterManagement/Set-special-sports-center-working-hours`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify({
                date: `${date}T${openHour}`,
                openHour,
                closeHour
            })
        });
        if (!response.ok) throw new Error('Response error:');
        
        return response;
    } catch (error) {
        console.error('Error setting special working hours:', error);
        throw error;
    }
}