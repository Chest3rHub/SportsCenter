import API_URL from "../appConfig";

export default async function getCourtReservations(courtId, date) {
    try {
        if (!(date instanceof Date) || isNaN(date)) {
            throw new Error('Invalid date');
        }
        
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const formattedDate = `${year}-${month}-${day}`;
        const response = await fetch(`${API_URL}/Reservation/get-court-reservations?courtId=${courtId}&date=${formattedDate}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include'
        });
        
        if (!response.ok) {
            throw new Error('Network problem');
        }
        
        return response;
    } catch (error) {
        console.error('Error fetching court reservations', error);
        throw error;
    }
}