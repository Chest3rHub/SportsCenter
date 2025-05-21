import API_URL from "../appConfig";

export default async function getTrainerBusyTimes(trainerId, date) {
    const isoDate = date.toISOString(); 

    const response = await fetch(`${API_URL}/Employees/${trainerId}/busy-times?date=${isoDate}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Błąd pobierania zajętości trenera: ${response.status} ${errorText}`);
    }

    const data = await response.json();
    return data;
}
